using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_balances.types;
using System.Numerics;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Extra Flags C# Wrapper
    /// </summary>
    public class ExtraFlagsSharp
    {
        /// <summary>
        /// Extra Flags Constructor
        /// </summary>
        /// <param name="extraFlags"></param>
        public ExtraFlagsSharp(ExtraFlags extraFlags)
        {
            Value = extraFlags.Value.Value;
        }

        /// <summary>
        /// Value
        /// </summary>
        public BigInteger Value { get; }
    }
}