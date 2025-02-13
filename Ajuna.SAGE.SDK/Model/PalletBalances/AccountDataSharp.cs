using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_balances.types;
using System.Numerics;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Account Data C# Wrapper
    /// </summary>
    public class AccountDataSharp
    {
        /// <summary>
        /// Account Data Constructor
        /// </summary>
        /// <param name="accountData"></param>
        public AccountDataSharp(AccountData accountData)
        {
            Free = accountData.Free.Value;
            Reserved = accountData.Reserved.Value;
            Frozen = accountData.Frozen.Value;
            Flags = new ExtraFlagsSharp(accountData.Flags);
        }

        /// <summary>
        /// Free
        /// </summary>
        public BigInteger Free { get; }

        /// <summary>
        /// Reserved
        /// </summary>
        public BigInteger Reserved { get; }

        /// <summary>
        /// Frozen
        /// </summary>
        public BigInteger Frozen { get; }

        /// <summary>
        /// Flags
        /// </summary>
        public ExtraFlagsSharp Flags { get; }
    }
}