using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_affiliates.traits;
using Substrate.NetApi.Model.Types.Primitive;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Wrapped AffiliatorState
    /// </summary>
    public class AffiliatorStateSharp
    {
        /// <summary>
        /// AffiliatorStateSharp constructor
        /// </summary>
        /// <param name="affiliatorState"></param>
        public AffiliatorStateSharp(AffiliatorState affiliatorState)
        {
            Status = affiliatorState.Status.Value;
            Index = affiliatorState.Status.Value == AffiliatableStatus.Affiliatable ?
                ((U32)affiliatorState.Status.Value2).Value :
                (uint?)null;
            Affiliates = affiliatorState.Affiliates;
        }

        /// <summary>
        /// Status
        /// </summary>
        public AffiliatableStatus Status { get; }

        /// <summary>
        /// Index
        /// </summary>
        public uint? Index { get; }

        /// <summary>
        /// Affiliates
        /// </summary>
        public uint Affiliates { get; }
    }
}