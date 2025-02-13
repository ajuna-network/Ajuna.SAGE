using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
using Substrate.NetApi.Model.Types.Primitive;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// HumanVariantSharp
    /// </summary>
    public class HumanVariantSharp
    {
        /// <summary>
        /// HumanVariantSharp
        /// </summary>
        /// <param name="value2"></param>
        public HumanVariantSharp(HumanVariant humanVariant)
        {
            SeatId = humanVariant.SeatId.OptionFlag ? humanVariant.SeatId.Value.Value : (uint?)null;
        }

        // Parameterless constructor for deserialization
        public HumanVariantSharp() { }

        /// <summary>
        /// SeatId
        /// </summary>
        public uint? SeatId { get; set; }
    }
}