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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types
{
    
    
    /// <summary>
    /// >> ExistenceReason
    /// </summary>
    public enum ExistenceReason
    {
        
        /// <summary>
        /// >> Consumer
        /// </summary>
        Consumer = 0,
        
        /// <summary>
        /// >> Sufficient
        /// </summary>
        Sufficient = 1,
        
        /// <summary>
        /// >> DepositHeld
        /// </summary>
        DepositHeld = 2,
        
        /// <summary>
        /// >> DepositRefunded
        /// </summary>
        DepositRefunded = 3,
        
        /// <summary>
        /// >> DepositFrom
        /// </summary>
        DepositFrom = 4,
    }
    
    /// <summary>
    /// >> 168 - Variant[pallet_assets.types.ExistenceReason]
    /// </summary>
    public sealed class EnumExistenceReason : BaseEnumRust<ExistenceReason>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumExistenceReason()
        {
				AddTypeDecoder<BaseVoid>(ExistenceReason.Consumer);
				AddTypeDecoder<BaseVoid>(ExistenceReason.Sufficient);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U128>(ExistenceReason.DepositHeld);
				AddTypeDecoder<BaseVoid>(ExistenceReason.DepositRefunded);
				AddTypeDecoder<BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.NetApi.Model.Types.Primitive.U128>>(ExistenceReason.DepositFrom);
        }
    }
}
