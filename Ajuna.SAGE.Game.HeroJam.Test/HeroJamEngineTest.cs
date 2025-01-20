using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    public class HeroJamEngineTest
    {
        private IBlockchainInfoProvider _blockchainInfoProvider;

        private Engine<HeroJamIdentifier, HeroJamRule> _engine;

        private Player _player;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Initialize objects that are shared across all tests
            _blockchainInfoProvider = new BlockchainInfoProvider(1234);
            _engine = HeroJameGame.Create(_blockchainInfoProvider);
            _player = new Player(Utils.GenerateRandomId(), 100);
        }

        [Test]
        [Order(0)]
        public void Test_HeroJamEngine_CurrentBlockNumber_1()
        {
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(1));
        }

        [Test]
        [Order(1)]
        public void Test_CreateHero_Transition()
        {
            Assert.That(_player.Assets?.Count, Is.EqualTo(0));
            // initial balance
            Assert.That(_player.Balance.Value, Is.EqualTo(100));

            // Set identifier for CreateHero
            var identifier = new HeroJamIdentifier((byte)HeroAction.Create, (byte)AssetType.Hero);

            IAsset[]? inputAssets = null;

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do player transition
            _player.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<HeroJamAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroAsset = outputAssets[0] as HeroJamAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroAsset, Is.Not.Null);
            Assert.That(heroAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroAsset.Energy, Is.EqualTo(100));
            Assert.That(heroAsset.StateType, Is.EqualTo(StateType.None));
            Assert.That(heroAsset.StateChangeBlockNumber, Is.EqualTo(0));
            Assert.That(heroAsset.Balance.Value, Is.EqualTo(10));

            Assert.That(_player.Assets?.Count, Is.EqualTo(1));
            // balance after create hero
            Assert.That(_player.Balance.Value, Is.EqualTo(90));
        }

        [Test]
        [Order(2)]
        public void Test_Work_Transition()
        {
            Assert.That(_player.Assets, Is.Not.Null);
            Assert.That(_player.Assets?.Count, Is.EqualTo(1));

            var heroInAsset = _player.Assets.ElementAt(0) as HeroJamAsset;
            Assert.That(heroInAsset, Is.Not.Null);
            Assert.That(heroInAsset.Energy, Is.EqualTo(100));
            Assert.That(heroInAsset.Fatigue, Is.EqualTo(0));
            Assert.That(heroInAsset.Score, Is.EqualTo(0));

            // Set identifier for CreateHero
            var subIdentifier = (byte)WorkType.Hunt << 4 + (byte)ActionTime.Long;
            var identifier = new HeroJamIdentifier((byte)HeroAction.Work, (byte)subIdentifier);

            var inputAssets = _player.Assets?
                .Select(p => new HeroJamAsset(p))
                .Where(p => p.AssetType == AssetType.Hero)
                .ToArray();

            Assert.That(inputAssets, Is.Not.Null);
            Assert.That(inputAssets.Count, Is.EqualTo(1));

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do palyer transition
            _player.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<HeroJamAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroOutAsset = outputAssets[0] as HeroJamAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroOutAsset, Is.Not.Null);
            Assert.That(heroOutAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroOutAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroOutAsset.StateType, Is.EqualTo(StateType.Work));
            Assert.That(heroOutAsset.StateChangeBlockNumber, Is.EqualTo(3601));

            Assert.That(heroOutAsset.Energy, Is.EqualTo(100));
            Assert.That(heroOutAsset.Fatigue, Is.EqualTo(0));
            Assert.That(heroInAsset.Score, Is.EqualTo(1));
        }

        [Test]
        [Order(3)]
        public void Test_HeroJamEngine_CurrentBlockNumber_2()
        {
            _blockchainInfoProvider.CurrentBlockNumber = 3700;
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(3700));
        }

        [Test]
        [Order(4)]
        public void Test_Sleep_Transition()
        {
            Assert.That(_player.Assets, Is.Not.Null);
            Assert.That(_player.Assets?.Count, Is.EqualTo(1));

            var heroInAsset = _player.Assets.ElementAt(0) as HeroJamAsset;
            Assert.That(heroInAsset, Is.Not.Null);
            Assert.That(heroInAsset.Energy, Is.EqualTo(100));
            Assert.That(heroInAsset.Fatigue, Is.EqualTo(0));
            Assert.That(heroInAsset.Score, Is.EqualTo(1));

            // Set identifier for CreateHero
            var subIdentifier = (byte)SleepType.Normal << 4 + (byte)ActionTime.Short;
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)subIdentifier);

            var inputAssets = _player.Assets?
                .Select(p => new HeroJamAsset(p))
                .Where(p => p.AssetType == AssetType.Hero && p.AssetSubType == AssetSubType.None)
                .ToArray();

            Assert.That(inputAssets, Is.Not.Null);
            Assert.That(inputAssets.Count, Is.EqualTo(1));

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do palyer transition
            _player.Transition(inputAssets, outputAssets);

            // Verify that the hero was created
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<HeroJamAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroOutAsset = outputAssets[0] as HeroJamAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroOutAsset, Is.Not.Null);
            Assert.That(heroOutAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroOutAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroOutAsset.StateType, Is.EqualTo(StateType.Sleep));
            Assert.That(heroOutAsset.StateChangeBlockNumber, Is.EqualTo(4300));

            Assert.That(heroOutAsset.Energy, Is.EqualTo(82));
            Assert.That(heroOutAsset.Fatigue, Is.EqualTo(24));
        }

    }
}