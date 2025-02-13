using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// AssetSharp
    /// </summary>
    public class AssetVariantSharp
    {
        /// <summary>
        /// AssetVariantSharp
        /// </summary>
        /// <param name="variant"></param>
        public AssetVariantSharp(EnumAssetVariant variant)
        {
            AssetVariant = variant.Value;
            switch(variant.Value)
            {
                case AssetVariant.Player:
                    PlayerVariantSharp = new PlayerVariantSharp((EnumPlayerVariant)variant.Value2);
                    break;
                case AssetVariant.Machine:
                    MachineVariantSharp = new MachineVariantSharp((MachineVariant)variant.Value2);
                    break;
                case AssetVariant.Seat:
                    SeatVariantSharp = new SeatVariantSharp((SeatVariant)variant.Value2);
                    break;
            }
        }

        // Parameterless constructor for deserialization
        public AssetVariantSharp() { }

        /// <summary>
        /// AssetVariant
        /// </summary>
        public AssetVariant AssetVariant { get; set; }

        /// <summary>
        /// PlayerVariantSharp
        /// </summary>
        public PlayerVariantSharp? PlayerVariantSharp { get;  set; }

        /// <summary>
        /// MachineVariantSharp
        /// </summary>
        public MachineVariantSharp? MachineVariantSharp { get;  set; }

        /// <summary>
        /// SeatVariantSharp
        /// </summary>
        public SeatVariantSharp? SeatVariantSharp { get;  set; }
    }
}