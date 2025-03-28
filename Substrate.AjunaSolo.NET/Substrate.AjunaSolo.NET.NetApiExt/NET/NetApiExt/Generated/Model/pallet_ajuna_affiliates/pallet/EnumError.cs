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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_affiliates.pallet
{
    
    
    /// <summary>
    /// >> Error
    /// The `Error` enum of this pallet.
    /// </summary>
    public enum Error
    {
        
        /// <summary>
        /// >> CannotAffiliateSelf
        /// An account cannot affiliate itself
        /// </summary>
        CannotAffiliateSelf = 0,
        
        /// <summary>
        /// >> TargetAccountIsNotAffiliatable
        /// The account is not allowed to receive affiliates
        /// </summary>
        TargetAccountIsNotAffiliatable = 1,
        
        /// <summary>
        /// >> AffiliateOthersOnlyWhiteListed
        /// Only whitelisted accounts can affiliate for others
        /// </summary>
        AffiliateOthersOnlyWhiteListed = 2,
        
        /// <summary>
        /// >> AffiliatorNotFound
        /// No account matches the provided affiliator identifier
        /// </summary>
        AffiliatorNotFound = 3,
        
        /// <summary>
        /// >> CannotAffiliateMoreAccounts
        /// This account has reached the affiliate limit
        /// </summary>
        CannotAffiliateMoreAccounts = 4,
        
        /// <summary>
        /// >> CannotAffiliateAlreadyAffiliatedAccount
        /// This account has already been affiliated by another affiliator
        /// </summary>
        CannotAffiliateAlreadyAffiliatedAccount = 5,
        
        /// <summary>
        /// >> CannotAffiliateToExistingAffiliator
        /// This account is already an affiliator, so it cannot affiliate to another account
        /// </summary>
        CannotAffiliateToExistingAffiliator = 6,
        
        /// <summary>
        /// >> CannotAffiliateBlocked
        /// The account is blocked, so it cannot be affiliated to
        /// </summary>
        CannotAffiliateBlocked = 7,
        
        /// <summary>
        /// >> ExtrinsicAlreadyHasRule
        /// The given extrinsic identifier is already paired with an affiliate rule
        /// </summary>
        ExtrinsicAlreadyHasRule = 8,
        
        /// <summary>
        /// >> ExtrinsicHasNoRule
        /// The given extrinsic identifier is not associated with any rule
        /// </summary>
        ExtrinsicHasNoRule = 9,
    }
    
    /// <summary>
    /// >> 351 - Variant[pallet_ajuna_affiliates.pallet.Error]
    /// The `Error` enum of this pallet.
    /// </summary>
    public sealed class EnumError : BaseEnum<Error>
    {
    }
}
