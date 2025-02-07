using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamGameTests
    {
        private IBlockchainInfoProvider _blockchainInfoProvider;
        private Engine<CasinoJamIdentifier, CasinoJamRule> _engine;
        private Player _user;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Initialize blockchain info, engine, and player.
            _blockchainInfoProvider = new BlockchainInfoProvider(1234);
            _engine = CasinoJameGame.Create(_blockchainInfoProvider);
            // Create a player with initial balance 100.
            _user = new Player(Utils.GenerateRandomId(), 1_002_000);
        }

        [Test, Order(0)]
        public void Test_CurrentBlockNumber()
        {
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(1));
        }

        [Test, Order(1)]
        public void Test_CreatePlayerTransition()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(0));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);

            IAsset[]? inputAssets = null;

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

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
            Assert.That(_engine.AssetBalance(human.Id), Is.Null);

            var tracker = outputAssets[1] as TrackerAsset;

            // Check that the tracker asset is correctly initialized
            Assert.That(tracker, Is.Not.Null);
            Assert.That(tracker.AssetType, Is.EqualTo(AssetType.Player));
            Assert.That(tracker.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Tracker));
            Assert.That(_engine.AssetBalance(human.Id), Is.Null);

            Assert.That(_user.Assets?.Count, Is.EqualTo(2));
        }

        [Test, Order(2)]
        public void Test_CreateMachineTransition_Bandit()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(2));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            var identifier = CasinoJamIdentifier.Create(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            IAsset[]? inputAssets = null;

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

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

            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
        }

        [Test, Order(3)]
        public void Test_FundMachineTransition()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_002_000));

            var machine = _user.Assets.ElementAt(2);

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Fund(AssetType.Machine, TokenType.T_1000000);

            IAsset[]? inputAssets = [machine];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<BaseAsset>());

            // Cast to PlayerAsset and check the properties
            var asset = outputAssets[0] as BaseAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(asset, Is.Not.Null);
            Assert.That(asset.AssetType, Is.EqualTo(AssetType.Machine));
            Assert.That(_engine.AssetBalance(asset.Id), Is.EqualTo(1_000_000));

            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(2_000));
        }

        [Test, Order(4)]
        public void Test_GambleTransition_NoToken()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(2_000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, CasinoJam.MultiplierType.V2);

            IAsset[]? inputAssets = [player, tracker, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(3));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[2]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance));

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedBandit.SlotAResult);
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedBandit.SlotBResult);
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedBandit.SlotCResult);
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedBandit.SlotDResult);
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));
        }

        [Test, Order(5)]
        public void Test_FundPlayerTransition()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(2_000));

            var player = _user.Assets.ElementAt(0);

            // Player creation transition expects no input assets.
            var identifier = CasinoJamIdentifier.Fund(AssetType.Player, TokenType.T_1000);

            IAsset[]? inputAssets = [player];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

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
            Assert.That(_engine.AssetBalance(asset.Id), Is.EqualTo(1_000));

            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            Assert.That(_user.Balance.Value, Is.EqualTo(1_000));
        }

        [Test, Order(6)]
        public void Test_GambleTransition_Once()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, MultiplierType.V1);

            IAsset[]? inputAssets = [player, tracker, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(3));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance - 1));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("131-33"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[2]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 1));
        }

        [Test, Order(8)]
        public void Test_GambleTransition_Twice()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, CasinoJam.MultiplierType.V2);

            IAsset[]? inputAssets = [player, tracker, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(3));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance - 2));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("257-11"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("247-11"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("000-00"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[2]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 2));
        }

        [Test, Order(9)]
        public void Test_GambleTransition_Three()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, CasinoJam.MultiplierType.V3);

            IAsset[]? inputAssets = [player, tracker, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(3));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance - 3));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("763-20"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("054-30"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("123-33"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("000-00"));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[2]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 3));
        }

        [Test, Order(10)]
        public void Test_GambleTransition_Four()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Gamble(TokenType.T_1, CasinoJam.MultiplierType.V4);

            IAsset[]? inputAssets = [player, tracker, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(3));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<TrackerAsset>());
            Assert.That(outputAssets[2], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance - 4 + 2120));

            // Cast to TrackerAsset and check the properties
            TrackerAsset updatedTracker = new TrackerAsset(outputAssets[1]);

            var slotAResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(0));
            var SlotBResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(1));
            var SlotCResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(2));
            var SlotDResult = CasinoJamUtil.UnpackSlotResult(updatedTracker.GetSlot(3));
            Assert.That($"{slotAResult.slot1}{slotAResult.slot2}{slotAResult.slot3}-{slotAResult.bonus1}{slotAResult.bonus2}", Is.EqualTo("074-12"));
            Assert.That($"{SlotBResult.slot1}{SlotBResult.slot2}{SlotBResult.slot3}-{SlotBResult.bonus1}{SlotBResult.bonus2}", Is.EqualTo("666-11"));
            Assert.That($"{SlotCResult.slot1}{SlotCResult.slot2}{SlotCResult.slot3}-{SlotCResult.bonus1}{SlotCResult.bonus2}", Is.EqualTo("042-13"));
            Assert.That($"{SlotDResult.slot1}{SlotDResult.slot2}{SlotDResult.slot3}-{SlotDResult.bonus1}{SlotDResult.bonus2}", Is.EqualTo("222-10"));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[2]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance + 4 - 2120));
        }

        [Test, Order(11)]
        public void Test_LootTransition()
        {
            Assert.That(_user.Assets?.Count, Is.EqualTo(3));
            // initial balance
            Assert.That(_user.Balance.Value, Is.EqualTo(1_000));

            var player = new HumanAsset(_user.Assets.ElementAt(0));
            var prevPlayerBalance = _engine.AssetBalance(player.Id);

            var tracker = new BanditAsset(_user.Assets.ElementAt(1));

            var bandit = new BanditAsset(_user.Assets.ElementAt(2));
            var prevBanditBalance = _engine.AssetBalance(bandit.Id);

            var identifier = CasinoJamIdentifier.Loot(TokenType.T_1000);

            IAsset[]? inputAssets = [player, bandit];

            var transitionResult = _engine.Transition(_user, identifier, inputAssets, out IAsset[] outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _user.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets, Is.Not.Null);
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<HumanAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<BanditAsset>());

            // Cast to HumanAsset and check the properties
            HumanAsset updatedPlayer = new HumanAsset(outputAssets[0]);

            Assert.That(updatedPlayer, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedPlayer.Id), Is.EqualTo(prevPlayerBalance + 1000));

            // Cast to MachineAsset and check the properties
            BanditAsset updatedBandit = new BanditAsset(outputAssets[1]);

            Assert.That(updatedBandit, Is.Not.Null);
            Assert.That(_engine.AssetBalance(updatedBandit.Id), Is.EqualTo(prevBanditBalance - 1000));

        }
    }
}