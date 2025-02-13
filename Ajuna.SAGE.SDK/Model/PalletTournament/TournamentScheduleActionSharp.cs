using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Wrapped TournamentScheduleActionSharp
    /// </summary>
    public class TournamentScheduleActionSharp
    {
        /// <summary>
        /// TournamentScheduleActionSharp constructor
        /// </summary>
        /// <param name="tournamentScheduledAction"></param>
        public TournamentScheduleActionSharp(EnumTournamentScheduledAction tournamentScheduledAction)
        {
            TournamentScheduledAction = tournamentScheduledAction.Value;
            ScheduledAction = (U16)((BaseTuple<U16, U32>)tournamentScheduledAction.Value2).Value[0];
            BlockNumber = (U32)((BaseTuple<U16, U32>)tournamentScheduledAction.Value2).Value[1];
        }

        /// <summary>
        /// TournamentScheduledAction
        /// </summary>
        public TournamentScheduledAction TournamentScheduledAction { get; }

        /// <summary>
        /// ScheduledAction
        /// </summary>
        public U16 ScheduledAction { get; }

        /// <summary>
        /// BlockNumber
        /// </summary>
        public U32 BlockNumber { get; }
    }
}