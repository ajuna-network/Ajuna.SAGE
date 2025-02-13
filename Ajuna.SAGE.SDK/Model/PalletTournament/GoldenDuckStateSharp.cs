using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Wrapped GoldenDuckState
    /// </summary>
    public class GoldenDuckStateSharp
    {
        /// <summary>
        /// GoldenDuckStateSharp constructor
        /// </summary>
        /// <param name="goldenDuckState"></param>
        public GoldenDuckStateSharp(EnumGoldenDuckState goldenDuckState)
        {
            GoldenDuckState = goldenDuckState.Value;
            switch (GoldenDuckState)
            {
                case GoldenDuckState.Disabled:
                    Percentage = null;
                    break;

                case GoldenDuckState.Enabled:
                    var tt = (BaseTuple<U8, BaseOpt<H256>>)goldenDuckState.Value2;
                    Percentage = (U8)tt.Value[0];
                    var optH256 = (BaseOpt<H256>)tt.Value[1];
                    AvatarId = optH256.OptionFlag ? optH256.Value.ToHexString() : null;
                    break;
            }
        }

        /// <summary>
        /// GoldenDuckStateSharp constructor
        /// </summary>
        public GoldenDuckStateSharp()
        { }

        /// <summary>
        /// GoldenDuckState
        /// </summary>
        public GoldenDuckState GoldenDuckState { get; set; }

        /// <summary>
        /// Percentage
        /// </summary>
        public byte? Percentage { get; set; }

        /// <summary>
        /// AvatarId
        /// </summary>
        public string? AvatarId { get; set; }
    }
}