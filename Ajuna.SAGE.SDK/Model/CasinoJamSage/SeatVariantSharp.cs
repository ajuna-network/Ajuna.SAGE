using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.enums;
using Substrate.NetApi.Model.Types.Primitive;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// SeatVariantSharp
    /// </summary>
    public class SeatVariantSharp
    {
        /// <summary>
        /// SeatVariantSharp
        /// </summary>
        /// <param name="value2"></param>
        public SeatVariantSharp(SeatVariant value2)
        {
            RentDuration = value2.RentDuration.Value;
            PlayerFee = value2.PlayerFee.Value;
            PlayerGracePeriod = value2.PlayerGracePeriod.Value;
            ReservationStartBlock = value2.ReservationStartBlock.Value;
            ReservationDuration = value2.ReservationDuration.Value;
            LastActionBlock = value2.LastActionBlock.Value;
            PlayerActionCount = value2.PlayerActionCount.Value;
            PlayerId = value2.PlayerId.OptionFlag ? value2.PlayerId.Value.Value : (uint?)null;
            MachineId = value2.MachineId.OptionFlag ? value2.MachineId.Value.Value : (uint?) null;
        }

        // Parameterless constructor for deserialization
        public SeatVariantSharp() { }

        /// <summary>
        /// SeatValidityPeriod
        /// </summary>
        public RentDuration RentDuration { get; set; }

        /// <summary>
        /// PlayerFee
        /// </summary>
        public ushort PlayerFee { get; set; }

        /// <summary>
        /// PlayerGracePeriod
        /// </summary>
        public byte PlayerGracePeriod { get; set; }

        /// <summary>
        /// ReservationStartBlock
        /// </summary>
        public uint ReservationStartBlock { get; set; }

        /// <summary>
        /// ReservationDuration
        /// </summary>
        public ReservationDuration ReservationDuration { get; set; }

        /// <summary>
        /// LastActionBlock
        /// </summary>
        public ushort LastActionBlock { get; set; }

        /// <summary>
        /// PlayerActionCount
        /// </summary>
        public ushort PlayerActionCount { get; set; }

        /// <summary>
        /// PlayerId
        /// </summary>
        public uint? PlayerId { get; set; }

        /// <summary>
        /// MachineId
        /// </summary>
        public uint? MachineId { get; set; }
    }
}