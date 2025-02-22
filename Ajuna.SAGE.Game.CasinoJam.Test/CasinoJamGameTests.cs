using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Model;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamGameTests : CasinoJamBaseTest
    {
        private IAccount _user;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {

            var userId = Engine.AccountManager.Create();
            var user = Engine.AccountManager.Account(userId);
            Assert.That(user, Is.Not.Null);
            _user = user;
            _user.Balance.Deposit(1_002_000);
        }

        [Test, Order(0)]
        public void Test_CurrentBlockNumber()
        {
            Assert.That(BlockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(1));
        }

        [Test, Order(1)]
        public void Test_CreatePlayerTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(0));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var transitionResult = Engine.Transition(_user, identifier, null, out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            var asset = Engine.AssetManager.Read(outputAssets[0].Id);
            Assert.That(asset, Is.Not.Null);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());

            // Cast to PlayerAsset and check the properties
            var human = outputAssets[0] as HumanAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(human, Is.Not.Null);
            Assert.That(human.AssetType, Is.EqualTo(AssetType.Player));
            Assert.That(human.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Human));
            Assert.That(Engine.AssetBalance(human.Id), Is.Null);

            var tracker = outputAssets[1] as TrackerAsset;

            // Check that the tracker asset is correctly initialized
            Assert.That(tracker, Is.Not.Null);
            Assert.That(tracker.AssetType, Is.EqualTo(AssetType.Player));
            Assert.That(tracker.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Tracker));
            Assert.That(Engine.AssetBalance(human.Id), Is.Null);

            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(2));
        }

        [Test, Order(2)]
        public void Test_CreateMachineTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(2));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            var identifier = CasinoJamIdentifier.Create(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var transitionResult = Engine.Transition(_user, identifier, null, out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<MachineAsset>());

            // Cast to MachineAsset and check the properties
            MachineAsset asset = outputAssets[0] as MachineAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.AssetType, Is.EqualTo(AssetType.Machine));
            Assert.That(asset.AssetSubType, Is.EqualTo((AssetSubType)MachineSubType.Bandit));
            Assert.That(asset.Value1Factor, Is.EqualTo(TokenType.T_1));
            Assert.That(asset.Value1Multiplier, Is.EqualTo(MultiplierType.V1));

            var assets = Engine.AssetManager.AssetOf(_user).Select(p => p as BaseAsset);
            Assert.That(assets.Count, Is.EqualTo(3));
        }

        [Test, Order(3)]
        public void Test_FundMachineTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            var machine = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Deposit(AssetType.Machine, TokenType.T_1000000);
            var transitionResult = Engine.Transition(_user, identifier, [machine], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to PlayerAsset and check the properties
            var asset = outputAssets[0] as BaseAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.AssetType, Is.EqualTo(AssetType.Machine));
            Assert.That(Engine.AssetBalance(asset.Id), Is.EqualTo(1_000_000));

            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(3));

            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(2_000));
        }

        [Test, Order(4)]
        public void Test_RentTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(2_000));

            var machine = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            Assert.That((machine as MachineAsset)?.SeatLinked, Is.EqualTo(0));

            var identifier = CasinoJamIdentifier.Rent(AssetType.Seat, AssetSubType.None, RentDuration.Day1);
            var transitionResult = Engine.Transition(_user, identifier, [machine], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<MachineAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<SeatAsset>());

            // Cast to MachineAsset and check the properties
            MachineAsset updatedMachine = new MachineAsset(outputAssets[0]);

            Assert.That(updatedMachine, Is.Not.Null);
            Assert.That(updatedMachine.SeatLinked, Is.EqualTo(1));

            // Cast to SeatAsset and check the properties
            SeatAsset seat = new SeatAsset(outputAssets[1]);

            Assert.That(seat, Is.Not.Null);
            Assert.That(seat.MachineId, Is.EqualTo(updatedMachine.Id));

            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            Assert.That(_user.Balance.Value, Is.EqualTo(1_900));
        }

        [Test, Order(5)]
        public void Test_FundPlayerTransition_1()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Deposit(AssetType.Player, TokenType.T_1000);
            var transitionResult = Engine.Transition(_user, identifier, [human], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to PlayerAsset and check the properties
            var asset = new BaseAsset(outputAssets[0]);

            // Check that the hero asset is correctly initialized
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.AssetType, Is.EqualTo(AssetType.Player));
            Assert.That(asset.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Human));
            Assert.That(Engine.AssetBalance(asset.Id), Is.EqualTo(1_000));

            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            Assert.That(_user.Balance.Value, Is.EqualTo(900));
        }

        [Test, Order(6)]
        public void Test_ReserveTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Reserve(AssetType.Seat, AssetSubType.None, ReservationDuration.Mins5);
            var transitionResult = Engine.Transition(_user, identifier, [human, seat], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<SeatAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedHuman = new HumanAsset(outputAssets[0]);

            Assert.That(updatedHuman, Is.Not.Null);
            Assert.That(updatedHuman.SeatId, Is.EqualTo(seat.Id));
            Assert.That(Engine.AssetBalance(updatedHuman.Id), Is.EqualTo(prevHumanBalance - 1));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[1]);

            Assert.That(updatedSeat, Is.Not.Null);
            Assert.That(updatedSeat.PlayerId, Is.EqualTo(human.Id));
            Assert.That(Engine.AssetBalance(updatedSeat.Id), Is.EqualTo(1));

        }

        [Test, Order(9)]
        public void Test_GambleTransition_Once()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            Assert.That((seat as SeatAsset)?.PlayerActionCount, Is.EqualTo(0));

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, MultiplierType.V1);
            var transitionResult = Engine.Transition(_user, identifier, [human, tracker, seat, bandit], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(4));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<SeatAsset>());
            Assert.That(outputAssets[3], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevHumanBalance - 1));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("257-11"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[2]);
            Assert.That(updatedSeat.PlayerActionCount, Is.EqualTo(1));
            Assert.That(updatedSeat.LastActionBlock, Is.EqualTo(0));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[3]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 1));
        }

        [Test, Order(10)]
        public void Test_GambleTransition_Twice()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, CasinoJam.MultiplierType.V2);
            var transitionResult = Engine.Transition(_user, identifier, [human, tracker, seat, bandit], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(4));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<SeatAsset>());
            Assert.That(outputAssets[3], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevHumanBalance - 2));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("763-20"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("054-30"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[2]);
            Assert.That(updatedSeat.PlayerActionCount, Is.EqualTo(2));
            Assert.That(updatedSeat.LastActionBlock, Is.EqualTo(0));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[3]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 2));
        }

        [Test, Order(11)]
        public void Test_GambleTransition_Three()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, MultiplierType.V3);
            var transitionResult = Engine.Transition(_user, identifier, [human, tracker, seat, bandit], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(4));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<SeatAsset>());
            Assert.That(outputAssets[3], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevHumanBalance - 3 + 1088));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("074-12"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("666-11"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("042-13"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[2]);
            Assert.That(updatedSeat.PlayerActionCount, Is.EqualTo(3));
            Assert.That(updatedSeat.LastActionBlock, Is.EqualTo(0));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[3]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 3 - 1088));
        }

        [Test, Order(12)]
        public void Test_GambleTransition_Four()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, MultiplierType.V4);
            var transitionResult = Engine.Transition(_user, identifier, [human, tracker, seat, bandit], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(4));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<SeatAsset>());
            Assert.That(outputAssets[3], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedHuman = new HumanAsset(outputAssets[0]);

            Assert.That(updatedHuman, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedHuman.Id), Is.EqualTo(prevHumanBalance - 4 + 2));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("331-02"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("577-22"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("373-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("146-10"));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[2]);
            Assert.That(updatedSeat.PlayerActionCount, Is.EqualTo(4));
            Assert.That(updatedSeat.LastActionBlock, Is.EqualTo(0));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[3]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 4 - 2));
        }

        [Test, Order(13)]
        public void Test_Withdraw_Player_Transition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(900));

            var prevUserBalance = _user.Balance.Value;

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);

            var prevHumanBalance = Engine.AssetBalance(human.Id);

            var identifier = CasinoJamIdentifier.Withdraw(AssetType.Player, (AssetSubType)PlayerSubType.Human, TokenType.T_1000);
            var transitionResult = Engine.Transition(_user, identifier, [human], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevHumanBalance - 1_000));

            Assert.That(_user.Balance.Value, Is.EqualTo(prevUserBalance + 1_000));
        }

        [Test, Order(14)]
        public void Test_NoWithdraw_Machine_Transition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1900));

            var prevUserBalance = _user.Balance.Value;

            var machine = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            var prevMachineBalance = Engine.AssetBalance(machine.Id);

            var identifier = CasinoJamIdentifier.Withdraw(AssetType.Machine, (AssetSubType)MachineSubType.Bandit, TokenType.T_1000);
            var transitionResult = Engine.Transition(_user, identifier, [machine], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to MachineAsset and check the properties
            MachineAsset updatedMachine = new MachineAsset(outputAssets[0]);

            Assert.That(updatedMachine, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedMachine.Id), Is.EqualTo(prevMachineBalance));

            Assert.That(_user.Balance.Value, Is.EqualTo(prevUserBalance));
        }

        [Test, Order(15)]
        public void Test_ReleaseTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_900));

            var human = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Human);
            var tracker = GetAsset(_user, AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None);

            var prevHumanBalance = Engine.AssetBalance(human.Id);
            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Release();
            var transitionResult = Engine.Transition(_user, identifier, [human, seat], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);
            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(updatedPlayer.SeatId, Is.EqualTo(0));
            Assert.That(Engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevHumanBalance + 1));

            // Cast to SeatAsset and check the properties
            SeatAsset updatedSeat = new SeatAsset(outputAssets[1]);
            Assert.That(updatedSeat, Is.Not.Null);
            Assert.That(updatedSeat.PlayerId, Is.EqualTo(0));
            Assert.That(updatedSeat.ReservationStartBlock, Is.EqualTo(0));
            Assert.That(updatedSeat.ReservationDuration, Is.EqualTo(ReservationDuration.None));
            Assert.That(updatedSeat.LastActionBlock, Is.EqualTo(0));
            Assert.That(updatedSeat.PlayerActionCount, Is.EqualTo(0));
            Assert.That(Engine.AssetBalance(updatedSeat.Id), Is.EqualTo(prevSeatBalance - 1));
        }

        [Test, Order(16)]
        public void Test_ReturnTransition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(4));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_900));

            var prevUserBalance = _user.Balance.Value;

            var bandit = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit) as MachineAsset;
            var seat = GetAsset(_user, AssetType.Seat, (AssetSubType)SeatSubType.None) as SeatAsset;

            Assert.That(bandit.SeatLinked, Is.EqualTo(1));
            Assert.That(seat.MachineId, Is.EqualTo(bandit.Id));

            var prevBanditBalance = Engine.AssetBalance(bandit.Id);
            var prevSeatBalance = Engine.AssetBalance(seat.Id);

            var identifier = CasinoJamIdentifier.Return();
            var transitionResult = Engine.Transition(_user, identifier, [bandit, seat], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to MachineAsset and check the properties
            MachineAsset updatedBandit = new MachineAsset(outputAssets[0]);
            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(updatedBandit.SeatLinked, Is.EqualTo(0));

            Assert.That(_user.Balance.Value, Is.EqualTo(prevUserBalance));
        }

        [Test, Order(17)]
        public void Test_Withdraw_Machine_Transition()
        {
            Assert.That(Engine.AssetManager.AssetOf(_user).Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1900));

            var prevUserBalance = _user.Balance.Value;

            var machine = GetAsset(_user, AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            var prevMachineBalance = Engine.AssetBalance(machine.Id);

            var identifier = CasinoJamIdentifier.Withdraw(AssetType.Machine, (AssetSubType)MachineSubType.Bandit, TokenType.T_1000);
            var transitionResult = Engine.Transition(_user, identifier, [machine], out IAsset[] outputAssets);
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to MachineAsset and check the properties
            MachineAsset updatedMachine = new MachineAsset(outputAssets[0]);

            Assert.That(updatedMachine, Is.Not.Null);
            Assert.That(Engine.AssetBalance(updatedMachine.Id), Is.EqualTo(prevMachineBalance - 1000));

            Assert.That(_user.Balance.Value, Is.EqualTo(prevUserBalance + 1000));
        }
    }

    public class CasinoJamBaseTest
    {
        public IBlockchainInfoProvider BlockchainInfoProvider { get; }
        public Engine<CasinoJamIdentifier, CasinoJamRule> Engine { get; }

        public CasinoJamBaseTest()
        {
            BlockchainInfoProvider = new BlockchainInfoProvider(1234);
            Engine = CasinoJameGame.Create(BlockchainInfoProvider);
        }

        internal BaseAsset GetAsset(IAccount user, AssetType type, AssetSubType subType)
        {
            BaseAsset? result = Engine.AssetManager
                .AssetOf(user)
                .Select(p => (BaseAsset)p)
                .Where(p => p.AssetType == type && p.AssetSubType == subType)
                .FirstOrDefault();
            Assert.That(result, Is.Not.Null);
            return result;
        }
    }
}