//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.Base;
using System.Collections.Generic;


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_treasury
{
    
    
    /// <summary>
    /// >> 278 - Composite[pallet_treasury.SpendStatus]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SpendStatus : BaseType
    {
        
        /// <summary>
        /// >> asset_kind
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseTuple AssetKind { get; set; }
        /// <summary>
        /// >> amount
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 Amount { get; set; }
        /// <summary>
        /// >> beneficiary
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 Beneficiary { get; set; }
        /// <summary>
        /// >> valid_from
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ValidFrom { get; set; }
        /// <summary>
        /// >> expire_at
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ExpireAt { get; set; }
        /// <summary>
        /// >> status
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_treasury.EnumPaymentState Status { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SpendStatus";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(AssetKind.Encode());
            result.AddRange(Amount.Encode());
            result.AddRange(Beneficiary.Encode());
            result.AddRange(ValidFrom.Encode());
            result.AddRange(ExpireAt.Encode());
            result.AddRange(Status.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            AssetKind = new Substrate.NetApi.Model.Types.Base.BaseTuple();
            AssetKind.Decode(byteArray, ref p);
            Amount = new Substrate.NetApi.Model.Types.Primitive.U128();
            Amount.Decode(byteArray, ref p);
            Beneficiary = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
            Beneficiary.Decode(byteArray, ref p);
            ValidFrom = new Substrate.NetApi.Model.Types.Primitive.U32();
            ValidFrom.Decode(byteArray, ref p);
            ExpireAt = new Substrate.NetApi.Model.Types.Primitive.U32();
            ExpireAt.Decode(byteArray, ref p);
            Status = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_treasury.EnumPaymentState();
            Status.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
