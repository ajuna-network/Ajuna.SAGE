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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset
{
    
    
    /// <summary>
    /// >> PlayerVariant
    /// </summary>
    public enum PlayerVariant
    {
        
        /// <summary>
        /// >> Human
        /// </summary>
        Human = 0,
        
        /// <summary>
        /// >> Tracker
        /// </summary>
        Tracker = 1,
    }
    
    /// <summary>
    /// >> 356 - Variant[example_transition.asset.PlayerVariant]
    /// </summary>
    public sealed class EnumPlayerVariant : BaseEnumRust<PlayerVariant>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumPlayerVariant()
        {
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.HumanVariant>(PlayerVariant.Human);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.TrackerVariant>(PlayerVariant.Tracker);
        }
    }
}
