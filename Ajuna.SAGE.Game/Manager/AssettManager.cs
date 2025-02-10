using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Manager
{
    public interface IAssetManager
    {
        uint Create(IAsset asset);
        IAsset? Read(uint id);
        bool Update(IAsset asset);
        bool Delete(IAsset asset);
        bool Delete(uint id);

        /// <summary>
        /// Assets of IAccount
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<IAsset> AssetOf(IAccount account);
    }

    public class AssetManager : IAssetManager
    {
        private uint _nextId;

        private readonly Dictionary<uint, IAsset> _data;

        public AssetManager()
        {
            _nextId = 1000;
            _data = [];
        }

        public uint Create(IAsset asset)
        {
            uint id = _nextId++;
            asset.Id = id;
            _data.Add(id, asset);
            return id;
        }

        public IAsset? Read(uint id)
        {
            if (!_data.TryGetValue(id, out IAsset? asset))
            {
                return null;
            }
            return asset;
        }

        public bool Update(IAsset asset)
        {
            if (!_data.Remove(asset.Id))
            {
                return false;
            }

            _data.Add(asset.Id, asset);
            return true;
        }

        public bool Delete(IAsset asset)
        {
            return Delete(asset.Id);
        }
        public bool Delete(uint id)
        {
            return _data.Remove(id);
        }

        /// <inheritdoc/>
        public IEnumerable<IAsset> AssetOf(IAccount account)
        {
            return _data.Values.Where(p => p.OwnedBy(account)).ToList();
        }
    }
}