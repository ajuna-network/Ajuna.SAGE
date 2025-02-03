using Ajuna.SAGE.Game.CasinoJam;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class CasinoJamUtilityTests
    {
        [Test]
        public void Test_PackAndUnpackSlotResult()
        {
            // Use sample values: Slot1=7, Slot2=7, Slot3=7, Bonus1=0, Bonus2=0.
            ushort packed = CasinoJamUtil.PackSlotResult(7, 7, 7, 0, 0);
            var (s1, s2, s3, bonus1, bonus2) = CasinoJamUtil.UnpackSlotResult(packed);
            Assert.That(s1, Is.EqualTo(7));
            Assert.That(s2, Is.EqualTo(7));
            Assert.That(s3, Is.EqualTo(7));
            Assert.That(bonus1, Is.EqualTo(0));
            Assert.That(bonus2, Is.EqualTo(0));
        }
    }
}