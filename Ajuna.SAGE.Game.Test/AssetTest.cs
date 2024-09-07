using NUnit.Framework;
using Ajuna.SAGE.Generic;
using System.Linq;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Tests
{
    [TestFixture]
    public class AssetTests
    {
        [Test]
        public void Test_AssetConstructor_WithByteArrayDna()
        {
            // Arrange
            byte[] id = [1, 2, 3, 4];
            byte collectionId = 1;
            uint score = 100;
            uint genesis = 0;
            byte[] dna = [5, 6, 7, 8];

            // Act
            Asset asset = new Asset(id, collectionId, score, genesis, dna);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(asset.Id, Is.EqualTo(id), "The ID should match the provided value.");
                Assert.That(asset.CollectionId, Is.EqualTo(collectionId), "The SetId should match the provided value.");
                Assert.That(asset.Score, Is.EqualTo(score), "The Score should match the provided value.");
                Assert.That(asset.Data, Is.EqualTo(dna), "The DNA should match the provided value.");
            });
        }

        [Test]
        public void Test_AssetConstructor_WithHexStringDna()
        {
            // Arrange
            byte[] id = [1, 2, 3, 4];
            byte collectionId = 1;
            uint score = 100;
            uint genesis = 0;
            string hexStringDna = "05060708";

            // Act
            Asset asset = new Asset(id, collectionId, score, genesis, hexStringDna);

            // Assert
            Assert.That(asset.Data, Is.EqualTo(Utils.HexToBytes(hexStringDna)), "The DNA should match the value converted from the hex string.");
        }

        [Test]
        public void Test_AssetConstructor_WithDefaults()
        {
            // Arrange
            byte[] id = [1, 2, 3, 4];
            byte collectionId = 1;
            uint score = 100;
            uint genesis = 0;

            // Act
            Asset asset = new Asset(id, collectionId, score, genesis);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(asset.Id, Is.EqualTo(id), "The ID should match the provided value.");
                Assert.That(asset.CollectionId, Is.EqualTo(collectionId), "The SetId should match the provided value.");
                Assert.That(asset.Score, Is.EqualTo(score), "The Score should match the provided value.");
                Assert.That(asset.Data, Is.EqualTo(new byte[Constants.DNA_SIZE]), "The DNA should be initialized to the default size.");
            });
        }

        [Test]
        public void Test_Asset_Empty()
        {
            // Arrange
            byte[] id = [1, 2, 3, 4];
            byte collectionId = 1;

            // Act
            Asset emptyAsset = Asset.Empty(id, collectionId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(emptyAsset.Id, Is.EqualTo(id), "The ID should match the provided value.");
                Assert.That(emptyAsset.CollectionId, Is.EqualTo(collectionId), "The SetId should match the provided value.");
                Assert.That(emptyAsset.Score, Is.EqualTo(0), "The Score should be initialized to 0.");
                Assert.That(emptyAsset.Data, Is.EqualTo(new byte[Constants.DNA_SIZE]), "The DNA should be initialized to the default size.");
            });
        }

        [Test]
        public void Test_Asset_Equals()
        {
            byte collectionId = 1;
            uint score = 100;
            uint genesis = 0;

            // Arrange
            byte[] id1 = [1, 2, 3, 4];
            byte[] id2 = [1, 2, 3, 4];
            byte[] id3 = [4, 3, 2, 1];

            Asset asset1 = new Asset(id1, collectionId, score, genesis);
            Asset asset2 = new Asset(id2, collectionId, score, genesis);
            Asset asset3 = new Asset(id3, collectionId, score, genesis);

            // Act & Assert
            Assert.Multiple(() =>
            {
                Assert.That(asset1.Equals(asset2), Is.True, "Assets with the same ID should be equal.");
                Assert.That(asset1.Equals(asset3), Is.False, "Assets with different IDs should not be equal.");
                Assert.That(asset1.Equals(null), Is.False, "An asset should not be equal to null.");
            });
        }
    }
}
