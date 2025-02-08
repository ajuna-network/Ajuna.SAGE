using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Model;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamMachineAssetTests
    {
        [Test]
        public void Test_MachineAssetTokenProperties()
        {
            // Create a new MachineAsset using a genesis value (e.g., 1)
            var machineAsset = new MachineAsset(1);

            // Test Value1Factor and Value1Multiplier (stored in byte at offset 8)
            machineAsset.Value1Factor = TokenType.T_10;
            Assert.That(machineAsset.Value1Factor, Is.EqualTo(TokenType.T_10));
            machineAsset.Value1Multiplier = MultiplierType.V1;
            Assert.That(machineAsset.Value1Multiplier, Is.EqualTo(MultiplierType.V1));

            // Test Value2Factor and Value2Multiplier (stored in byte at offset 9)
            machineAsset.Value2Factor = TokenType.T_100;
            Assert.That(machineAsset.Value2Factor, Is.EqualTo(TokenType.T_100));
            machineAsset.Value2Multiplier = MultiplierType.V2;
            Assert.That(machineAsset.Value2Multiplier, Is.EqualTo(MultiplierType.V2));

            // Test Value3Factor and Value3Multiplier (stored in byte at offset 10)
            machineAsset.Value3Factor = TokenType.T_1000;
            Assert.That(machineAsset.Value3Factor, Is.EqualTo(TokenType.T_1000));
            machineAsset.Value3Multiplier = MultiplierType.V3;
            Assert.That(machineAsset.Value3Multiplier, Is.EqualTo(MultiplierType.V3));
        }

        [Test]
        public void Test_BanditAssetProperties()
        {
            // Create a new BanditAsset using a genesis value (e.g., 1)
            var banditAsset = new BanditAsset(1);

            // Verify that the BanditAsset is a MachineAsset with subtype Bandit
            Assert.That(banditAsset.AssetType, Is.EqualTo(AssetType.Machine));
            Assert.That(banditAsset.AssetSubType, Is.EqualTo((AssetSubType)MachineSubType.Bandit));

            // By default, the constructor sets MaxSpins to 4.
            Assert.That(banditAsset.MaxSpins, Is.EqualTo(4));

            // Test modifying MaxSpins
            banditAsset.MaxSpins = 6;
            Assert.That(banditAsset.MaxSpins, Is.EqualTo(6));

            // Test Jackpot property (stored as a 16-bit field at offset 24)
            uint jackpotValue = 5000;
            banditAsset.Jackpot = jackpotValue;
            Assert.That(banditAsset.Jackpot, Is.EqualTo(jackpotValue));
        }
    }
}