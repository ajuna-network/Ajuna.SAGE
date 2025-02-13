using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_seasons.types;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.IO;
using System.Text;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSeasons
{
    /// <summary>
    /// SeasonScheduleSharp
    /// </summary>
    public class SeasonScheduleSharp
    {
        /// <summary>
        /// SeasonScheduleSharp constructor
        /// </summary>
        /// <param name="seasonConfig"></param>
        public SeasonScheduleSharp(SeasonSchedule seasonSchedule)
        {
            EarlyStart = seasonSchedule.EarlyStart.Value;
            Start = seasonSchedule.Start.Value;
            End = seasonSchedule.End.OptionFlag ? seasonSchedule.End.Value : null;
        }

        // Parameterless constructor for deserialization
        public SeasonScheduleSharp() { }

        /// <summary>
        /// To substrate
        /// </summary>
        /// <returns></returns>
        public SeasonSchedule ToSubstrate()
        {
            var end = new BaseOpt<U32>();
            if (End.HasValue)
            {
                end.Create(new U32(End.Value));
            }
            var result = new SeasonSchedule
            {
                EarlyStart = new U32(EarlyStart),
                Start = new U32(Start),
                End = end
            };
            return result;
        }

        /// <summary>
        /// EarlyStart
        /// </summary>
        public uint EarlyStart { get; set; }

        /// <summary>
        /// Start
        /// </summary>
        public uint Start { get; set; }

        /// <summary>
        /// End
        /// </summary>
        public uint? End { get; set; }
    }
}