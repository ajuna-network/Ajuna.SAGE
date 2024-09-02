using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Generic.Test
{
    public class DataTest
    {
        [Test]
        public void SetTest()
        {
            var dna = new byte[Constants.DNA_SIZE];

            Assert.That(dna, Is.Not.Null);
            Assert.That(dna.ToHexDna, Has.Length.EqualTo(64)); 
            Assert.That(dna.ToHexDna, Is.EqualTo("0000000000000000000000000000000000000000000000000000000000000000"));

            dna.Set(0, ByteType.High, 1);
            Assert.That(dna.ToHexDna, Is.EqualTo("1000000000000000000000000000000000000000000000000000000000000000"));

            dna.Set(0, ByteType.Low, 2);
            Assert.That(dna.ToHexDna, Is.EqualTo("1200000000000000000000000000000000000000000000000000000000000000"));

            dna.Set(0, ByteType.Full, 255);
            Assert.That(dna.ToHexDna, Is.EqualTo("FF00000000000000000000000000000000000000000000000000000000000000"));

            dna.Set(0, ByteType.Low, 0);
            Assert.That(dna.ToHexDna, Is.EqualTo("F000000000000000000000000000000000000000000000000000000000000000"));

            dna.Set(0, ByteType.High, 0);
            Assert.That(dna.ToHexDna, Is.EqualTo("0000000000000000000000000000000000000000000000000000000000000000"));
        }

        [Test]
        public void ReadTest()
        {
            var dna = Utils.HexToBytes("1234000000000000000000000000000000000000000000000000000000000000");

            Assert.That(dna, Is.Not.Null);
            Assert.That(dna.ToHexDna, Has.Length.EqualTo(64));
            Assert.That(dna.ToHexDna, Is.EqualTo("1234000000000000000000000000000000000000000000000000000000000000"));

            var result0 = dna.Read(0, ByteType.High);
            Assert.That(result0, !Is.Null);
            Assert.That(result0, Is.EqualTo(1));

            var result1 = dna.Read(0, ByteType.Low);
            Assert.That(result1, !Is.Null);
            Assert.That(result1, Is.EqualTo(2));

            var result2 = dna.Read(0, ByteType.Full);
            Assert.That(result2, !Is.Null);
            Assert.That(result2, Is.EqualTo(18));
        }
    }
}