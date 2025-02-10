using System.Security.Cryptography;
using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class BaseAssetBuilder
    {
        private uint _id;
        private uint _ownerId;
        private readonly byte _collectionId;
        private readonly AssetType _assetType;
        private readonly AssetSubType _assetSubType;

        private uint _score = 0;
        private uint _genesis;

        private byte _energy = 0;
        private StateType _stateType = StateType.None;
        private uint _stateChangeBlockNumber = 0;

        public BaseAssetBuilder(uint ownerId, byte collectionId, AssetType assetType, AssetSubType assetSubType)
        {
            _ownerId = ownerId;
            _collectionId = collectionId;
            _assetType = assetType;
            _assetSubType = assetSubType;
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
            var asset = new BaseAsset(0, _ownerId, _collectionId, _score, _genesis)
            {
                AssetType = _assetType,
                AssetSubType = _assetSubType,
            };

            return asset;
        }

    }
}
