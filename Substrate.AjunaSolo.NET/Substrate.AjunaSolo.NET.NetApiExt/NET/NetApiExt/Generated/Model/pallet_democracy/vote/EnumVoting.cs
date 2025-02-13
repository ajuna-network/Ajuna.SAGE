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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.vote
{
    
    
    /// <summary>
    /// >> Voting
    /// </summary>
    public enum Voting
    {
        
        /// <summary>
        /// >> Direct
        /// </summary>
        Direct = 0,
        
        /// <summary>
        /// >> Delegating
        /// </summary>
        Delegating = 1,
    }
    
    /// <summary>
    /// >> 289 - Variant[pallet_democracy.vote.Voting]
    /// </summary>
    public sealed class EnumVoting : BaseEnumRust<Voting>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumVoting()
        {
				AddTypeDecoder<BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT20, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.types.Delegations, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.vote.PriorLock>>(Voting.Direct);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Primitive.U128, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.conviction.EnumConviction, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.types.Delegations, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_democracy.vote.PriorLock>>(Voting.Delegating);
        }
    }
}
