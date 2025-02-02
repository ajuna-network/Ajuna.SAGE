using System.Security.Cryptography;
using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class BaseAssetBuilder
    {
        private ulong _id;
        private readonly byte _collectionId;
        private readonly AssetType _assetType;
        private readonly AssetSubType _assetSubType;

        private uint _score = 0;
        private uint _genesis;

        private byte _energy = 0;
        private StateType _stateType = StateType.None;
        private uint _stateChangeBlockNumber = 0;

        public BaseAssetBuilder(ulong? id, byte collectionId, AssetType assetType, AssetSubType assetSubType)
        {
            _id = id ?? GenerateRandomId();
            _collectionId = collectionId;
            _assetType = assetType;
            _assetSubType = assetSubType;
        }

        public BaseAssetBuilder(byte collectionId, AssetType assetType, AssetSubType assetSubType)
            : this(null, collectionId, assetType, assetSubType) { }

        private ulong GenerateRandomId()
        {
            var id = new byte[8];
            RandomNumberGenerator.Fill(id);

            return BitConverter.ToUInt64(id, 0);
        }

        public BaseAssetBuilder SetId(ulong id)
        {
            _id = id;
            return this;
        }

        public BaseAssetBuilder SetGenesis(uint genesis)
        {
            _genesis = genesis;
            return this;
        }

        public BaseAssetBuilder SetScore(uint score)
        {
            _score = score;
            return this;
        }

        public BaseAsset Build()
        {
            var asset = new BaseAsset(_id, _collectionId, _score, _genesis)
            {
                AssetType = _assetType,
                AssetSubType = _assetSubType,
            };

            return asset;
        }

    }
}
