using Ajuna.SAGE.Game.CasinoJam;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamAssetsTests
    {
        [Test]
        public void Test_PlayerAssetTokenProperty()
        {
            var playerAsset = new PlayerAsset(1);
            playerAsset.LastReward = 123456u;
            Assert.That(playerAsset.LastReward, Is.EqualTo(123456u));
        }

        [Test]
        public void Test_MachineAssetTokenProperty()
        {
            var machineAsset = new MachineAsset(1);
            //machineAsset.Token = 654321u;
            //Assert.That(machineAsset.Token, Is.EqualTo(654321u));
        }

        [Test]
        public void Test_BanditAssetSlotResultProperty()
        {
            var bandit = new BanditAsset(1);
            ushort value = CasinoJamUtil.PackSlotResult(7, 7, 7, 1, 2);
            bandit.SlotAResult = value;
            Assert.That(bandit.SlotAResult, Is.EqualTo(value));
        }
    }
}