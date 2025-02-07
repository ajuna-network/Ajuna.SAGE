namespace Ajuna.SAGE.Game.Manager
{
    public interface ILock
    {
        bool CanLock(ulong id, out bool lockState);

        bool Lock(ulong id);

        bool CanUnlock(ulong id, out bool lockState);

        bool Unlock(ulong id);
    }

    public class LockManager : ILock
    {
        private readonly Dictionary<ulong, bool> _data = [];

        public bool CanLock(ulong id, out bool state)
        {
            return !_data.TryGetValue(id, out state) || !state;
        }

        public bool Lock(ulong id)
        {
            if (!CanLock(id, out _))
            {
                return false;
            }
            _data[id] = true;
            return true;
        }

        public bool CanUnlock(ulong id, out bool state)
        {
            return _data.TryGetValue(id, out state) && state;
        }

        public bool Unlock(ulong id)
        {
            if (!CanUnlock(id, out _))
            {
                return false;
            }

            _data[id] = false;

            return true;
        }

        public bool? IsLocked(ulong id) => _data.TryGetValue(id, out bool state) ? state : null;
    }
}