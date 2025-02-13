using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.NetApi.Model.Types.Primitive;

namespace Substrate.Integration.Model.PalletTournament
{
    /// <summary>
    /// GoldenDuckConfigSharp
    /// </summary>
    public class GoldenDuckConfigSharp
    {
        /// <summary>
        /// GoldenDuckConfigSharp constructor
        /// </summary>
        /// <param name="goldenDuckConfig"></param>
        public GoldenDuckConfigSharp(EnumGoldenDuckConfig goldenDuckConfig)
        {
            GoldenDuckConfig = goldenDuckConfig.Value;
            switch (GoldenDuckConfig)
            {
                case GoldenDuckConfig.Disabled:
                    RewardPercentage = null;
                    break;

                case GoldenDuckConfig.Enabled:
                    RewardPercentage = (U8)goldenDuckConfig.Value2;
                    break;
            }
        }

        /// <summary>
        /// GoldenDuckConfig
        /// </summary>
        public GoldenDuckConfig GoldenDuckConfig { get; }

        /// <summary>
        /// RewardPercentage
        /// </summary>
        public byte? RewardPercentage { get; }
    }
}