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
            _player = new Player(Utils.GenerateRandomId());
        }

        [Test]
        public void Test_HeroJamEngine_CurrentBlockNumber()
        {
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(1));
        }

        [Test]
        [Order(1)]
        public void Test_CreateHero_Transition()
        {
            Assert.That(_player.Assets?.Count, Is.EqualTo(0));

            // Set identifier for CreateHero
            var identifier = new HeroJamIdentifier((byte)HeroAction.Create, (byte)AssetType.Hero);

            IAsset[]? inputAssets = null;

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);

            // transition succeded
            Assert.That(transitionResult, Is.True);

            // Do palyer transition
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
            Assert.That(heroAsset.StateType, Is.EqualTo(StateType.Idle));
            Assert.That(heroAsset.StateChangeBlockNumber, Is.EqualTo(0));

            Assert.That(_player.Assets?.Count, Is.EqualTo(1));
        }

        [Test]
        [Order(2)]
        public void Test_Sleep_Transition()
        {
            Assert.That(_player.Assets?.Count, Is.EqualTo(1));

            // Set identifier for CreateHero
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)ActionTime.Short);

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
            var heroAsset = outputAssets[0] as HeroJamAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroAsset, Is.Not.Null);
            Assert.That(heroAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroAsset.Energy, Is.EqualTo(100));
            Assert.That(heroAsset.StateType, Is.EqualTo(StateType.Sleep));
            Assert.That(heroAsset.StateChangeBlockNumber, Is.EqualTo(601));
        }

    }
}