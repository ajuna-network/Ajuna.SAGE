using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// PlayerVariantSharp
    /// </summary>
    public class PlayerVariantSharp
    {
        /// <summary>
        /// PlayerVariantSharp
        /// </summary>
        /// <param name="enumPlayerVariant"></param>
        public PlayerVariantSharp(EnumPlayerVariant enumPlayerVariant)
        {
            PlayerVariant = enumPlayerVariant.Value;
            switch(enumPlayerVariant.Value)
            {
                case PlayerVariant.Human:
                    HumanVariant = new HumanVariantSharp((HumanVariant)enumPlayerVariant.Value2);
                    break;
                case PlayerVariant.Tracker:
                    TrackerVariant = new TrackerVariantSharp((TrackerVariant)enumPlayerVariant.Value2);
                    break;
            }
        }

        // Parameterless constructor for deserialization
        public PlayerVariantSharp() { }

        /// <summary>
        /// PlayerVariant
        /// </summary>
        public PlayerVariant PlayerVariant { get; set; }

        /// <summary>
        /// HumanVariantSharp
        /// </summary>
        public HumanVariantSharp? HumanVariant { get; set; }

        /// <summary>
        /// TrackerVariantSharp
        /// </summary>
        public TrackerVariantSharp? TrackerVariant { get; private set; }
    }
}