namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Balance interface
    /// </summary>
    public interface IBalance
    {
        uint Value { get; }

        bool Withdraw(uint amount);

        void Deposit(uint amount);
    }
}
