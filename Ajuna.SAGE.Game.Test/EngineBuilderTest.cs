using Ajuna.SAGE.Game.Model;
using Moq;

namespace Ajuna.SAGE.Game.Test
{
    [TestFixture]
    public class EngineBuilderTest
    {
        private Mock<IBlockchainInfoProvider> _mockBlockchainInfoProvider;

        [SetUp]
        public void Setup()
        {
            // Create a mock without passing any constructor arguments
            _mockBlockchainInfoProvider = new Mock<IBlockchainInfoProvider>();

            // Set up the necessary methods in the mock
            _mockBlockchainInfoProvider.Setup(m => m.GenerateRandomHash()).Returns(new byte[] { 1, 2, 3, 4 });
            _mockBlockchainInfoProvider.Setup(m => m.CurrentBlockNumber).Returns(100);
        }

        [Test]
        public void Test_EngineBuilder_CreatesEngineWithTransitions()
        {
            var player = new Account(1);

            // Arrange
            var identifier = new ActionIdentifier(ActionType.TypeA, ActionSubType.TypeX);
            var rules = new ActionRule(ActionRuleType.MinAsset, ActionRuleOp.GreaterEqual, 1);

            TransitionFunction<ActionRule> function = (e, r, f, w, h, b, m) =>
            {
                var asset = w.First();
                asset.Score += 10;
                return new List<IAsset> { asset };
            };

            var engine = new EngineBuilder<ActionIdentifier, ActionRule>(_mockBlockchainInfoProvider.Object)
                .SetVerifyFunction((p, r, a, b, m, s) => true)
                .AddTransition(identifier, new[] { rules }, default, function)
                .Build();

            var assets = new Asset[]
            {
                new Asset(1, 0, 1, 50, 0, [])
            };

            // Act
            var transitionFlag = engine.Transition(player, identifier, assets, out IAsset[] transitionResult);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(transitionFlag, Is.True, "The transition should be true.");
                Assert.That(transitionResult.Length, Is.EqualTo(1), "There should be one resulting asset.");
                Assert.That(transitionResult.First().Score, Is.EqualTo(60), "The resulting asset's score should be 60.");
            });
        }

        [Test]
        public void Test_EngineBuilder_CanAddMultipleTransitions()
        {
            var player = new Account(1);

            // Arrange
            var identifier1 = new ActionIdentifier(ActionType.TypeA, ActionSubType.TypeX);
            var identifier2 = new ActionIdentifier(ActionType.TypeB, ActionSubType.TypeY);

            var rules1 = new ActionRule(ActionRuleType.MinAsset, ActionRuleOp.GreaterEqual, 1);
            var rules2 = new ActionRule(ActionRuleType.MaxAsset, ActionRuleOp.LesserEqual, 5);

            TransitionFunction<ActionRule> function1 = (e, r, f, w, h, b, m) =>
            {
                var asset = w.First();
                asset.Score += 10;
                return new List<IAsset> { asset };
            };

            TransitionFunction<ActionRule> function2 = (e, r, f, w, h, b, m) =>
            {
                var asset = w.First();
                asset.Score += 20;
                return new List<IAsset> { asset };
            };

            var engine = new EngineBuilder<ActionIdentifier, ActionRule>(_mockBlockchainInfoProvider.Object)
                .SetVerifyFunction((p, r, a, b, m, s) => true)
                .AddTransition(identifier1, [rules1], default, function1)
                .AddTransition(identifier2, [rules2], default, function2)
                .Build();

            var assets = new Asset[]
            {
                new(1, 0, 1, 50, 0, [])
            };

            // Act
            var transitionFlag1 = engine.Transition(player, identifier1, assets, out IAsset[] transitionResult);

            Assert.That(transitionFlag1, Is.True);
            Assert.That(transitionResult.First().Score, Is.EqualTo(60), "The resulting asset's score should be 60 after the first transition.");

            var transitionFlag2 = engine.Transition(player, identifier2, assets, out IAsset[] transitionResult2);

            Assert.That(transitionFlag2, Is.True);
            Assert.That(transitionResult2.First().Score, Is.EqualTo(80), "The resulting asset's score should be 80 after the second transition.");
        }
    }
}