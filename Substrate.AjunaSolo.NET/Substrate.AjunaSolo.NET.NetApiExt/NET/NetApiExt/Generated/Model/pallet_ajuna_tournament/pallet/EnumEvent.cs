//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.pallet
{
    
    
    /// <summary>
    /// >> Event
    /// The `Event` enum of this pallet
    /// </summary>
    public enum Event
    {
        
        /// <summary>
        /// >> TournamentCreated
        /// </summary>
        TournamentCreated = 0,
        
        /// <summary>
        /// >> TournamentRemoved
        /// </summary>
        TournamentRemoved = 1,
        
        /// <summary>
        /// >> TournamentActivePeriodStarted
        /// </summary>
        TournamentActivePeriodStarted = 2,
        
        /// <summary>
        /// >> TournamentClaimPeriodStarted
        /// </summary>
        TournamentClaimPeriodStarted = 3,
        
        /// <summary>
        /// >> TournamentEnded
        /// </summary>
        TournamentEnded = 4,
        
        /// <summary>
        /// >> EntityEnteredRanking
        /// </summary>
        EntityEnteredRanking = 5,
        
        /// <summary>
        /// >> EntityBecameGoldenDuck
        /// </summary>
        EntityBecameGoldenDuck = 6,
        
        /// <summary>
        /// >> RankingRewardClaimed
        /// </summary>
        RankingRewardClaimed = 7,
        
        /// <summary>
        /// >> GoldenDuckRewardClaimed
        /// </summary>
        GoldenDuckRewardClaimed = 8,
    }
    
    /// <summary>
    /// >> 77 - Variant[pallet_ajuna_tournament.pallet.Event]
    /// The `Event` enum of this pallet
    /// </summary>
    public sealed class EnumEvent : BaseEnumRust<Event>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumEvent()
        {
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.TournamentCreated);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.TournamentRemoved);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.TournamentActivePeriodStarted);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.TournamentClaimPeriodStarted);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.TournamentEnded);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.EntityEnteredRanking);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(Event.EntityBecameGoldenDuck);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(Event.RankingRewardClaimed);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>>(Event.GoldenDuckRewardClaimed);
        }
    }
}
