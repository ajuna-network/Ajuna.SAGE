using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Balance interface
    /// </summary>
    public interface IBalance : IEquatable<IBalance>
    {
        uint Value { get; }

        bool Withdraw(uint amount);

        void Deposit(uint amount);

        void SetValue(uint value);
    }
}
