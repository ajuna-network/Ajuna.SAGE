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
    /// >> AssetVariant
    /// </summary>
    public enum AssetVariant
    {
        
        /// <summary>
        /// >> Player
        /// </summary>
        Player = 0,
        
        /// <summary>
        /// >> Machine
        /// </summary>
        Machine = 1,
        
        /// <summary>
        /// >> Seat
        /// </summary>
        Seat = 2,
    }
    
    /// <summary>
    /// >> 357 - Variant[example_transition.asset.AssetVariant]
    /// </summary>
    public sealed class EnumAssetVariant : BaseEnumRust<AssetVariant>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumAssetVariant()
        {
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumPlayerVariant>(AssetVariant.Player);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.MachineVariant>(AssetVariant.Machine);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.SeatVariant>(AssetVariant.Seat);
        }
    }
}
