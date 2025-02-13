using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// BanditVariantSharp
    /// </summary>
    public class BanditVariantSharp
    {
        /// <summary>
        /// BanditVariantSharp
        /// </summary>
        /// <param name="value2"></param>
        public BanditVariantSharp(BanditVariant banditVariant)
        {
            MaxSpins = banditVariant.MaxSpins.Value;
            Jackpot = banditVariant.Jackpot.Value;
        }

        // Parameterless constructor for deserialization
        public BanditVariantSharp() { }

        /// <summary>
        /// MaxSpins
        /// </summary>
        public byte MaxSpins { get; set; }

        /// <summary>
        /// Jackpot
        /// </summary>
        public uint Jackpot { get; set; }
    }
}