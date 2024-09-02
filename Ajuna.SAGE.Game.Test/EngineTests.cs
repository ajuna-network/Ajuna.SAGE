using Ajuna.SAGE.Generic.Model;
using Moq;

namespace Ajuna.SAGE.Generic.Tests
{
    [TestFixture]
    public class EngineTests
    {
        private Mock<IBlockchainInfoProvider> _mockBlockchainInfoProvider;
        private Engine<ActionIdentifier, ActionRule> _engine;

        [SetUp]
        public void Setup()
        {
            // Create a mock without passing any constructor arguments
            _mockBlockchainInfoProvider = new Mock<IBlockchainInfoProvider>();

            // Set up the necessary methods in the mock
            _mockBlockchainInfoProvider.Setup(m => m.GenerateRandomHash()).Returns(new byte[] { 1, 2, 3, 4 });
            _mockBlockchainInfoProvider.Setup(m => m.CurrentBlockNumber).Returns(100);

            _engine = new Engine<ActionIdentifier, ActionRule>(_mockBlockchainInfoProvider.Object, (p, r, a, b) => true);
        }

        [Test]
        public void Test_AddTransition_And_Transition_Valid()
        {
            var player = new Player(new byte[] { 1 });

            // Arrange
            var identifier = new ActionIdentifier(ActionType.TypeA, ActionSubType.TypeX);

            var rules = new ActionRule(ActionRuleType.MinAsset, ActionRuleOp.GreaterEqual, 1);

            TransitionFunction<ActionRule> function = (r, w, h, b) =>
            {
                var asset = w.First().Asset;
                asset.Score += 10;
                return new List<Asset> { asset };
            };

            _engine.AddTransition(identifier, new[] { rules }, function);

            var assets = new Asset[]
            {
                new Asset(new byte[] { 1 }, 1, 50)
            };

            // Act
            var flag = _engine.Transition(player, identifier, assets, out Asset[] result);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(flag, Is.True, "The transition should be true.");
                Assert.That(result.Length, Is.EqualTo(1), "There should be one resulting asset.");
                Assert.That(result.First().Score, Is.EqualTo(60), "The resulting asset's score should be 60.");
            });
        }

        [Test]
        public void Test_Transition_DuplicateAssets_ThrowsException()
        {
            var player = new Player(new byte[] { 1 });

            // Arrange
            var identifier = new ActionIdentifier(ActionType.TypeA, ActionSubType.TypeX);
            var rules = new ActionRule(ActionRuleType.MinAsset, ActionRuleOp.GreaterEqual, 1);

            TransitionFunction<ActionRule> function = (r, w, h, b) => w.Select(wrappedAsset => wrappedAsset.Asset);

            _engine.AddTransition(identifier, new[] { rules }, function);

            var duplicateAsset = new Asset(new byte[] { 1 }, 1, 50);
            var assets = new Asset[] { duplicateAsset, duplicateAsset };

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => _engine.Transition(player, identifier, assets, out Asset[] result), "Trying to Forge duplicates.");
        }

        [Test]
        public void Test_Transition_UnsupportedIdentifier_ThrowsException()
        {
            var player = new Player(new byte[] { 1 });

            // Arrange
            var unsupportedIdentifier = new ActionIdentifier((ActionType)99, (ActionSubType)99);
            var assets = new Asset[]
            {
                new Asset(new byte[] { 1 }, 1, 50)
            };

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => _engine.Transition(player, unsupportedIdentifier, assets, out Asset[] result), "Unsupported Transition for Identifier.");
        }

        [Test]
        public void Test_Transition_InvalidAssetCount_ReturnsFalse()
        {
            var player = new Player(new byte[] { 1 });

            // Arrange
            var identifier = new ActionIdentifier(ActionType.TypeA, ActionSubType.TypeX);
            var rule = new ActionRule(ActionRuleType.MinAsset, ActionRuleOp.GreaterEqual, 2);

            TransitionFunction<ActionRule> function = (r, w, h, b) => w.Select(wrappedAsset => wrappedAsset.Asset);

            var blockchainInfoProvider = new Mock<IBlockchainInfoProvider>();
            blockchainInfoProvider.Setup(b => b.GenerateRandomHash()).Returns(new byte[] { 0x00 });
            blockchainInfoProvider.Setup(b => b.CurrentBlockNumber).Returns(1u);

            // Setting up the Engine with custom Verify function
            var engine = new EngineBuilder<ActionIdentifier, ActionRule>(blockchainInfoProvider.Object)
                .SetVerifyFunction((player, rule, assets, blocknumber) =>
                {
                    if (rule.RuleType == (byte)ActionRuleType.MinAsset && rule.RuleOp == (byte)ActionRuleOp.GreaterEqual)
                    {
                        return assets.Length >= BitConverter.ToUInt32(rule.RuleValue);
                    }

                    return false;
                })
                .AddTransition(identifier, new[] { rule }, function)
                .Build();

            var assets = new Asset[]
            {
                new Asset(new byte[] { 1 }, 1, 50)
            };

            // Act
            bool success = engine.Transition(player, identifier, assets, out Asset[] result);

            // Assert
            Assert.That(success, Is.False, "Expected Transition to return false due to insufficient assets.");
            Assert.That(result, Is.Empty, "Expected result to be empty when transition fails.");
        }
    }
}