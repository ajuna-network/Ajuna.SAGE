using Substrate.AjunaSolo.NET.NetApiExt.Client;
using Substrate.Integration;
using Substrate.NET.Schnorrkel.Keys;
using Substrate.NetApi.Model.Extrinsics;
using Substrate.NetApi.Model.Types;

namespace Substrate.NetApi.TestNode
{
    public abstract class NodeTest
    {
        private string _webSocketUrl;

        private Account _baseAccount;

        protected SubstrateNetwork _client;

        // Secret Key URI `//Alice` is account:
        // Secret seed:      0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a
        // Public key(hex):  0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // Account ID:       0xd43593c715fdd31c61141abd04a99fd6822c8558854ccde39a5684e7a56da27d
        // SS58 Address:     5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY
        public MiniSecret MiniSecretAlice => new MiniSecret(Utils.HexToByteArray("0xe5be9a5092b81bca64be81d212e7f2f9eba183bb7a90954f7b76361f6edb5c0a"), ExpandMode.Ed25519);

        public Account Alice => Account.Build(KeyType.Sr25519, MiniSecretAlice.ExpandToSecret().ToEd25519Bytes(), MiniSecretAlice.GetPair().Public.Key);

        // Secret Key URI `//Bob` is account:
        // Secret seed:      0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89
        // Public key(hex):  0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
        // Account ID:       0x8eaf04151687736326c9fea17e25fc5287613693c912909cb226aa4794f26a48
        // SS58 Address:     5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty
        public MiniSecret MiniSecretBob => new MiniSecret(Utils.HexToByteArray("0x398f0c28f98885e046333d4a41c19cee4c37368a9832c6502f6cfd182e2aef89"), ExpandMode.Ed25519);

        public Account Bob => Account.Build(KeyType.Sr25519, MiniSecretBob.ExpandToSecret().ToEd25519Bytes(), MiniSecretBob.GetPair().Public.Key);

        public MiniSecret MiniSecretPlayer1 => new MiniSecret(Utils.HexToByteArray("0x0a52c9148e690566d64e080ffafdda6111d3404ee122d7caf5dd7d25573b07f5"), ExpandMode.Ed25519);
        public Account Player1 => Account.Build(KeyType.Sr25519, MiniSecretPlayer1.ExpandToSecret().ToEd25519Bytes(), MiniSecretPlayer1.GetPair().Public.Key);

        public MiniSecret MiniSecretPlayer2 => new MiniSecret(Utils.HexToByteArray("0xa07e56ddadf45a99b71861c0e42493b254a53f9ffcdaec81de423bbd178becae"), ExpandMode.Ed25519);
        public Account Player2 => Account.Build(KeyType.Sr25519, MiniSecretPlayer2.ExpandToSecret().ToEd25519Bytes(), MiniSecretPlayer2.GetPair().Public.Key);

        public NodeTest(string url = "ws://127.0.0.1:9944", Account? account = null)
        {
            _webSocketUrl = url;
            _baseAccount = account ?? Alice;
        }

        [SetUp]
        public async Task ConnectAsync()
        {
            await _client.ConnectAsync(true, true, CancellationToken.None);
        }

        [TearDown]
        public async Task CloseAsync()
        {
            await _client.DisconnectAsync();
        }

        [OneTimeSetUp]
        public void CreateClient()
        {
            _client = new SubstrateNetwork(Alice, _webSocketUrl);
        }

        [OneTimeTearDown]
        public void DisposeClient()
        {
            _client.SubstrateClient.Dispose();
        }

        /// <summary>
        /// Return the 20th hash block from now (totally arbitrary)
        /// </summary>
        /// <returns></returns>
        protected async Task<byte[]> GivenBlockAsync()
        {
            var lastBlockData = await _client.SubstrateClient.Chain.GetBlockAsync();
            var lastBlockNumber = lastBlockData.Block.Header.Number.Value;

            var blockNumber = new Model.Types.Base.BlockNumber();
            blockNumber.Create((uint)(lastBlockNumber - 20));
            return (await _client.SubstrateClient.Chain.GetBlockHashAsync(blockNumber)).Bytes;
        }

