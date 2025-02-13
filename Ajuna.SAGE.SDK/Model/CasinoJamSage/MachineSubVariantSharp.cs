using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// MachineSubVariantSharp
    /// </summary>
    public class MachineSubVariantSharp
    {
        /// <summary>
        /// subVariant
        /// </summary>
        /// <param name="subVariant"></param>
        public MachineSubVariantSharp(EnumMachineSubVariant enumMachineSubVariant)
        {
            MachineSubVariant = enumMachineSubVariant.Value;
            switch(enumMachineSubVariant.Value)
            {
                case MachineSubVariant.Bandit:
                    BanditVariant = new BanditVariantSharp((BanditVariant)enumMachineSubVariant.Value2);
                    break;
            }
        }

        // Parameterless constructor for deserialization
        public MachineSubVariantSharp() { }

        /// <summary>
        /// MachineSubVariant
        /// </summary>
        public MachineSubVariant MachineSubVariant { get; set; }

        /// <summary>
        /// BanditVariantSharp
        /// </summary>
        public BanditVariantSharp? BanditVariant { get; set; }
    }
}