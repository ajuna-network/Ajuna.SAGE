using System.Security.Cryptography;
using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamAssetBuilder
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

        public HeroJamAssetBuilder(ulong? id, byte collectionId, AssetType assetType, AssetSubType assetSubType)
        {
            _id = id ?? GenerateRandomId();
            _collectionId = collectionId;
            _assetType = assetType;
            _assetSubType = assetSubType;
        }

        public HeroJamAssetBuilder(byte collectionId, AssetType assetType, AssetSubType assetSubType)
            : this(null, collectionId, assetType, assetSubType) { }

        private ulong GenerateRandomId()
        {
            var id = new byte[8];
            RandomNumberGenerator.Fill(id);

            return BitConverter.ToUInt64(id, 0);
        }

        public HeroJamAssetBuilder SetId(ulong id)
        {
            _id = id;
            return this;
        }

        public HeroJamAssetBuilder SetGenesis(uint genesis)
        {
            _genesis = genesis;
            return this;
        }

        public HeroJamAssetBuilder SetScore(uint score)
        {
            _score = score;
            return this;
        }

        public HeroJamAssetBuilder SetEnergy(byte energy)
        {
            _energy = energy;
            return this;
        }

        public HeroJamAssetBuilder SetStateType(StateType stateType)
        {
            _stateType = stateType;
            return this;
        }

        public HeroJamAssetBuilder SetStateChangeBlockNumber(uint stateChangeBlockNumber)
        {
            _stateChangeBlockNumber = stateChangeBlockNumber;
            return this;
        }

        public HeroJamAsset Build()
        {
            var asset = new HeroJamAsset(_id, _collectionId, _score, _genesis)
            {
                AssetType = _assetType,
                AssetSubType = _assetSubType,
                Genesis = _genesis,
                Energy = _energy,
                StateType = _stateType,
                StateChangeBlockNumber = _stateChangeBlockNumber
            };

            return asset;
        }
    }
}
