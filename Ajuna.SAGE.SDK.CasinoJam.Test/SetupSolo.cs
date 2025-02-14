using Newtonsoft.Json.Linq;
using Substrate.AjunaSolo.NET.NetApiExt.Client;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_runtime;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage;
using Substrate.Integration;
using Substrate.Integration.Call;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.Integration.Model;
using Substrate.Integration.Model.PalletTournament;
using Substrate.NET.Schnorrkel.Keys;
using Substrate.NetApi;
using Substrate.NetApi.Model.Rpc;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using Substrate.NetApi.TestNode;
using System.Numerics;
using System.Xml.Linq;
using Ajuna.SAGE.SDK.Model.CasinoJamSeasons;
using Ajuna.SAGE.SDK.Model.CasinoJamSage;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.enums;

namespace Ajuna.TestSuite
{
    public class SetupSolo : NodeTest
    {
        private Account _sudo;
        private Account _organizer;
        private Account _user1;
        private Account _user2;

        private ushort _seasonId;

        [SetUp]
        public void Setup()
        {
            _sudo = Alice;
            _organizer = Alice;
            _user1 = Player1;
            _user2 = Player2;
            _seasonId = 1;
        }

        [Test]
        public void CheckAddresses()
        {
            Assert.That(_sudo.ToString(), Is.EqualTo(Alice.ToString()));
            Assert.That(_organizer.ToString(), Is.EqualTo(Alice.ToString()));
            Assert.That(_user1.ToString(), Is.EqualTo("5DZJM6Xjr9toZHPj5Z59ZCoUviRNxCWRAv5ZJzbX5LqVYnS5"));
            Assert.That(_user2.ToString(), Is.EqualTo("5EU7fYbJXpJTrq7xEVaQSK7TWSTAQCdbnALC1zthJD9TrfGm"));

        }

        [Test, Order(1)]
        public async Task SetOrganizerTestAsync()
        {
            var subscriptionId = await _client.SetOrganizerAsync(_sudo, _organizer.ToAccountId32(), 1, CancellationToken.None);
            Assert.That(subscriptionId, Is.Not.Null);

            var tcs = new TaskCompletionSource<bool>();
            void OnExtrinsicUpdated(string subId, ExtrinsicInfo extrinsicInfo)
            {
                if (subId == subscriptionId && extrinsicInfo.TransactionEvent == TransactionEvent.BestChainBlockIncluded)
                {
                    tcs.TrySetResult(true);
                }
            }
            _client.ExtrinsicManager.ExtrinsicUpdated += OnExtrinsicUpdated;
            await tcs.Task;
            _client.ExtrinsicManager.ExtrinsicUpdated -= OnExtrinsicUpdated;
        }

        [Test, Order(2)]
        public async Task FundPartyTestAsync()
        {
            var enumCalls = new List<EnumRuntimeCall>
            {
                PalletBalances.BalancesTransferKeepAlive(_organizer.ToAccountId32(), 10000 * SubstrateNetwork.DECIMALS),
                PalletBalances.BalancesTransferKeepAlive(_user1.ToAccountId32(), 1_001_000 * SubstrateNetwork.DECIMALS),
                PalletBalances.BalancesTransferKeepAlive(_user2.ToAccountId32(), 10000 * SubstrateNetwork.DECIMALS),
            };

            var subscriptionId = await _client.BatchAllAsync(Alice, enumCalls, 2, CancellationToken.None);
            Assert.That(subscriptionId, Is.Not.Null);

            var tcs = new TaskCompletionSource<bool>();
            void OnExtrinsicUpdated(string subId, ExtrinsicInfo extrinsicInfo)
            {
                if (subId == subscriptionId && extrinsicInfo.TransactionEvent == TransactionEvent.BestChainBlockIncluded)
                {
                    tcs.TrySetResult(true);
                }
            }
            _client.ExtrinsicManager.ExtrinsicUpdated += OnExtrinsicUpdated;
            await tcs.Task;
            _client.ExtrinsicManager.ExtrinsicUpdated -= OnExtrinsicUpdated;
        }

        [Test, Order(3)]
        public async Task CreateSeasonTestAsync()
        {
            uint seasonId = 1;

            var seasonConfig = new SeasonConfigSharp()
            {
                Fee = new SeasonFeeConfigSharp()
                {
                    TransferAsset = 1 * SubstrateNetwork.DECIMALS,
                    BuyAssetMin = 1 * SubstrateNetwork.DECIMALS,
                    BuyPercent = 1,
                    UpgradeAssetInventory = 1 * SubstrateNetwork.DECIMALS,
                    UnlockTradeAsset = 1 * SubstrateNetwork.DECIMALS,
                    UnlockTransferAsset = 1 * SubstrateNetwork.DECIMALS,
                    StateTransitionBaseFee = 1 * SubstrateNetwork.DECIMALS,
                }
            };

            var seasonMetadata = new SeasonMetadataSharp()
            {
                Name = "Season 1",
                Description = "Testing Season"
            };

            var currentBlocknumber = await _client.GetBlocknumberAsync(CancellationToken.None);
            Assert.That(currentBlocknumber, Is.Not.Null);
            Assert.That(currentBlocknumber.HasValue, Is.True);

            var seasonSchedule = new SeasonScheduleSharp()
            {
                EarlyStart = currentBlocknumber.Value + 5,
                Start = currentBlocknumber.Value + 10,
                End = null,
            };

            var subscriptionId = await _client.UpdateSeasonAsync(_organizer, seasonId, seasonConfig, seasonMetadata, seasonSchedule, 1, CancellationToken.None);
            Assert.That(subscriptionId, Is.Not.Null);

            var tcs = new TaskCompletionSource<bool>();
            void OnExtrinsicUpdated(string subId, ExtrinsicInfo extrinsicInfo)
            {
                if (subId == subscriptionId && extrinsicInfo.TransactionEvent == TransactionEvent.BestChainBlockIncluded)
                {
                    tcs.TrySetResult(true);
                }
            }
            _client.ExtrinsicManager.ExtrinsicUpdated += OnExtrinsicUpdated;
            await tcs.Task;
            _client.ExtrinsicManager.ExtrinsicUpdated -= OnExtrinsicUpdated;
        }
    }
}