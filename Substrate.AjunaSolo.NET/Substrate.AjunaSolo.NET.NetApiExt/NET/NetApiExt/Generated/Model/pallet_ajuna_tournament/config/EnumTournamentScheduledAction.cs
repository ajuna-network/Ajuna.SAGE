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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config
{
    
    
    /// <summary>
    /// >> TournamentScheduledAction
    /// </summary>
    public enum TournamentScheduledAction
    {
        
        /// <summary>
        /// >> StartActivePhase
        /// </summary>
        StartActivePhase = 0,
        
        /// <summary>
        /// >> SwitchToClaimPhase
        /// </summary>
        SwitchToClaimPhase = 1,
        
        /// <summary>
        /// >> EndClaimPhase
        /// </summary>
        EndClaimPhase = 2,
    }
    
    /// <summary>
    /// >> 352 - Variant[pallet_ajuna_tournament.config.TournamentScheduledAction]
    /// </summary>
    public sealed class EnumTournamentScheduledAction : BaseEnumRust<TournamentScheduledAction>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumTournamentScheduledAction()
        {
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(TournamentScheduledAction.StartActivePhase);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(TournamentScheduledAction.SwitchToClaimPhase);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.NetApi.Model.Types.Primitive.U32>>(TournamentScheduledAction.EndClaimPhase);
        }
    }
}
