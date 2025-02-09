using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamPlayerAssetTests
    {
        [Test]
        public void Test_HumanAssetType()
        {
            // Create a HumanAsset using a genesis value (e.g., 1)
            var humanAsset = new HumanAsset(0, 1);

            // Verify that the asset type is set to Player
            Assert.That(humanAsset.AssetType, Is.EqualTo(AssetType.Player));

            // Verify that the asset subtype is set to Human (cast to AssetSubType)
            Assert.That(humanAsset.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Human));
        }

        [Test]
        public void Test_TrackerAssetTypeAndLastReward()
        {
            // Create a TrackerAsset using a genesis value (e.g., 1)
            var trackerAsset = new TrackerAsset(0, 1);

            // Verify that the asset type is set to Player
            Assert.That(trackerAsset.AssetType, Is.EqualTo(AssetType.Player));

            // Verify that the asset subtype is set to Tracker (cast to AssetSubType)
            Assert.That(trackerAsset.AssetSubType, Is.EqualTo((AssetSubType)PlayerSubType.Tracker));

            // Test the LastReward property
            uint expectedReward = 123456u;
            trackerAsset.LastReward = expectedReward;
            Assert.That(trackerAsset.LastReward, Is.EqualTo(expectedReward));
        }

        [Test]
        public void Test_TrackerAssetSlotOperations()
        {
            // Create a TrackerAsset using a genesis value (e.g., 1)
            var trackerAsset = new TrackerAsset(0, 1);

            // Define a test slot index and a packed value (16-bit value)
            byte slotIndex = 2;
            ushort packedValue = 0xABCD;  // Example value

            // Set the slot at the given index
            trackerAsset.SetSlot(slotIndex, packedValue);

            // Retrieve the slot value using GetSlot
            ushort retrievedValue = trackerAsset.GetSlot(slotIndex);

            // Verify that the set and retrieved slot values match
            Assert.That(retrievedValue, Is.EqualTo(packedValue));
        }
    }
}