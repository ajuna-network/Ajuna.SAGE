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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config
{
    
    
    /// <summary>
    /// >> AssetFilter
    /// </summary>
    public enum AssetFilter
    {
        
        /// <summary>
        /// >> Trade
        /// </summary>
        Trade = 0,
        
        /// <summary>
        /// >> Transfer
        /// </summary>
        Transfer = 1,
    }
    
    /// <summary>
    /// >> 268 - Variant[pallet_sage.config.AssetFilter]
    /// </summary>
    public sealed class EnumAssetFilter : BaseEnumRust<AssetFilter>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumAssetFilter()
        {
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType>(AssetFilter.Trade);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType>(AssetFilter.Transfer);
        }
    }
}
