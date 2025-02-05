using Ajuna.SAGE.Game.Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace Ajuna.SAGE.Game.Test
{
    public class AvatarToolsTest
    {
        [Test]
        public void BitEnumSelectorTest()
        {
            byte bitSelector = 0b01010101;

            var expected = new List<NibbleType> {
                NibbleType.X0,
                NibbleType.X2,
                NibbleType.X4,
                NibbleType.X6
            };

            Assert.That(AssetTools.BitsToEnums<NibbleType>(bitSelector), Is.EqualTo(expected));
        }

        [Test]
        public void BitEnumOrder1Test()
        {
            byte bitOrder = 0b01101100;

            var list = new List<NibbleType> {
                NibbleType.X0,
                NibbleType.X2,
                NibbleType.X4,
                NibbleType.X6
            };

            var expected = new List<NibbleType> {
                NibbleType.X2,
                NibbleType.X4,
                NibbleType.X6,
                NibbleType.X0
            };

            Assert.That(AssetTools.BitsOrderToEnums(bitOrder, list), Is.EqualTo(expected));
        }

        [Test]
        public void BitEnumOrder2Test()
        {
            byte bitOrder = 0b01110010;

            var list = new List<NibbleType> {
                NibbleType.X4, // 0 = 00
                NibbleType.X5, // 1 = 01
                NibbleType.X6, // 2 = 10
                NibbleType.X7  // 3 = 11
            };

            var expected = new List<NibbleType> {
                NibbleType.X5, // 1 = 01
                NibbleType.X7, // 3 = 11
                NibbleType.X4, // 0 = 00
                NibbleType.X6  // 2 = 10
            };

            Assert.That(AssetTools.BitsOrderToEnums(bitOrder, list), Is.EqualTo(expected));
        }

        [Test]
        public void BluePrintPatternByteTest()
        {
            var pattern = new List<NibbleType> {
                NibbleType.X2,
                NibbleType.X4,
                NibbleType.X1,
                NibbleType.X3
            };

            Assert.That(AssetTools.EnumsToBits(pattern), Is.EqualTo(0b00011110));
        }

        [Test]
        public void BluePrintOrderByteTest()
        {
            var pattern = new List<NibbleType> {
                NibbleType.X2,
                NibbleType.X4,
                NibbleType.X1,
                NibbleType.X3
            };
            Assert.That(AssetTools.EnumsOrderToBits(pattern), Is.EqualTo(0b01110010));
        }

        [Test]
        public void CurrentPeriodTest()
        {
            Assert.That(AssetTools.CurrentPeriod(10, 14, 0), Is.EqualTo(0));
            Assert.That(AssetTools.CurrentPeriod(10, 14, 9), Is.EqualTo(0));
            Assert.That(AssetTools.CurrentPeriod(10, 14, 10), Is.EqualTo(1));
            Assert.That(AssetTools.CurrentPeriod(10, 14, 19), Is.EqualTo(1));
            // ...
            Assert.That(AssetTools.CurrentPeriod(10, 14, 130), Is.EqualTo(13));
            Assert.That(AssetTools.CurrentPeriod(10, 14, 139), Is.EqualTo(13));
            Assert.That(AssetTools.CurrentPeriod(10, 14, 140), Is.EqualTo(0));
        }
    }
}