        /// <summary>
        /// Execute a transaction and wait for the result
        /// </summary>
        /// <typeparam name="TOption"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="account"></param>
        /// <param name="option"></param>
        /// <param name="transactionFunction"></param>
        /// <param name="avatarsErrors"></param>
        /// <param name="timeoutDuration"></param>
        /// <returns></returns>
        public async Task<(bool, ExtrinsicInfo)> ExecuteTransactionTestAsync<TEnum>(
            Account account,
            Method method,
            TEnum? avatarsErrors,
            TimeSpan timeoutDuration) where TEnum : struct, Enum
        {
            var tcs = new TaskCompletionSource<(bool, ExtrinsicInfo)>();
            var subscriptionId = "";

            void OnExtrinsicUpdated(string subId, ExtrinsicInfo extrinsicInfo)
            {
                if (subId == subscriptionId && (extrinsicInfo.HasEvents || extrinsicInfo.Error != null))
                {
                    tcs.SetResult((true, extrinsicInfo));
                }
            }

            _client.ExtrinsicManager.ExtrinsicUpdated += OnExtrinsicUpdated;
            subscriptionId = await _client.GenericExtrinsicAsync(account, method.ModuleName + "." + method.CallName, method, 100, CancellationToken.None);
            Assert.That(subscriptionId, Is.Not.Null);

            var finished = await Task.WhenAny(tcs.Task, Task.Delay(timeoutDuration));
            _client.ExtrinsicManager.ExtrinsicUpdated -= OnExtrinsicUpdated;

            Assert.That(finished, Is.EqualTo(tcs.Task), "Test timed out waiting for final callback");
            var taskResult = await tcs.Task;

            Assert.That(taskResult.Item1, Is.True);

            Assert.That(taskResult.Item2.SystemExtrinsicEvent(out var systemExtrinsicEvent, out var errorMsg), Is.True);
            Assert.That(systemExtrinsicEvent, Is.Not.Null);

            if (avatarsErrors.HasValue)
            {
                Assert.That(systemExtrinsicEvent, Is.EqualTo(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicFailed));
                Assert.That(errorMsg, Is.Not.Null);
                var errorArray = errorMsg.Split(";");
                if (errorArray.Length < 4)
                {
                    Assert.That(errorArray.Contains(avatarsErrors.ToString()), Is.True);
                }
                else
                {
                    var errorValue = (TEnum)Enum.ToObject(typeof(TEnum), BitConverter.ToUInt32(Utils.HexToByteArray(errorArray[3]), 0));
                    Assert.That(errorValue, Is.EqualTo(avatarsErrors.Value));
                }
            }
            else
            {
                Assert.That(systemExtrinsicEvent, Is.EqualTo(Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_system.pallet.Event.ExtrinsicSuccess));
                Assert.That(errorMsg, Is.Null);
            }

            return taskResult;
        }

        /// <summary>
        /// Mint
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mintPayment"></param>
        /// <param name="avatarsErrors"></param>
        /// <returns></returns>
        //public async Task<(bool IsSuccess, ExtrinsicInfo ExtrinsicInfo)> MintAsync(Account account, MintPayment mintPayment, AwesomeAvatarsErrors? avatarsErrors)
        //{
        //    var method = AwesomeAvatarsCalls.Mint(new MintOptionSharp(mintPayment, PackType.Special, MintPackSize.Six).ToSubstrate());
        //    return await ExecuteTransactionTestAsync(account, method, avatarsErrors, TimeSpan.FromSeconds(90));
        //}

        /// <summary>
        /// Remove price
        /// </summary>
        /// <param name="account"></param>
        /// <param name="avatarId"></param>
        /// <param name="avatarsErrors"></param>
        /// <returns></returns>
        //public async Task<(bool IsSuccess, ExtrinsicInfo ExtrinsicInfo)> RemovePriceAsync(Account account, H256 avatarId, AwesomeAvatarsErrors? avatarsErrors)
        //{
        //    var method = AwesomeAvatarsCalls.RemovePrice(avatarId);
        //    return await ExecuteTransactionTestAsync(account, method, avatarsErrors, TimeSpan.FromSeconds(90));
        //}
    }
}