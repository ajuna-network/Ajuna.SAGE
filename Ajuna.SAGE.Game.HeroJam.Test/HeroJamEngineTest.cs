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
            Assert.That(outputAssets[0], Is.InstanceOf<HeroAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroAsset = outputAssets[0] as HeroAsset;

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

            var heroInAsset = _player.Assets.ElementAt(0) as HeroAsset;
            Assert.That(heroInAsset, Is.Not.Null);
            Assert.That(heroInAsset.Energy, Is.EqualTo(100));
            Assert.That(heroInAsset.Fatigue, Is.EqualTo(0));
            Assert.That(heroInAsset.Score, Is.EqualTo(0));

            // Set identifier for CreateHero
            var subIdentifier = (byte)WorkType.Hunt << 4 + (byte)ActionTime.Long;
            var identifier = new HeroJamIdentifier((byte)HeroAction.Work, (byte)subIdentifier);

            var inputAssets = _player.Assets?
                .Select(p => new HeroAsset(p))
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
            Assert.That(outputAssets[0], Is.InstanceOf<HeroAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroOutAsset = outputAssets[0] as HeroAsset;

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

            var heroInAsset = _player.Assets.ElementAt(0) as HeroAsset;
            Assert.That(heroInAsset, Is.Not.Null);
            Assert.That(heroInAsset.Energy, Is.EqualTo(100));
            Assert.That(heroInAsset.Fatigue, Is.EqualTo(0));
            Assert.That(heroInAsset.Score, Is.EqualTo(1));

            // Set identifier for CreateHero
            var subIdentifier = (byte)SleepType.Normal << 4 + (byte)ActionTime.Short;
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)subIdentifier);

            var inputAssets = _player.Assets?
                .Select(p => new HeroAsset(p))
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
            Assert.That(outputAssets[0], Is.InstanceOf<HeroAsset>());

            // Cast to HeroJamAsset and check the properties
            var heroOutAsset = outputAssets[0] as HeroAsset;

            // Check that the hero asset is correctly initialized
            Assert.That(heroOutAsset, Is.Not.Null);
            Assert.That(heroOutAsset.AssetType, Is.EqualTo(AssetType.Hero));
            Assert.That(heroOutAsset.AssetSubType, Is.EqualTo(AssetSubType.None));
            Assert.That(heroOutAsset.StateType, Is.EqualTo(StateType.Sleep));
            Assert.That(heroOutAsset.StateChangeBlockNumber, Is.EqualTo(4300));

            Assert.That(heroOutAsset.Energy, Is.EqualTo(82));
            Assert.That(heroOutAsset.Fatigue, Is.EqualTo(24));
        }

        [Test]
        [Order(5)]
        public void Test_HeroJamEngine_CurrentBlockNumber_3()
        {
            _blockchainInfoProvider.CurrentBlockNumber = 4400;
            Assert.That(_blockchainInfoProvider.CurrentBlockNumber, Is.EqualTo(4400));
        }

        [Test]
        [Order(6)]
        public void Test_Disassemble_Transition()
        {
            // First, ensure a hero exists.
            Assert.That(_player.Assets, Is.Not.Null);
            Assert.That(_player.Assets.Count, Is.GreaterThanOrEqualTo(1));
            var hero = _player.Assets.ElementAt(0) as HeroAsset;
            Assert.That(hero, Is.Not.Null);

            // Create a usable animal asset.
            // For example, a Duck that, when disassembled, produces 4 Meat.
            BaseAsset asset = HeroJamUtil.CreateAnimal((AssetSubType)AnimalSubType.Duck, 1);
            _player.Assets.Add(asset);
            // Ensure the asset is a UsableAsset.
            Assert.That(asset, Is.InstanceOf<UsableAsset>());
            Assert.That(asset.AssetFlags[(byte)UseType.Disassemble], Is.True);

            // Prepare the two-asset input: hero at index 0 and usable animal at index 1.
            IAsset[] inputAssets = [hero, asset];

            // Set identifier for Use transition with UseType.Disassemble.
            var identifier = new HeroJamIdentifier((byte)HeroAction.Use, (byte)UseType.Disassemble);

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);
            Assert.That(transitionResult, Is.True);

            // Execute the transition on the player.
            _player.Transition(inputAssets, outputAssets);

            // Expecting two assets: updated hero and a new consumable asset (e.g. Meat).
            Assert.That(outputAssets.Length, Is.EqualTo(2));
            Assert.That(outputAssets[0], Is.InstanceOf<HeroAsset>());
            Assert.That(outputAssets[1], Is.InstanceOf<ConsumableAsset>());

            // Verify that the consumable asset has the expected properties.
            var consumable = outputAssets[1] as ConsumableAsset;
            Assert.That(consumable, Is.Not.Null);
            // For Meat, we expect the effect to be: Effect1HeroStats = Energy and Effect1Value = 7.
            Assert.That(consumable.Effect1HeroStats, Is.EqualTo(HeroStats.Energy));
            Assert.That(consumable.Effect1Value, Is.EqualTo(7));
        }

        [Test]
        [Order(6)]
        public void Test_Consume_Transition()
        {
            // First, create a hero if not already present.
            Assert.That(_player.Assets, Is.Not.Null);
            var hero = _player.Assets.ElementAt(0) as HeroAsset;
            Assert.That(hero, Is.Not.Null);
            Assert.That(hero.Energy, Is.EqualTo(82));

            // Create a consumable asset. For example, Meat that gives +7 Energy.
            BaseAsset asset = HeroJamUtil.CreateItem((AssetSubType)ItemSubType.Meat, 1);
            // Ensure the asset is a ConsumableAsset.
            Assert.That(asset, Is.InstanceOf<ConsumableAsset>());
            Assert.That(asset.AssetFlags[(byte)UseType.Consume], Is.True);

            _player.Assets.Add(asset);

            // Record the hero's energy before consumption.
            byte energyBefore = hero.Energy;

            // Prepare the input: hero at index 0 and the consumable asset at index 1.
            IAsset[] inputAssets = [hero, asset];

            // Set identifier for Use transition with UseType.Consume.
            var identifier = new HeroJamIdentifier((byte)HeroAction.Use, (byte)UseType.Consume);

            var transitionResult = _engine.Transition(_player, identifier, inputAssets, out var outputAssets);
            Assert.That(transitionResult, Is.True);

            // Execute the transition.
            _player.Transition(inputAssets, outputAssets);

            // Expecting only one asset: the updated hero.
            Assert.That(outputAssets.Length, Is.EqualTo(1));
            Assert.That(outputAssets[0], Is.InstanceOf<HeroAsset>());

            var updatedHero = outputAssets[0] as HeroAsset;
            Assert.That(updatedHero, Is.Not.Null);
            // For Meat, the consume effect is to add +7 Energy.
            Assert.That(updatedHero.Energy, Is.EqualTo(Math.Min(energyBefore + 7, 100)));
        }

    }
}