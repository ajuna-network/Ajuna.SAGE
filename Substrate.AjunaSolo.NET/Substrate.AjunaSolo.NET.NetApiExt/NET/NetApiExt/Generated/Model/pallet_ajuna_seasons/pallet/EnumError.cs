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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_seasons.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> NoActiveSeason
        /// There is currently no active season
        /// </summary>
        NoActiveSeason = 0,
        
        /// <summary>
        /// >> CannotScheduleSeasonWithoutConfig
        /// Cannot set season schedule without season config first.
        /// </summary>
        CannotScheduleSeasonWithoutConfig = 1,
        
        /// <summary>
        /// >> CannotScheduleSeasonIfPreviousSeasonIsInfinite
        /// The previous season has no end, making it so that no
        /// new seasons can be added after it.
        /// </summary>
        CannotScheduleSeasonIfPreviousSeasonIsInfinite = 2,
        
        /// <summary>
        /// >> CannotScheduleInfiniteSeasonIfNextSeasonExists
        /// Cannot modify a season to be infinite if a season after it has already
        /// been scheduled.
        /// </summary>
        CannotScheduleInfiniteSeasonIfNextSeasonExists = 3,
        
        /// <summary>
        /// >> SeasonStartBeforeCurrentBlock
        /// The season's early start is before the current block.
        /// </summary>
        SeasonStartBeforeCurrentBlock = 4,
        
        /// <summary>
        /// >> SeasonStartOverlapsPreviousSeason
        /// The season starts before the previous season starts.
        /// </summary>
        SeasonStartOverlapsPreviousSeason = 5,
        
        /// <summary>
        /// >> SeasonStartOverlapsNextSeason
        /// The season starts after the next season starts.
        /// </summary>
        SeasonStartOverlapsNextSeason = 6,
        
        /// <summary>
        /// >> SeasonStartBeforeEarlyStart
        /// The season's early start is earlier than its normal start.
        /// </summary>
        SeasonStartBeforeEarlyStart = 7,
        
        /// <summary>
        /// >> SeasonEndBeforeStart
        /// The season's start block is greater than its end block.
        /// </summary>
        SeasonEndBeforeStart = 8,
        
        /// <summary>
        /// >> AssetNotRegistered
        /// The given asset was not registered in any season.
        /// </summary>
        AssetNotRegistered = 9,
        
        /// <summary>
        /// >> InvalidSeason
        /// The given season identifier has not been registered.
        /// </summary>
        InvalidSeason = 10,
        
        /// <summary>
        /// >> ScheduleSlotAlreadyInUse
        /// The given season schedule update clashed with another season's schedule.
        /// </summary>
        ScheduleSlotAlreadyInUse = 11,
    }
    
    /// <summary>
    /// >> 372 - Variant[pallet_ajuna_seasons.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
