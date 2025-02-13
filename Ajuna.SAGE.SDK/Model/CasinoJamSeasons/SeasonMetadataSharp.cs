using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_seasons.types;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Text;
using System.Xml.Linq;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSeasons
{
    public class SeasonMetadataSharp
    {
        /// <summary>
        /// SeasonMetadataSharp constructor
        /// </summary>
        /// <param name="seasonConfig"></param>
        public SeasonMetadataSharp(SeasonMetadata seasonMetadata)
        {
            Name = seasonMetadata.Name.Value.ToText();
            Description = seasonMetadata.Description.Value.ToText();
        }

        // Parameterless constructor for deserialization
        public SeasonMetadataSharp() { }

        /// <summary>
        /// To substrate
        /// </summary>
        /// <returns></returns>
        public SeasonMetadata ToSubstrate()
        {
            var result = new SeasonMetadata
            {
                Name = new BoundedVecT2
                {
                    Value = new BaseVec<U8>(Encoding.UTF8.GetBytes(Name).ToU8Array())
                },
                Description = new BoundedVecT3
                {
                    Value = new BaseVec<U8>(Encoding.UTF8.GetBytes(Description).ToU8Array())
                }
            };
            return result;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; set; }
    }
}