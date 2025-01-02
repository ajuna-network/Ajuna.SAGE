namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Balance interface
    /// </summary>
    public interface IBalance : IEquatable<IBalance>
    {
        uint Value { get; set; }

        bool Withdraw(uint amount);

        void Deposit(uint amount);

        void SetValue(uint value);
    }
}
