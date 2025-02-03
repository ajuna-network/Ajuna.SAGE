using Ajuna.SAGE.Game.CasinoJam;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamAssetsTests
    {
        [Test]
        public void Test_PlayerAssetTokenProperty()
        {
            var playerAsset = new PlayerAsset(1000, 1);
            // Set the token property (stored as a 32-bit uint at Data[8-11])
            playerAsset.Token = 123456u;
            Assert.That(playerAsset.Token, Is.EqualTo(123456u));
        }

        [Test]
        public void Test_MachineAssetTokenProperty()
        {
            var machineAsset = new MachineAsset(10_000_000, 1);
            machineAsset.Token = 654321u;
            Assert.That(machineAsset.Token, Is.EqualTo(654321u));
        }

        [Test]
        public void Test_BanditAssetSlotResultProperty()
        {
            var bandit = new BanditAsset(10_000_000, 1);
            ushort value = CasinoJamUtil.PackSlotResult(7, 7, 7, 1, 2);
            bandit.SlotAResult = value;
            Assert.That(bandit.SlotAResult, Is.EqualTo(value));
        }
    }
}