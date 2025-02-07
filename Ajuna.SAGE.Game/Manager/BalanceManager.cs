namespace Ajuna.SAGE.Game.Manager
{
    public interface IBalanceManager
    {
        bool CanDeposit(ulong id, uint balance, out uint currentBalance);

        bool Deposit(ulong id, uint balance);

        bool CanWithdraw(ulong id, uint balance, out uint currentBalance);

        bool Withdraw(ulong id, uint balance);

        ulong AllAssetBalances();

        uint? AssetBalance(ulong id);
    }

    public class BalanceManager : IBalanceManager
    {
        private readonly Dictionary<ulong, uint> _data = [];

        public bool CanDeposit(ulong id, uint balance, out uint currentBalance)
        {
            return !_data.TryGetValue(id, out currentBalance) || balance <= uint.MaxValue - currentBalance;
        }

        public bool Deposit(ulong id, uint balance)
        {
            if (!CanDeposit(id, balance, out uint currentBalance))
            {
                return false;
            }
            _data[id] = currentBalance + balance;
            return true;
        }

        public bool CanWithdraw(ulong id, uint balance, out uint currentBalance)
        {
            return _data.TryGetValue(id, out currentBalance) && balance <= currentBalance;
        }

        public bool Withdraw(ulong id, uint balance)
        {
            if (!CanWithdraw(id, balance, out uint currentBalance))
            {
                return false;
            }

            _data[id] = currentBalance - balance;
            return true;
        }

        public ulong AllAssetBalances() => (ulong)_data.Sum(x => x.Value);
        public uint? AssetBalance(ulong id) => _data.TryGetValue(id, out uint balance) ? balance : null;
    }
}