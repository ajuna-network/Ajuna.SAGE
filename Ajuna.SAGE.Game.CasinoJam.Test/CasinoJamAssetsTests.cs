using Ajuna.SAGE.Game.CasinoJam;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamAssetsTests
    {
        [Test]
        public void Test_TrackerAssetTokenProperty()
        {
            var trackerAsset = new TrackerAsset(1);
            trackerAsset.LastReward = 123456u;
            Assert.That(trackerAsset.LastReward, Is.EqualTo(123456u));
        }

        [Test]
        public void Test_MachineAssetTokenProperty()
        {
            var machineAsset = new MachineAsset(1);
            machineAsset.Value1Factor = TokenType.T_10;
            Assert.That(machineAsset.Value1Factor, Is.EqualTo(TokenType.T_10));
            machineAsset.Value1Multiplier = MultiplierType.V1;
            Assert.That(machineAsset.Value1Multiplier, Is.EqualTo(MultiplierType.V1));
            machineAsset.Value2Factor = TokenType.T_100;
            Assert.That(machineAsset.Value2Factor, Is.EqualTo(TokenType.T_100));
            machineAsset.Value2Multiplier = MultiplierType.V2;
            Assert.That(machineAsset.Value2Multiplier, Is.EqualTo(MultiplierType.V2));
            machineAsset.Value3Factor = TokenType.T_1000;
            Assert.That(machineAsset.Value3Factor, Is.EqualTo(TokenType.T_1000));
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