using Ajuna.SAGE.Generic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ajuna.SAGE.Game.Model
{

    /// <summary>
    /// Balance class
    /// </summary>
    public class Balance : IBalance
    {
        public uint Value { get ; private set; }

        /// <summary>
        /// Balance constructor
        /// </summary>
        public Balance() : this(0) { }

        /// <summary>
        /// Balance constructor
        /// </summary>
        public Balance(uint value)
        {
            Value = value;
        }

        public void Deposit(uint amount)
        {
            Value += amount;
        }

        public bool Withdraw(uint amount)
        {
            if (Value >= amount)
            {
                Value -= amount;
                return true;
            }

            return false;
        }

        public void SetValue(uint value)
        {
            Value = value;
        }

        /// <inheritdoc/>
        public bool Equals(IBalance? other)
        {
            if (other == null)
            {
                return false;
            }

            return Value == other.Value;
        }

    }
}
