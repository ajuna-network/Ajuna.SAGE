using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Manager
{
    public interface IAccountManager
    {
        uint Create();

        bool Destroy(uint id);

        IAccount? Account(uint id);

        uint EngineId { get; }
    }

    public class AccountManager : IAccountManager
    {
        private uint _nextId;

        private readonly Dictionary<uint, IAccount> _data = [];

        private readonly uint _engineId;
        public uint EngineId => _engineId;

        public AccountManager()
        {
            _nextId = 10000000;
            _data = [];

            _engineId = Create();
        }

        public uint Create()
        {
            uint id = _nextId++;
            _data.Add(id, new Account(id, 0));
            return id;
        }

        public bool Destroy(uint id)
        {
            if (!_data.ContainsKey(id))
            {
                return false;
            }
            _data.Remove(id);
            return true;
        }

        public IAccount? Account(uint id)
        {
            if (!_data.TryGetValue(id, out IAccount? account))
            {
                return null; 
            }
            return account;
        }
    }
}