using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Model;
using Ajuna.SAGE.Game.Model;
using NUnit.Framework;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamGameGambleTests : CasinoJamBaseTest
    {
        private readonly CasinoJamIdentifier CREATE_PLAYER = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);
        private readonly CasinoJamIdentifier FUND_PLAYER_T1000 = CasinoJamIdentifier.Deposit(AssetType.Player, TokenType.T_1000);
        private readonly CasinoJamIdentifier CREATE_MACHINE = CasinoJamIdentifier.Create(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
        private readonly CasinoJamIdentifier FUND_MACHINE_T100000 = CasinoJamIdentifier.Deposit(AssetType.Machine, TokenType.T_100000);
        private readonly CasinoJamIdentifier RENT_SEAT = CasinoJamIdentifier.Rent(AssetType.Seat, AssetSubType.None, RentDuration.Day1);
        private readonly CasinoJamIdentifier RESERVE_SEAT = CasinoJamIdentifier.Reserve(AssetType.Seat, AssetSubType.None, ReservationDuration.Mins5);
        private readonly CasinoJamIdentifier GAMBLE = CasinoJamIdentifier.Gamble(0x00, MultiplierType.V1);

        private IAccount _user;

        [SetUp]
        public void Setup()
        {
            IAsset[] outputAssets;

            // Create and fund a single user.
            _user = Engine.AccountManager.Account(Engine.AccountManager.Create());
            Assert.That(_user, Is.Not.Null);
            _user.Balance.Deposit(1_000_000);

            // Increment block number before each transition.
            BlockchainInfoProvider.CurrentBlockNumber++;

            // CREATE_PLAYER: creates both a human and tracker asset.
            bool result = Engine.Transition(_user, CREATE_PLAYER, null, out outputAssets);
            Assert.That(result, Is.True, "Create player transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // FUND_PLAYER: deposit funds to the human asset.
            var human = GetAsset<HumanAsset>(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            result = Engine.Transition(_user, FUND_PLAYER_T1000, [human], out outputAssets);
            Assert.That(result, Is.True, "Fund player transition should succeed.");
            Assert.That(Engine.AssetBalance(human.Id), Is.EqualTo(1000));
            BlockchainInfoProvider.CurrentBlockNumber++;

            // CREATE_MACHINE: create a bandit machine.
            result = Engine.Transition(_user, CREATE_MACHINE, null, out outputAssets);
            Assert.That(result, Is.True, "Create machine transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // FUND_MACHINE: deposit funds to the human asset.
            var bandit = GetAsset<BanditAsset>(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            result = Engine.Transition(_user, FUND_MACHINE_T100000, [bandit], out outputAssets);
            Assert.That(result, Is.True, "Fund machine transition should succeed.");
            Assert.That(Engine.AssetBalance(bandit.Id), Is.EqualTo(100000));
            BlockchainInfoProvider.CurrentBlockNumber++;

            // RENT_SEAT: rent a seat from the bandit.
            bandit = GetAsset<BanditAsset>(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            result = Engine.Transition(_user, RENT_SEAT, [bandit], out outputAssets);
            Assert.That(result, Is.True, "Rent seat transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // RESERVE_SEAT: reserve the seat using the human asset.
            var seat = GetAsset<SeatAsset>(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);
            human = GetAsset<HumanAsset>(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            result = Engine.Transition(_user, RESERVE_SEAT, [human, seat], out outputAssets);
            Assert.That(result, Is.True, "Reserve seat transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;
            // At this point, the seat should have its ReservationStartBlock set.
        }

        [Test]
        public void Test_GambleActionLimit()
        {
            Assert.That(BlockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(8));

            // Retrieve the four required assets for a gamble transition:
            // 1. The human (player) asset,
            // 2. The tracker asset,
            // 3. The reserved seat asset,
            // 4. The bandit (machine) asset.
            var human = GetAsset<HumanAsset>(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset<TrackerAsset>(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var seat = GetAsset<SeatAsset>(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);
            var bandit = GetAsset<BanditAsset>(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            Assert.That(seat.LastActionBlockOffset, Is.EqualTo(0), "LastActionBlock should be 0 before the first gamble.");

            // FIRST GAMBLE TRANSITION: execute gamble normally.
            bool resultFirst = Engine.Transition(_user, GAMBLE, [human, tracker, seat, bandit], out IAsset[] gambleAssetsFirst);
            Assert.That(resultFirst, Is.True, "First gamble transition should succeed.");

            // Capture key state after the first gamble.
            var updatedSeatFirst = gambleAssetsFirst[2] as SeatAsset;
            ushort expectedLastAction = (ushort)(BlockchainInfoProvider.CurrentBlockNumber - updatedSeatFirst.ReservationStartBlock);
            Assert.That(updatedSeatFirst.LastActionBlockOffset, Is.EqualTo(expectedLastAction), "Seat's LastActionBlock should be updated after the first gamble.");

            var updatedTrackerFirst = gambleAssetsFirst[1] as TrackerAsset;
            var trackerSlot0First = updatedTrackerFirst.GetSlot(0);

            // SECOND GAMBLE TRANSITION: attempt to gamble again in the same block.
            bool resultSecond = Engine.Transition(_user, GAMBLE, [human, tracker, seat, bandit], out IAsset[] gambleAssetsSecond);
            Assert.That(resultSecond, Is.False, "Second gamble transition should not success, as it has the cooldown rule.");

        }
    }
}
