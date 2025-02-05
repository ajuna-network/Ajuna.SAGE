
namespace Ajuna.SAGE.Game
{
    public interface IAssetBalanceManager
    {
        bool Deposit(ulong id, uint balance);
        bool Withdraw(ulong id, uint balance);
        ulong AllAssetBalances();
        uint? AssetBalance(ulong id);
    }

    public class AssetBalanceManager : IAssetBalanceManager
    {
        // Internal storage for asset balances.
        private readonly Dictionary<ulong, uint> _assetsBalances = [];

        public bool Deposit(ulong id, uint balance)
        {
            if (_assetsBalances.TryGetValue(id, out uint currentBalance))
            {
                var capacity = uint.MaxValue - currentBalance;
                if (balance > capacity)
                {
                    return false;
                }
                _assetsBalances[id] = currentBalance + balance;
            }
            else
            {
                _assetsBalances[id] = balance;
            }
            return true;
        }

        public bool Withdraw(ulong id, uint balance)
        {
            if (!_assetsBalances.TryGetValue(id, out uint currentBalance))
            {
                return false;
            }
            if (balance > currentBalance)
            {
                return false;
            }
            _assetsBalances[id] = currentBalance - balance;
            return true;
        }

        public ulong AllAssetBalances() => (ulong)_assetsBalances.Sum(x => x.Value);
        public uint? AssetBalance(ulong id) => _assetsBalances.TryGetValue(id, out uint balance) ? balance : null;
    }
}