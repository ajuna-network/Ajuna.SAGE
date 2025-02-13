using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.season_manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSeasons
{
    /// <summary>
    /// SeasonFeeConfigSharp
    /// </summary>
    public class SeasonConfigSharp
    {
        /// <summary>
        /// SeasonConfigSharp constructor
        /// </summary>
        /// <param name="seasonConfig"></param>
        public SeasonConfigSharp(SeasonConfig seasonConfig)
        {
            Fee = new SeasonFeeConfigSharp(seasonConfig.Fee);
        }

        // Parameterless constructor for deserialization
        public SeasonConfigSharp() { }

        /// <summary>
        /// To substrate
        /// </summary>
        /// <returns></returns>
        public SeasonConfig ToSubstrate()
        {
            var result = new SeasonConfig();
            result.Fee = Fee.ToSubstrate();
            return result;
        }

        /// <summary>
        /// Fee
        /// </summary>
        public SeasonFeeConfigSharp Fee { get; set; }
    }
}
