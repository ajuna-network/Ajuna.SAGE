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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.affiliates
{
    
    
    /// <summary>
    /// >> AffiliateMethods
    /// </summary>
    public enum AffiliateMethods
    {
        
        /// <summary>
        /// >> UpgradeAssetInventory
        /// </summary>
        UpgradeAssetInventory = 0,
        
        /// <summary>
        /// >> TradeAsset
        /// </summary>
        TradeAsset = 1,
        
        /// <summary>
        /// >> StateTransition
        /// </summary>
        StateTransition = 2,
    }
    
    /// <summary>
    /// >> 69 - Variant[pallet_sage.config.affiliates.AffiliateMethods]
    /// </summary>
    public sealed class EnumAffiliateMethods : BaseEnumRust<AffiliateMethods>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumAffiliateMethods()
        {
				AddTypeDecoder<BaseVoid>(AffiliateMethods.UpgradeAssetInventory);
				AddTypeDecoder<BaseVoid>(AffiliateMethods.TradeAsset);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.EnumCasinoAction>(AffiliateMethods.StateTransition);
        }
    }
}
