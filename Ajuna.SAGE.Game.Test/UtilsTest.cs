namespace Ajuna.SAGE.Game.Test
{
    public class UtilsTest
    {
        [Test]
        public void Test_ValidHexString_ReturnsByteArray()
        {
            // Arrange
            string hexString = "0x0A1B2C3D";
            byte[] expectedBytes = new byte[] { 0x0A, 0x1B, 0x2C, 0x3D };

            // Act
            byte[] result = Utils.HexToBytes(hexString);

            // Assert
            Assert.That(result, Is.EqualTo(expectedBytes));
        }

        [Test]
        public void Test_OddLengthHexString_ThrowsNotSupportedException()
        {
            // Arrange
            string hexString = "0x0A1B2C3";

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => Utils.HexToBytes(hexString));
        }

        [Test]
        public void Test_InvalidCharactersInHexString_ThrowsNotSupportedException()
        {
            // Arrange
            string hexString = "0x0A1B2C3Z";

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => Utils.HexToBytes(hexString));
        }

        [Test]
        public void RandomEnum_ReturnsRandomEnumValue()
        {
            // Arrange
            var random = new Random();

            // Act
            var result = Utils.RandomEnum<ItemSubTypeA>(random);

            // Assert
            Assert.That(Enum.IsDefined(typeof(ItemSubTypeA), result), Is.True);
        }

        [Test]
        public void ChunkBy_ShouldReturnCorrectChunkedList()
        {
            // Arrange
            var sourceList = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            int chunkSize = 3;
            var expectedResult = new List<List<int>>
            {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 },
                new List<int> { 7, 8, 9 },
                new List<int> { 10 }
            };

            // Act
            var result = sourceList.ChunkBy(chunkSize);

            // Assert
            Assert.That(result, Has.Count.EqualTo(expectedResult.Count));
            for (int i = 0; i < expectedResult.Count; i++)
            {
                Assert.That(result[i], Is.EqualTo(expectedResult[i]));
            }
        }

        [Test]
        public void SetNibbleTest()
        {
            byte colorBits = 0b0000_0000;
            colorBits = colorBits.SetHighNibble(0x01);
            Assert.That(colorBits, Is.EqualTo(16));

            colorBits = colorBits.SetLowNibble(0x05);
            Assert.That(colorBits, Is.EqualTo(21));
        }
    }
}