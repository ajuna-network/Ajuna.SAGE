using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    public class HeroJamEngineTest
    {
        private IBlockchainInfoProvider _blockchainInfoProvider;

        private Engine<HeroJamIdentifier, HeroJamRule> _engine;

        private Player _player;

        [SetUp]
        public void Setup()
        {
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
        public void Test_CreateHero_Transition()
        {
            // Set identifier for CreateHero
            var identifier = new HeroJamIdentifier((byte)HeroAction.Create, (byte)AssetType.Hero);

            var transitionResult = _engine.Transition(_player, identifier, null, out var transitionAssets);
            
            // Perform the transition
            Assert.That(transitionResult, Is.True);

            // Verify that the hero was created
            Assert.That(transitionAssets.Length, Is.EqualTo(1));
            Assert.That(transitionAssets[0], Is.InstanceOf<HeroJamAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroAsset = transitionAssets[0] as HeroJamAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroAsset, Is.Not.Null);
            Assert.That(heroAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroAsset.Energy, Is.EqualTo(100));
            Assert.That(heroAsset.StateType, Is.EqualTo(StateType.Idle));
            Assert.That(heroAsset.StateChangeBlockNumber, Is.EqualTo(0));
        }

    }
}