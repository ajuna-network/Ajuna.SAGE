using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Game.CasinoJam.Model;
using System.Linq;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamGameKickReleaseTests
    {
        private readonly CasinoJamIdentifier CREATE_PLAYER = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);
        private readonly CasinoJamIdentifier CREATE_MACHINE = CasinoJamIdentifier.Create(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
        private readonly CasinoJamIdentifier RENT_SEAT = CasinoJamIdentifier.Rent(AssetType.Seat, AssetSubType.None, MultiplierType.V1);
        private readonly CasinoJamIdentifier RESERVE_SEAT = CasinoJamIdentifier.Reserve(AssetType.Seat, AssetSubType.None, MultiplierType.V1);

        private IBlockchainInfoProvider _blockchainInfoProvider;
        private Engine<CasinoJamIdentifier, CasinoJamRule> _engine;
        private Account _userA; // The owner of the reserved seat.
        private Account _userB; // A second player who will try to kick.

        [SetUp]
        public void Setup()
        {
            // Initialize blockchain info, engine, and player.
            _blockchainInfoProvider = new BlockchainInfoProvider(1234);
            _engine = CasinoJameGame.Create(_blockchainInfoProvider);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // Create two players with ample balance.
            _userA = new Account(Utils.GenerateRandomId(), 1_000_000);
            _userB = new Account(Utils.GenerateRandomId(), 1_000_000);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // Both players need to have their player assets (human and tracker) created.
            IAsset[]? inputAssets = null;

            // Create playerA's assets.
            bool result = _engine.Transition(_userA, CREATE_PLAYER, inputAssets = null, out IAsset[] outputAssetsA);
            Assert.That(result, Is.True, "Player A creation transition should succeed.");
            _userA.Transition(inputAssets, outputAssetsA);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // Create playerB's assets.
            result = _engine.Transition(_userB, CREATE_PLAYER, inputAssets = null, out IAsset[] outputAssetsB);
            Assert.That(result, Is.True, "Player B creation transition should succeed.");
            _userB.Transition(inputAssets, outputAssetsB);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // For player A, we now create a machine asset and then rent a seat.
            // (This is similar to what is done in Test_RentTransition.)

            result = _engine.Transition(_userA, CREATE_MACHINE, inputAssets = null, out IAsset[] outputMachine);
            Assert.That(result, Is.True, "Machine creation transition should succeed.");
            _userA.Transition(inputAssets, outputMachine);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // Assume the newly created machine is the last asset in playerA's asset list.
            var machines = _userA.Query([CasinoJamUtil.MatchType(AssetType.Machine,(AssetSubType)MachineSubType.Bandit)]);
            Assert.That(machines, Is.Not.Null, "Machines returns not null.");
            var machine = machines.FirstOrDefault();
            Assert.That(machine, Is.Not.Null, "Machine asset should be found.");

            // Rent a seat from the machine using multiplier V1.
            result = _engine.Transition(_userA, RENT_SEAT, inputAssets = [machine], out IAsset[] outputRent);
            Assert.That(result, Is.True, "Rent transition should succeed.");
            _userA.Transition(inputAssets, outputRent);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // After renting, assume that the new seat is the last asset.
            // Now reserve the seat with player A’s human asset.

            // Assume the newly created machine is the last asset in playerA's asset list.
            var humans = _userB.Query([CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Human)]);
            Assert.That(humans, Is.Not.Null, "Humans returns not null.");
            var human = humans.FirstOrDefault();
            Assert.That(human, Is.Not.Null, "Human asset should be found.");

            var seats = _userA.Query([CasinoJamUtil.MatchType(AssetType.Seat, (AssetSubType)SeatSubType.None)]);
            Assert.That(seats, Is.Not.Null, "Seats returns not null.");
            var seat = seats.FirstOrDefault();
            Assert.That(seat, Is.Not.Null, "Seat asset should be found.");

            // Player A’s human asset is at index 0; seat asset is at index 3.
            IAsset[] reserveInput = [human, seat];
            result = _engine.Transition(_userB, RESERVE_SEAT, reserveInput, out IAsset[] outputReserve);
            Assert.That(result, Is.True, "Reserve transition should succeed.");
            _userB.Transition(reserveInput, outputReserve);

            _blockchainInfoProvider.CurrentBlockNumber++; // ---------------------------------- ***

            // At this point, player A’s human asset should have its SeatId set,
            // and the seat asset should have its PlayerId set.
            var humanA = new HumanAsset(_userA.Assets.ElementAt(0));
            var seatA = new SeatAsset(_userA.Assets.ElementAt(3));
            Assert.That(humanA.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanA.Id));
        }

        [Test, Order(1)]
        public void Test_ReleaseTransition_Success()
        {
            // Verify that the owner (player A) can release his reserved seat.
            var humanA = new HumanAsset(_userA.Assets.ElementAt(0));
            var seatA = new SeatAsset(_userA.Assets.ElementAt(3));

            // Record initial asset balances.
            uint? humanBalanceBefore = _engine.AssetBalance(humanA.Id);
            uint? seatBalanceBefore = _engine.AssetBalance(seatA.Id);

            // Call the release transition.
            var releaseId = CasinoJamIdentifier.Release();
            IAsset[] releaseInput = [humanA, seatA];
            bool result = _engine.Transition(_userA, releaseId, releaseInput, out IAsset[] outputAssets);
            Assert.That(result, Is.True, "Release transition should succeed.");
            _userA.Transition(releaseInput, outputAssets);

            // The release function should “clear” the seat reservation.
            var updatedHuman = new HumanAsset(outputAssets[0]);
            var updatedSeat = new SeatAsset(outputAssets[1]);
            Assert.That(updatedHuman.SeatId, Is.EqualTo(0), "Human asset should have SeatId reset.");
            Assert.That(updatedSeat.PlayerId, Is.EqualTo(0), "Seat asset should have PlayerId reset.");

            // The reservation fee (stored in the seat) should be refunded to the human asset.
            uint? humanBalanceAfter = _engine.AssetBalance(humanA.Id);
            uint? seatBalanceAfter = _engine.AssetBalance(seatA.Id);
            uint fee = seatBalanceBefore ?? 0;
            Assert.That(humanBalanceAfter, Is.EqualTo((humanBalanceBefore ?? 0) + fee), "Human balance should be increased by the fee.");
            Assert.That(seatBalanceAfter, Is.EqualTo(0), "Seat balance should be zero after release.");
        }

        [Test, Order(2)]
        public void Test_ReleaseTransition_NonOwner_Failure()
        {
            // Verify that a player who is not the owner of the seat (player B)
            // cannot perform a release transition on player A’s reserved seat.
            var seatA = new SeatAsset(_userA.Assets.ElementAt(3));

            // Here we use player B’s human asset instead of player A’s.
            var wrongHuman = new HumanAsset(_userB.Assets.ElementAt(0));

            var releaseId = CasinoJamIdentifier.Release();
            IAsset[] releaseInput = [wrongHuman, seatA];
            bool result = _engine.Transition(_userB, releaseId, releaseInput, out IAsset[] outputAssets);
            // The engine’s rules (via the IsOwnerOf rule) should cause the transition to fail.
            Assert.That(result, Is.False, "Release transition should fail when called by a non-owner.");
        }

        [Test, Order(3)]
        public void Test_KickTransition_Success()
        {
            // Verify that player B (the sniper) can kick player A off his reserved seat
            // when the reservation is expired (or not in grace period).
            var sniper = new HumanAsset(_userB.Assets.ElementAt(0));
            var humanA = new HumanAsset(_userA.Assets.ElementAt(0));
            var seatA = new SeatAsset(_userA.Assets.ElementAt(3));

            // Pre-check: ensure that seat is still reserved by player A.
            Assert.That(humanA.SeatId, Is.EqualTo(seatA.Id));
            Assert.That(seatA.PlayerId, Is.EqualTo(humanA.Id));

            // Set the blockchain current block to a high value so that the kick conditions hold.
            // (With a fresh reservation at block 1, the Reserve transition set the following:
            //  ReservationStartBlock = 1, ReservationDuration = 600, PlayerGracePeriod = 30, LastActionBlock = 0.
            //  In Kick, isReservationValid becomes (1+600 <= b), so for b>=601 it is true.
            //  And isGracePeriod becomes (1+0+30 > b), which for b>=601 is false.
            //  Therefore, the check (isReservationValid && isGracePeriod) is false and the kick proceeds.)
            _blockchainInfoProvider.CurrentBlockNumber = 700;
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(700));

            // Record balances before the kick.
            uint? seatBalanceBefore = _engine.AssetBalance(seatA.Id);
            uint sniperBalanceBefore = _engine.AssetBalance(sniper.Id) ?? 0;

            // Kick transition expects three assets:
            // 1. The kicker’s (sniper’s) human asset,
            // 2. The victim’s human asset (player A’s),
            // 3. The reserved seat.
            var kickId = CasinoJamIdentifier.Kick();
            IAsset[] kickInput = [sniper, humanA, seatA];
            bool result = _engine.Transition(_userB, kickId, kickInput, out IAsset[] outputAssets);
            Assert.That(result, Is.True, "Kick transition should succeed when conditions allow.");
            // Apply the transition. (Note that the transition function returns an updated set of assets;
            // although player A’s assets are modified, the kick is initiated by player B.)
            _userB.Transition(kickInput, outputAssets);

            // Extract the updated assets.
            var updatedHumanA = new HumanAsset(outputAssets[1]);
            var updatedSeatA = new SeatAsset(outputAssets[2]);
            // After a successful kick, the victim’s human asset should have its SeatId cleared,
            // and the seat should no longer be occupied.
            Assert.That(updatedHumanA.SeatId, Is.EqualTo(0), "Victim's human asset should have SeatId reset after kick.");
            Assert.That(updatedSeatA.PlayerId, Is.EqualTo(0), "Seat asset should have PlayerId reset after kick.");

            // The reservation fee should have been transferred to the sniper’s asset.
            uint? seatBalanceAfter = _engine.AssetBalance(seatA.Id);
            uint sniperBalanceAfter = _engine.AssetBalance(sniper.Id) ?? 0;
            uint fee = seatBalanceBefore ?? 0;
            Assert.That(sniperBalanceAfter, Is.EqualTo(sniperBalanceBefore + fee), "Sniper should receive the reservation fee.");
            Assert.That(seatBalanceAfter, Is.EqualTo(0), "Seat balance should be zero after a kick.");
        }

        [Test, Order(4)]
        public void Test_KickTransition_NoKickDuringGrace()
        {
            // Verify that if the seat is still within its grace period,
            // the kick transition does not remove the reservation.
            var sniper = new HumanAsset(_userB.Assets.ElementAt(0));
            var humanA = new HumanAsset(_userA.Assets.ElementAt(0));
            var seatA = new SeatAsset(_userA.Assets.ElementAt(3));

            // To simulate recent activity, update the seat’s LastActionBlock.
            // For example, set it high so that the grace period condition holds.
            // With ReservationStartBlock = 1, if we set LastActionBlock to 600 and PlayerGracePeriod = 30,
            // then (1 + 600 + 30 = 631). If we set the current block to 610 then:
            //   isReservationValid: (1+600 <= 610) → (601 <= 610) is true,
            //   isGracePeriod: (1+600+30 > 610) → (631 > 610) is true.
            // In this case, the kick function returns early (i.e. does nothing).
            seatA.LastActionBlock = 600;

            _blockchainInfoProvider.CurrentBlockNumber = 610;
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(610));

            // Record balances before the attempted kick.
            uint? seatBalanceBefore = _engine.AssetBalance(seatA.Id);
            uint sniperBalanceBefore = _engine.AssetBalance(sniper.Id) ?? 0;

            var kickId = CasinoJamIdentifier.Kick();
            IAsset[] kickInput = [sniper, humanA, seatA];
            bool result = _engine.Transition(_userB, kickId, kickInput, out IAsset[] outputAssets);
            // Even if the rules are met the function returns the assets unchanged.
            Assert.That(result, Is.True, "Kick transition should return successfully but without changes when in grace period.");
            _userB.Transition(kickInput, outputAssets);

            // The output assets should show that the victim is still seated.
            var updatedHumanA = new HumanAsset(outputAssets[1]);
            var updatedSeatA = new SeatAsset(outputAssets[2]);
            Assert.That(updatedHumanA.SeatId, Is.EqualTo(seatA.Id), "Victim's human asset should still be linked to the seat.");
            Assert.That(updatedSeatA.PlayerId, Is.EqualTo(humanA.Id), "Seat asset should still be occupied by the victim.");

            // Balances remain unchanged.
            uint? seatBalanceAfter = _engine.AssetBalance(seatA.Id);
            uint sniperBalanceAfter = _engine.AssetBalance(sniper.Id) ?? 0;
            Assert.That(seatBalanceAfter, Is.EqualTo(seatBalanceBefore), "Seat balance should remain unchanged.");
            Assert.That(sniperBalanceAfter, Is.EqualTo(sniperBalanceBefore), "Sniper balance should remain unchanged.");
        }
    }
}