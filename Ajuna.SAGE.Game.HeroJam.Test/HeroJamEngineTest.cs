using Ajuna.SAGE.Generic;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    public class HeroJamEngineTest
    {
        private IBlockchainInfoProvider _blockchainInfoProvider;

        private Engine<HeroJamIdentifier, HeroJamRule> _engine;

        [SetUp]
        public void Setup()
        {
            // Create a mock or an actual instance of BlockchainInfoProvider
            _blockchainInfoProvider = new BlockchainInfoProvider(1234);

            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)ActionTime.FullDay);

            var rules = new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.GE, 1);

            TransitionFunction<HeroJamRule> function = (r, w, h, b) =>
            {
                var asset = w.First().Asset;
                asset.Score += 10;
                return [asset];
            };

            // Use the generic EngineBuilder with HeroJamIdentifier
            _engine = HeroJameGame.Create(_blockchainInfoProvider);
        }

        [Test]
        public void Test_HeroJamEngine_CurrentBlockNumber()
        {
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(1));
        }

        // Additional tests can be added here to test engine transitions, etc.
    }
}