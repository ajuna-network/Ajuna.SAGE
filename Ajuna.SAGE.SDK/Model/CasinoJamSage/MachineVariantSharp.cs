using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.enums;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// MachineVariantSharp
    /// </summary>
    public class MachineVariantSharp
    {
        /// <summary>
        /// MachineVariantSharp
        /// </summary>
        /// <param name="machineVariant"></param>
        public MachineVariantSharp(MachineVariant machineVariant)
        {
            SeatLinked = machineVariant.SeatLinked.Value;
            SeatLimit = machineVariant.SeatLimit.Value;

            Value1Mul = machineVariant.Value1Mul.Value;
            Value2Factor = machineVariant.Value2Factor.Value;
            Value2Mul = machineVariant.Value2Mul.Value;
            Value3Factor = machineVariant.Value3Factor.Value;
            Value3Mul = machineVariant.Value3Mul.Value;
            SubVariant = new MachineSubVariantSharp(machineVariant.SubVariant);

        }

        // Parameterless constructor for deserialization
        public MachineVariantSharp() { }

        /// <summary>
        /// Seat Linked
        /// </summary>
        public byte SeatLinked { get; set; }

        /// <summary>
        /// Seat Limit
        /// </summary>
        public byte SeatLimit { get; set; }

        /// <summary>
        /// Value1Mul
        /// </summary>
        public MultiplierType Value1Mul { get; set; }

        /// <summary>
        /// Value2Factor
        /// </summary>
        public TokenType Value2Factor { get; set; }

        /// <summary>
        /// Value2Mul
        /// </summary>
        public MultiplierType Value2Mul { get; set; }

        /// <summary>
        /// Value3Factor
        /// </summary>
        public TokenType Value3Factor { get; set; }

        /// <summary>
        /// Value3Mul
        /// </summary>
        public MultiplierType Value3Mul { get; set; }

        /// <summary>
        /// SubVariant
        /// </summary>
        public MachineSubVariantSharp SubVariant { get; set; }
    }

}