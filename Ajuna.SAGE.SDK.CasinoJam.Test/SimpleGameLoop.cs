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
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
using Substrate.NetApi.Model.Types.Metadata.Base;

namespace Ajuna.TestSuite
{
    public class SimpleGameLoop : NodeTest
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
        }

        [Test, Order(1)]
        public async Task CreatePlayer_User1_TestAsync()
        {
            var enumCasinoAction = new EnumCasinoAction();
            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Player, new BaseVoid());
            enumCasinoAction.Create(CasinoAction.Create, enumAssetType);

            var subscriptionId = await _client.StateTransitionAsync(_user1, enumCasinoAction, [], 1, CancellationToken.None);
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
        public async Task CreatePlayer_User2_TestAsync()
        {
            var enumCasinoAction = new EnumCasinoAction();
            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Player, new BaseVoid());
            enumCasinoAction.Create(CasinoAction.Create, enumAssetType);

            var subscriptionId = await _client.StateTransitionAsync(_user2, enumCasinoAction, [], 1, CancellationToken.None);
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
        public async Task CreateMachine_User1_TestAsync()
        {
            var enumCasinoAction = new EnumCasinoAction();
            var enumMachineType = new EnumMachineType();
            enumMachineType.Create(MachineType.Bandit);
            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Machine, enumMachineType);
            enumCasinoAction.Create(CasinoAction.Create, enumAssetType);

            var subscriptionId = await _client.StateTransitionAsync(_user1, enumCasinoAction, [], 1, CancellationToken.None);
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

        [Test, Order(4)]
        public async Task DepositMachine_User1_TestAsync()
        {
            var user1Assets = await GetAccountAssetsAsync(_user1, null);
            var machine = user1Assets.Where(p => p.Variant.AssetVariant == AssetVariant.Machine).FirstOrDefault();
            Assert.That(machine, Is.Not.Null);

            var enumCasinoAction = new EnumCasinoAction();
            var enumMachineType = new EnumMachineType();
            enumMachineType.Create(MachineType.Bandit);
            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Machine, enumMachineType);
            var enumTokenType = new EnumTokenType();
            enumTokenType.Create(TokenType.T100000);
            var tuple = new BaseTuple<EnumAssetType, EnumTokenType>(enumAssetType, enumTokenType);
            enumCasinoAction.Create(CasinoAction.Deposit, tuple);

            var subscriptionId = await _client.StateTransitionAsync(_user1, enumCasinoAction, [machine.Id], 1, CancellationToken.None);
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

        [Test, Order(5)]
        public async Task RentSeat_User1_TestAsync()
        {
            var user1Assets = await GetAccountAssetsAsync(_user1, null);
            var machine = user1Assets.Where(p => p.Variant.AssetVariant == AssetVariant.Machine).FirstOrDefault();
            Assert.That(machine, Is.Not.Null);

            var enumCasinoAction = new EnumCasinoAction();
            var enumMultiplierType = new EnumMultiplierType();
            enumMultiplierType.Create(MultiplierType.V1);
            enumCasinoAction.Create(CasinoAction.Rent, enumMultiplierType);

            var subscriptionId = await _client.StateTransitionAsync(_user1, enumCasinoAction, [machine.Id], 1, CancellationToken.None);
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

        [Test, Order(6)]
        public async Task DepositPlayer_User2_TestAsync()
        {
            // make sure to use the right user.
            var user = _user2;

            var user1Assets = await GetAccountAssetsAsync(user, null);
            var human = user1Assets.Where(p => 
                   p.Variant.AssetVariant == AssetVariant.Player 
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                .FirstOrDefault();
            Assert.That(human, Is.Not.Null);

            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Player, new BaseVoid());

            var enumTokenType = new EnumTokenType();
            enumTokenType.Create(TokenType.T100);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Deposit, 
                new BaseTuple<EnumAssetType, EnumTokenType>(enumAssetType, enumTokenType));

            var subscriptionId = await _client.StateTransitionAsync(user, enumCasinoAction, [human.Id], 1, CancellationToken.None);
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

        [Test, Order(6)]
        public async Task ReserveSeat_User2_TestAsync()
        {
            // make sure to use the right user.
            var user = _user1; // TODO: change to an other player!!

            var user1Assets = await GetAccountAssetsAsync(user, null);
            var human = user1Assets.Where(p =>
                   p.Variant.AssetVariant == AssetVariant.Player
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                .FirstOrDefault();
            Assert.That(human, Is.Not.Null);

            // take seat from user 2
            var seat = (await GetAccountAssetsAsync(_user1, null)).Where(p =>
                   p.Variant.AssetVariant == AssetVariant.Seat)
                .FirstOrDefault();
            Assert.That(seat, Is.Not.Null);

            var enumMultiplierType = new EnumMultiplierType();
            enumMultiplierType.Create(MultiplierType.V1);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Reserve, enumMultiplierType);

            var subscriptionId = await _client.StateTransitionAsync(user, enumCasinoAction, [human.Id, seat.Id], 1, CancellationToken.None);
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

        [Test, Order(6)]
        public async Task Gamble_User1_TestAsync()
        {
            // make sure to use the right user.
            var user = _user1; // TODO: change to an other player!!

            var user1Assets = await GetAccountAssetsAsync(user, null);
            var human = user1Assets.Where(p =>
                   p.Variant.AssetVariant == AssetVariant.Player
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                .FirstOrDefault();
            Assert.That(human, Is.Not.Null);

            var tracker = user1Assets.Where(p =>
                   p.Variant.AssetVariant == AssetVariant.Player
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Tracker)
                .FirstOrDefault();
            Assert.That(tracker, Is.Not.Null);

            var seat = user1Assets.Where(p =>
               p.Variant.AssetVariant == AssetVariant.Seat)
                .FirstOrDefault();
            Assert.That(seat, Is.Not.Null);

            var machine = user1Assets.Where(p =>
               p.Variant.AssetVariant == AssetVariant.Machine)
                .FirstOrDefault();
            Assert.That(machine, Is.Not.Null);

            var enumMultiplierType = new EnumMultiplierType();
            enumMultiplierType.Create(MultiplierType.V1);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Gamble, enumMultiplierType);

            var trackerVariant = tracker.Variant.PlayerVariantSharp?.TrackerVariant;
            Assert.That(trackerVariant, Is.Not.Null);
            Assert.That(trackerVariant.SlotAResult, Is.EqualTo(0));
            Assert.That(trackerVariant.SlotBResult, Is.EqualTo(0));
            Assert.That(trackerVariant.SlotCResult, Is.EqualTo(0));
            Assert.That(trackerVariant.SlotDResult, Is.EqualTo(0));
            Assert.That(trackerVariant.LastReward, Is.EqualTo(0));

            var subscriptionId = await _client.StateTransitionAsync(user, enumCasinoAction, [human.Id, tracker.Id, seat.Id, machine.Id], 1, CancellationToken.None);
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

            tracker = user1Assets.Where(p =>
               p.Variant.AssetVariant == AssetVariant.Player
            && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Tracker)
            .FirstOrDefault();
            Assert.That(tracker, Is.Not.Null);

            trackerVariant = tracker.Variant.PlayerVariantSharp?.TrackerVariant;
            Assert.That(trackerVariant, Is.Not.Null);
            Assert.That(trackerVariant.SlotAResult, Is.Not.EqualTo(0));
            Assert.That(trackerVariant.SlotBResult, Is.Not.EqualTo(0));
            Assert.That(trackerVariant.SlotCResult, Is.Not.EqualTo(0));
            Assert.That(trackerVariant.SlotDResult, Is.Not.EqualTo(0));
            Assert.That(trackerVariant.LastReward, Is.Not.EqualTo(0));
        }

        private async Task<List<AssetSharp>> GetAccountAssetsAsync(Account user, string blockhash)
        {
            var ownedAssetIds = await _client.GetAssetIdByAsync(user.ToAccountId32(), 1, blockhash, CancellationToken.None);
            Assert.That(ownedAssetIds, Is.Not.Null);
            var ownedAssets = await _client.GetAssetByKeysAsync(ownedAssetIds, blockhash, CancellationToken.None);
            Assert.That(ownedAssets, Is.Not.Null);

            return ownedAssets.Select(a => a.Value.Item2).ToList();
        }
    }
}