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
    /// >> GoldenDuckConfig
    /// </summary>
    public enum GoldenDuckConfig
    {
        
        /// <summary>
        /// >> Disabled
        /// </summary>
        Disabled = 0,
        
        /// <summary>
        /// >> Enabled
        /// </summary>
        Enabled = 1,
    }
    
    /// <summary>
    /// >> 264 - Variant[pallet_ajuna_tournament.config.GoldenDuckConfig]
    /// </summary>
    public sealed class EnumGoldenDuckConfig : BaseEnumRust<GoldenDuckConfig>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumGoldenDuckConfig()
        {
				AddTypeDecoder<BaseVoid>(GoldenDuckConfig.Disabled);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U8>(GoldenDuckConfig.Enabled);
        }
    }
}
