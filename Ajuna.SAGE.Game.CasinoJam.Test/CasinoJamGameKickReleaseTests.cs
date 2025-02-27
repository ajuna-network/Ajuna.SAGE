using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Game.CasinoJam.Model;
using System.Linq;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamGameKickReleaseTests : CasinoJamBaseTest
    {
        private readonly CasinoJamIdentifier CREATE_PLAYER = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);
        private readonly CasinoJamIdentifier FUND_PLAYER_T1000 = CasinoJamIdentifier.Deposit(AssetType.Player, TokenType.T_1000);
        private readonly CasinoJamIdentifier CREATE_MACHINE = CasinoJamIdentifier.Create(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
        private readonly CasinoJamIdentifier FUND_MACHINE_T1000 = CasinoJamIdentifier.Deposit(AssetType.Machine, TokenType.T_1000);
        private readonly CasinoJamIdentifier RENT_SEAT = CasinoJamIdentifier.Rent(AssetType.Seat, AssetSubType.None, RentDuration.Day1);
        private readonly CasinoJamIdentifier RESERVE_SEAT = CasinoJamIdentifier.Reserve(AssetType.Seat, AssetSubType.None, ReservationDuration.Mins5);
        private readonly CasinoJamIdentifier RELEASE = CasinoJamIdentifier.Release();
        private readonly CasinoJamIdentifier KICK = CasinoJamIdentifier.Kick();
        private readonly CasinoJamIdentifier GAMBLE = CasinoJamIdentifier.Gamble(0x00, MultiplierType.V1);

        private IAccount _userA; // The owner of the reserved seat.
        private IAccount _userB; // A second player who will try to kick.

        [SetUp]
        public void Setup()
        {
            // Initialize blockchain info, engine, and player.
            IAsset[] outputAssets;
            HumanAsset humanA;
            HumanAsset humanB;
            BanditAsset banditA;
            SeatAsset seatA;
            bool result;

            BlockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            var userA = Engine.AccountManager.Account(Engine.AccountManager.Create());
            Assert.That(userA, Is.Not.Null);
            _userA = userA;
            _userA.Balance.Deposit(1_000_000);

            var userB = Engine.AccountManager.Account(Engine.AccountManager.Create());
            Assert.That(userB, Is.Not.Null);
            _userB = userB;
            _userB.Balance.Deposit(1_000_000);

            // ---------------------------------- ***
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userA -> CREATE_PLAYER 
            result = Engine.Transition(_userA, CREATE_PLAYER, null, out outputAssets);
            Assert.That(result, Is.True, "Human creation transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userA -> FUND_PLAYER 
            humanA = GetAsset<HumanAsset>(_userA, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            result = Engine.Transition(_userA, FUND_PLAYER_T1000, [humanA], out outputAssets);
            Assert.That(result, Is.True, "Human funding transition should succeed.");
            Assert.That(Engine.AssetBalance(humanA.Id), Is.EqualTo(1000));
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userB -> CREATE_PLAYER 
            result = Engine.Transition(_userB, CREATE_PLAYER, null, out outputAssets);
            Assert.That(result, Is.True, "Human creation transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userB -> FUND_PLAYER 
            humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            result = Engine.Transition(_userB, FUND_PLAYER_T1000, [humanB], out outputAssets);
            Assert.That(result, Is.True, "Human funding transition should succeed.");
            Assert.That(Engine.AssetBalance(humanB.Id), Is.EqualTo(1000));
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userA -> CREATE_MACHINE 
            result = Engine.Transition(_userA, CREATE_MACHINE, null, out outputAssets);
            Assert.That(result, Is.True, "Machine creation transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userA -> RENT_SEAT 
            banditA = GetAsset<BanditAsset>(_userA, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            result = Engine.Transition(_userA, RENT_SEAT, [banditA], out outputAssets);
            Assert.That(result, Is.True, "Rent transition should succeed.");
            seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(seatA.MachineId, Is.EqualTo(banditA.Id));
            Assert.That(seatA.RentDuration, Is.EqualTo(RentDuration.Day1));
            BlockchainInfoProvider.CurrentBlockNumber++;

            // _userB -> RESERVE_SEAT
            humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            result = Engine.Transition(_userB, RESERVE_SEAT, [humanB, seatA], out outputAssets);
            Assert.That(result, Is.True, "Reserve transition should succeed.");
            BlockchainInfoProvider.CurrentBlockNumber++;

            seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));

            // At this point, user B’s human asset should have its SeatId set,
            // and the seat asset from user A should have its PlayerId set.
            humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(humanB.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));
        }

        [Test, Order(1)]
        public void Test_ReleaseTransition_Success()
        {
            var humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(humanB.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));

            // Record initial asset balances.
            uint? humanBalanceBefore = Engine.AssetBalance(humanB.Id);
            uint? seatBalanceBefore = Engine.AssetBalance(seatA.Id);

            bool result = Engine.Transition(_userB, RELEASE, [humanB, seatA], out IAsset[] outputAssets);
            Assert.That(result, Is.True, "Release transition should succeed.");

            // The release function should “clear” the seat reservation.
            humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(humanB.SeatId, Is.EqualTo(0), "Human asset should have SeatId reset.");
            Assert.That(seatA.PlayerId, Is.EqualTo(0), "Seat asset should have PlayerId reset.");

            // The reservation fee (stored in the seat) should be refunded to the human asset.
            uint? humanBalanceAfter = Engine.AssetBalance(humanB.Id);
            uint? seatBalanceAfter = Engine.AssetBalance(seatA.Id);
            uint fee = seatBalanceBefore ?? 0;
            Assert.That(humanBalanceAfter, Is.EqualTo((humanBalanceBefore ?? 0) + fee), "Human balance should be increased by the fee.");
            Assert.That(seatBalanceAfter, Is.EqualTo(0), "Seat balance should be zero after release.");
        }

        [Test, Order(2)]
        public void Test_ReleaseTransition_NonOwner_Failure()
        {
            var humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            Assert.That(humanB.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));

            // Here we use player B’s human asset instead of player A’s.
            var wrongHuman = GetAsset<HumanAsset>(_userA, AssetType.Player, (AssetSubType)PlayerSubType.Human);

            var releaseId = CasinoJamIdentifier.Release();
            IAsset[] releaseInput = [wrongHuman, seatA];
            bool result = Engine.Transition(_userB, releaseId, releaseInput, out IAsset[] outputAssets);
            // The engine’s rules (via the IsOwnerOf rule) should cause the transition to fail.
            Assert.That(result, Is.False, "Release transition should fail when called by a non-owner.");
        }

        [Test, Order(3)]
        public void Test_KickTransition_Success()
        {
            // In this test the seat is reserved by userB’s human asset (the victim).
            // We simulate that the reservation has expired by setting the blockchain block number high.
            // Then userA’s human asset (the kicker) calls the Kick transition to remove the victim.
            //
            // After a successful kick:
            //   • the victim’s SeatId is reset to 0,
            //   • the seat’s PlayerId is reset to 0,
            //   • and the reservation fee is transferred to the kicker.

            // Get the victim (userB’s human asset) and the seat.
            var humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            // Pre–check: the seat is currently reserved by the victim.
            Assert.That(humanB.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));

            // Get the kicker from userA.
            var humanA = GetAsset<HumanAsset>(_userA, AssetType.Player, (AssetSubType)PlayerSubType.Human);

            // Record balances before the kick.
            var preSeatABalance = Engine.AssetBalance(seatA.Id);
            var preHumanABalance = Engine.AssetBalance(humanA.Id);
            var preHumanBBalance = Engine.AssetBalance(humanB.Id);

            // Set the blockchain block number high so that the reservation is expired (i.e. not within the grace period).
            // (For example, with ReservationStartBlock = 1 and ReservationDuration = 30, a block number of 700 is well past the validity.)
            BlockchainInfoProvider.CurrentBlockNumber = 700;
            Assert.That(BlockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(700));

            // The Kick transition requires three assets:
            // 1. The kicker’s human asset (which must be owned by the caller),
            // 2. The victim’s human asset,
            // 3. The reserved seat.

            // The transition is called by the kicker (userA).
            bool result = Engine.Transition(_userA, KICK, [humanA, humanB, seatA], out IAsset[] outputAssets);
            Assert.That(result, Is.True, "Kick transition should succeed when reservation is expired.");

            // Extract the updated victim and seat.
            var updatedVictim = new HumanAsset(outputAssets[1]);
            var updatedSeat = new SeatAsset(outputAssets[2]);
            // Verify that the victim’s reservation is removed.
            Assert.That(updatedVictim.SeatId, Is.EqualTo(0), "Victim's human asset should have SeatId reset after kick.");
            Assert.That(updatedSeat.PlayerId, Is.EqualTo(0), "Seat asset should have PlayerId reset after kick.");

            // The reservation fee originally stored on the seat should now have been transferred to the kicker.
            var seatBalanceAfter = Engine.AssetBalance(seatA.Id);
            var updatedHumanABalance = Engine.AssetBalance(humanA.Id);
            Assert.That(updatedHumanABalance, Is.EqualTo(preHumanABalance + preSeatABalance), "Kicker should receive the reservation fee.");
            Assert.That(seatBalanceAfter, Is.EqualTo(0), "Seat balance should be zero after a kick.");
        }

        [Test, Order(4)]
        public void Test_KickTransition_NoKickDuringGrace()
        {
            // In this test we simulate that the reserved seat is still within its grace period.
            // That is, even though the kick transition is attempted,
            // the reservation is still valid and the kick function returns the assets unchanged.
            //
            // In this scenario:
            //   • the victim’s human asset remains linked to the seat,
            //   • the seat’s PlayerId remains unchanged,
            //   • and no fee is transferred to the kicker.

            // Get the victim (userB’s human asset), the seat, and the kicker (userA’s human asset).
            var humanB = GetAsset<HumanAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var trackerB = GetAsset<TrackerAsset>(_userB, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var seatA = GetAsset<SeatAsset>(_userA, AssetType.Seat, (AssetSubType)SeatSubType.None);
            var banditA = GetAsset<BanditAsset>(_userA, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var humanA = GetAsset<HumanAsset>(_userA, AssetType.Player, (AssetSubType)PlayerSubType.Human);

            // Pre–check: the seat is reserved by the victim.
            Assert.That(humanB.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanB.Id));

            // Record balances before the attempted kick.
            uint? preSeatABalance = Engine.AssetBalance(seatA.Id);
            uint? preHumanABalance = Engine.AssetBalance(humanA.Id);

            // Gamble once ... (to update the seat’s LastActionBlock)
            bool result = Engine.Transition(_userB, GAMBLE, [humanB, trackerB, seatA, banditA], out IAsset[] outputAssets);
            Assert.That(result, Is.True, "Kick transition should return successfully even when no kick occurs (due to grace period).");

            // Set the current block to 610 (within the grace period).
            BlockchainInfoProvider.CurrentBlockNumber = 20;
            Assert.That(BlockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(20));

            // Call the Kick transition as the kicker (userA).
            // Even though the transition rules are met, the function should detect the active grace period and return the assets unchanged.
            result = Engine.Transition(_userA, KICK, [humanA, humanB, seatA], out outputAssets);
            Assert.That(result, Is.True, "Kick transition should return successfully even when no kick occurs (due to grace period).");

            // Verify that the victim is still seated.
            var updatedVictim = new HumanAsset(outputAssets[1]);
            var updatedSeat = new SeatAsset(outputAssets[2]);
            Assert.That(updatedVictim.SeatId, Is.EqualTo(seatA.Id), "Victim's human asset should remain linked to the seat.");
            Assert.That(updatedSeat.PlayerId, Is.EqualTo(humanB.Id), "Seat asset should remain occupied by the victim.");

            // Also, verify that no fee transfer took place.
            uint? seatBalanceAfter = Engine.AssetBalance(seatA.Id);
            uint? kickerBalanceAfter = Engine.AssetBalance(humanA.Id);
            Assert.That(seatBalanceAfter, Is.EqualTo(preSeatABalance), "Seat balance should remain unchanged when kick is prevented by grace period.");
            Assert.That(kickerBalanceAfter, Is.EqualTo(preHumanABalance), "Kicker balance should remain unchanged when kick is prevented by grace period.");
        }
    }
}