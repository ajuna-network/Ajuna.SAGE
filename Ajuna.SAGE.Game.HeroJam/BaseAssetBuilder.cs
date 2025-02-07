using System.Security.Cryptography;
using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class BaseAssetBuilder
    {
        private uint _id;
        private readonly byte _collectionId;
        private readonly AssetType _assetType;
        private readonly AssetSubType _assetSubType;

        private uint _score = 0;
        private uint _genesis;

        private byte _energy = 0;
        private StateType _stateType = StateType.None;
        private uint _stateChangeBlockNumber = 0;

        public BaseAssetBuilder(uint? id, byte collectionId, AssetType assetType, AssetSubType assetSubType)
        {
            _id = id ?? Utils.GenerateRandomId();
            _collectionId = collectionId;
            _assetType = assetType;
            _assetSubType = assetSubType;
        }

        public BaseAssetBuilder(byte collectionId, AssetType assetType, AssetSubType assetSubType)
            : this(null, collectionId, assetType, assetSubType) { }

        public BaseAssetBuilder SetId(uint id)
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
