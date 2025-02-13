using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_system;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Account Info C# Wrapper
    /// </summary>
    public class AccountInfoSharp
    {
        /// <summary>
        /// Account Info Constructor
        /// </summary>
        /// <param name="accountInfo"></param>
        public AccountInfoSharp(AccountInfo accountInfo)
        {
            Nonce = accountInfo.Nonce.Value;
            Consumers = accountInfo.Consumers.Value;
            Providers = accountInfo.Providers.Value;
            Sufficients = accountInfo.Sufficients.Value;
            Data = new AccountDataSharp(accountInfo.Data);
        }

        /// <summary>
        /// Nonce
        /// </summary>
        public uint Nonce { get; }

        /// <summary>
        /// Consumers
        /// </summary>
        public uint Consumers { get; }

        /// <summary>
        /// Providers
        /// </summary>
        public uint Providers { get; }

        /// <summary>
        /// Sufficients
        /// </summary>
        public uint Sufficients { get; }

        /// <summary>
        /// Data
        /// </summary>
        public AccountDataSharp Data { get; }
    }
}