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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_payment_handler.withdraw_credit
{
    
    
    /// <summary>
    /// >> WithdrawKind
    /// </summary>
    public enum WithdrawKind
    {
        
        /// <summary>
        /// >> Payment
        /// </summary>
        Payment = 0,
        
        /// <summary>
        /// >> Voucher
        /// </summary>
        Voucher = 1,
    }
    
    /// <summary>
    /// >> 268 - Variant[ajuna_payment_handler.withdraw_credit.WithdrawKind]
    /// </summary>
    public sealed class EnumWithdrawKind : BaseEnumRust<WithdrawKind>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumWithdrawKind()
        {
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_support.traits.tokens.fungible.union_of.EnumNativeOrWithId>(WithdrawKind.Payment);
				AddTypeDecoder<BaseVoid>(WithdrawKind.Voucher);
        }
    }
}
