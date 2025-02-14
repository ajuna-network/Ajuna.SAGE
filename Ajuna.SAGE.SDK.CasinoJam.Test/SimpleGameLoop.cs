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
            var user_1_assets = await GetAccountAssetsAsync(_user1, null);
            var mach_1 = user_1_assets.Where(p => p.Variant.AssetVariant == AssetVariant.Machine).FirstOrDefault();
            Assert.That(mach_1, Is.Not.Null);

            var enumCasinoAction = new EnumCasinoAction();
            var enumMachineType = new EnumMachineType();
            enumMachineType.Create(MachineType.Bandit);
            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Machine, enumMachineType);
            var enumTokenType = new EnumTokenType();
            enumTokenType.Create(TokenType.T100000);
            var tuple = new BaseTuple<EnumAssetType, EnumTokenType>(enumAssetType, enumTokenType);
            enumCasinoAction.Create(CasinoAction.Deposit, tuple);

            var subscriptionId = await _client.StateTransitionAsync(_user1, enumCasinoAction, [mach_1.Id], 1, CancellationToken.None);
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
            var user_2 = _user2;

            var user_1_assets = await GetAccountAssetsAsync(user_2, null);
            var human_1 = user_1_assets.Where(p => 
                   p.Variant.AssetVariant == AssetVariant.Player 
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                .FirstOrDefault();
            Assert.That(human_1, Is.Not.Null);

            var enumAssetType = new EnumAssetType();
            enumAssetType.Create(AssetType.Player, new BaseVoid());

            var enumTokenType = new EnumTokenType();
            enumTokenType.Create(TokenType.T1000);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Deposit, 
                new BaseTuple<EnumAssetType, EnumTokenType>(enumAssetType, enumTokenType));

            var subscriptionId = await _client.StateTransitionAsync(user_2, enumCasinoAction, [human_1.Id], 1, CancellationToken.None);
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
            var user_2 = _user2; // TODO: change to an other player!!

            var user_2_assets = await GetAccountAssetsAsync(user_2, null);
            var human_2 = user_2_assets.Where(p =>
                   p.Variant.AssetVariant == AssetVariant.Player
                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                .FirstOrDefault();
            Assert.That(human_2, Is.Not.Null);

            // take seat from user 2
            var user_1_assets = await GetAccountAssetsAsync(_user1, null);
            var seat_2 = GetSeat(user_1_assets);
            Assert.That(seat_2, Is.Not.Null);

            var enumMultiplierType = new EnumMultiplierType();
            enumMultiplierType.Create(MultiplierType.V1);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Reserve, enumMultiplierType);

            var subscriptionId = await _client.StateTransitionAsync(user_2, enumCasinoAction, [human_2.Id, seat_2.Id], 1, CancellationToken.None);
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

        [Test, Order(7)]
        public async Task Gamble_User2_TestAsync()
        {
            // make sure to use the right user.
            var user_2 = _user2;

            var user_2_assets = await GetAccountAssetsAsync(user_2, null);

            var huma_2 = GetPlayerHuman(user_2_assets);
            Assert.That(huma_2, Is.Not.Null);

            var track_2 = GetPlayerTracker(user_2_assets);
            Assert.That(track_2, Is.Not.Null);

            // take seat from user 2
            var user_1_assets = await GetAccountAssetsAsync(_user1, null);

            var seat_1 = GetSeat(user_1_assets);
            Assert.That(seat_1, Is.Not.Null);

            // take machine from user 2
            var mach_1 = GetMachine(user_1_assets);
            Assert.That(mach_1, Is.Not.Null);

            var enumMultiplierType = new EnumMultiplierType();
            enumMultiplierType.Create(MultiplierType.V4);

            var enumCasinoAction = new EnumCasinoAction();
            enumCasinoAction.Create(CasinoAction.Gamble, enumMultiplierType);

            var track_2_variant = track_2.Variant.PlayerVariantSharp?.TrackerVariant;
            Assert.That(track_2_variant, Is.Not.Null);
            Assert.That(track_2_variant.SlotAResult, Is.EqualTo(0));
            Assert.That(track_2_variant.SlotBResult, Is.EqualTo(0));
            Assert.That(track_2_variant.SlotCResult, Is.EqualTo(0));
            Assert.That(track_2_variant.SlotDResult, Is.EqualTo(0));
            Assert.That(track_2_variant.LastReward, Is.EqualTo(0));

            var subscriptionId = await _client.StateTransitionAsync(user_2, enumCasinoAction, [huma_2.Id, track_2.Id, seat_1.Id, mach_1.Id], 1, CancellationToken.None);
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

            track_2 = user_2_assets.Where(p =>
               p.Variant.AssetVariant == AssetVariant.Player
            && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Tracker)
            .FirstOrDefault();
            Assert.That(track_2, Is.Not.Null);

            track_2_variant = track_2.Variant.PlayerVariantSharp?.TrackerVariant;
            Assert.That(track_2_variant, Is.Not.Null);
            Assert.That(track_2_variant.SlotAResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotBResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotCResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotDResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.LastReward, Is.Not.EqualTo(0));
        }

        [Test, Order(8)]
        public async Task Gamble_User2_Check_TestAsync()
        {
            // make sure to use the right user.
            var user_2 = _user2;

            var user_2_assets = await GetAccountAssetsAsync(user_2, null);

            var huma_2 = GetPlayerHuman(user_2_assets);
            Assert.That(huma_2, Is.Not.Null);

            var track_2 = GetPlayerTracker(user_2_assets);
            Assert.That(track_2, Is.Not.Null);

            track_2 = user_2_assets.Where(p =>
               p.Variant.AssetVariant == AssetVariant.Player
            && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Tracker)
            .FirstOrDefault();
            Assert.That(track_2, Is.Not.Null);

            var track_2_variant = track_2.Variant.PlayerVariantSharp?.TrackerVariant;
            Assert.That(track_2_variant, Is.Not.Null);
            Assert.That(track_2_variant.SlotAResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotBResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotCResult, Is.Not.EqualTo(0));
            Assert.That(track_2_variant.SlotDResult, Is.Not.EqualTo(0));
            //Assert.That(track_2_variant.LastReward, Is.Not.EqualTo(0));
        }

        private AssetSharp? GetPlayerHuman(List<AssetSharp> userAssets) => 
            userAssets.Where(p => p.Variant.AssetVariant == AssetVariant.Player
                                && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Human)
                                .FirstOrDefault();

        private AssetSharp? GetPlayerTracker(List<AssetSharp> userAssets) =>
            userAssets.Where(p => p.Variant.AssetVariant == AssetVariant.Player
                        && p.Variant.PlayerVariantSharp?.PlayerVariant == PlayerVariant.Tracker)
                        .FirstOrDefault();
        private AssetSharp? GetSeat(List<AssetSharp> userAssets) =>
            userAssets.Where(p => p.Variant.AssetVariant == AssetVariant.Seat)
                        .FirstOrDefault();

        private AssetSharp? GetMachine(List<AssetSharp> userAssets) =>
            userAssets.Where(p => p.Variant.AssetVariant == AssetVariant.Machine)
                        .FirstOrDefault();

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