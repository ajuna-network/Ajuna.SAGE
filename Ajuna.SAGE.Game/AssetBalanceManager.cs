
namespace Ajuna.SAGE.Game
{
    public interface IAssetBalanceManager
    {
        bool CanDeposit(ulong id, uint balance, out uint currentBalance);
        bool Deposit(ulong id, uint balance);
        bool CanWithdraw(ulong id, uint balance, out uint currentBalance);
        bool Withdraw(ulong id, uint balance);
        ulong AllAssetBalances();
        uint? AssetBalance(ulong id);
    }

    public class AssetBalanceManager : IAssetBalanceManager
    {
        private readonly Dictionary<ulong, uint> _assetsBalances = [];

        public bool CanDeposit(ulong id, uint balance, out uint currentBalance)
        {
            return !_assetsBalances.TryGetValue(id, out currentBalance) || balance <= uint.MaxValue - currentBalance;
        }

        public bool Deposit(ulong id, uint balance)
        {
            if (!CanDeposit(id, balance, out uint currentBalance))
            {
                return false;
            }
            _assetsBalances[id] = currentBalance + balance;
            return true;
        }

        public bool CanWithdraw(ulong id, uint balance, out uint currentBalance)
        {
            return _assetsBalances.TryGetValue(id, out currentBalance) && balance <= currentBalance;
        }

        public bool Withdraw(ulong id, uint balance)
        {
            if (!CanWithdraw(id, balance, out uint currentBalance))
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