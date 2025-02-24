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

            byte slotIndex0 = 0;
            (ushort, byte) packed0 = (0x4567, 0xDC);
            trackerAsset.SetSlot(slotIndex0, packed0);

            byte slotIndex1 = 1;
            (ushort, byte) packed1 = (0x1234, 0xAB);
            trackerAsset.SetSlot(slotIndex1, packed1);

            byte slotIndex2 = 2;
            (ushort, byte) packed2 = (0xABCD, 0x12);
            trackerAsset.SetSlot(slotIndex2, packed2);

            byte slotIndex3 = 3;
            (ushort, byte) packed3 = (0x9876, 0x45);
            trackerAsset.SetSlot(slotIndex3, packed3);

            // Verify that the set and retrieved slot values match
            Assert.That(trackerAsset.GetSlot(slotIndex0), Is.EqualTo(packed0));
            Assert.That(trackerAsset.GetSlot(slotIndex1), Is.EqualTo(packed1));
            Assert.That(trackerAsset.GetSlot(slotIndex2), Is.EqualTo(packed2));
            Assert.That(trackerAsset.GetSlot(slotIndex3), Is.EqualTo(packed3));
        }
    }
}