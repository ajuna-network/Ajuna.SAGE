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
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> NoActiveTournamentForCategory
        /// There's no active tournament for the selected category.
        /// </summary>
        NoActiveTournamentForCategory = 0,
        
        /// <summary>
        /// >> CannotRemoveActiveTournament
        /// The current tournament is active, so it cannot be removed.
        /// </summary>
        CannotRemoveActiveTournament = 1,
        
        /// <summary>
        /// >> TournamentNotInClaimPeriod
        /// The current tournament is not in its reward claim period.
        /// </summary>
        TournamentNotInClaimPeriod = 2,
        
        /// <summary>
        /// >> LatestTournamentAlreadyStarted
        /// The latest tournament for the selected category identifier already started,
        /// so it cannot be removed anymore.
        /// </summary>
        LatestTournamentAlreadyStarted = 3,
        
        /// <summary>
        /// >> AnotherTournamentAlreadyActiveForCategory
        /// There's already an active tournament for the selected category.
        /// </summary>
        AnotherTournamentAlreadyActiveForCategory = 4,
        
        /// <summary>
        /// >> TournamentNotFound
        /// Cannot find tournament data for the selected (category, tournament)
        /// identifier combination.
        /// </summary>
        TournamentNotFound = 5,
        
        /// <summary>
        /// >> TournamentActivationTooEarly
        /// Cannot activate a tournament before its configured block start,
        /// </summary>
        TournamentActivationTooEarly = 6,
        
        /// <summary>
        /// >> TournamentEndingTooEarly
        /// Cannot deactivate a tournament before its configured block end,
        /// </summary>
        TournamentEndingTooEarly = 7,
        
        /// <summary>
        /// >> FailedToRankEntity
        /// An error occurred trying to rank an entity,
        /// </summary>
        FailedToRankEntity = 8,
        
        /// <summary>
        /// >> InvalidTournamentConfig
        /// Tournament configuration is invalid.
        /// </summary>
        InvalidTournamentConfig = 9,
        
        /// <summary>
        /// >> CannotScheduleTournament
        /// Tournament schedule already in use by another tournament.
        /// </summary>
        CannotScheduleTournament = 10,
        
        /// <summary>
        /// >> RankingCandidateNotInWinnerTable
        /// A ranking duck candidate proposed by an account is not in the winner's table.
        /// </summary>
        RankingCandidateNotInWinnerTable = 11,
        
        /// <summary>
        /// >> GoldenDuckCandidateNotWinner
        /// A golden duck candidate proposed by an account is not the actual golden duck winner.
        /// </summary>
        GoldenDuckCandidateNotWinner = 12,
        
        /// <summary>
        /// >> TournamentRewardAlreadyClaimed
        /// The reward for this tournament has already been claimed
        /// </summary>
        TournamentRewardAlreadyClaimed = 13,
    }
    
    /// <summary>
    /// >> 369 - Variant[pallet_ajuna_tournament.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
