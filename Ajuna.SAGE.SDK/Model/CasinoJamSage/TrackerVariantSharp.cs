using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// TrackerVariantSharp
    /// </summary>
    public class TrackerVariantSharp
    {
        /// <summary>
        /// TrackerVariantSharp
        /// </summary>
        /// <param name="value2"></param>
        public TrackerVariantSharp(TrackerVariant trackerVariant)
        {
            SlotAResult = trackerVariant.SlotAResult.Value;
            SlotBResult = trackerVariant.SlotBResult.Value;
            SlotCResult = trackerVariant.SlotCResult.Value;
            SlotDResult = trackerVariant.SlotDResult.Value;
            LastReward = trackerVariant.LastReward.Value;
        }

        // Parameterless constructor for deserialization
        public TrackerVariantSharp() { }

        /// <summary>
        /// SlotAResult
        /// </summary>
        public ushort SlotAResult { get; set; }

        /// <summary>
        /// SlotBResult
        /// </summary>
        public ushort SlotBResult { get; set; }

        /// <summary>
        /// SlotCResult
        /// </summary>
        public ushort SlotCResult { get; set; }

        /// <summary>
        /// SlotDResult
        /// </summary>
        public ushort SlotDResult { get; set; }

        /// <summary>
        /// LastReward
        /// </summary>
        public uint LastReward { get; set; }
    }
}