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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_affiliates.traits
{
    
    
    /// <summary>
    /// >> 349 - Composite[pallet_ajuna_affiliates.traits.AffiliatorState]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AffiliatorState : BaseType
    {
        
        /// <summary>
        /// >> status
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_affiliates.traits.EnumAffiliatableStatus Status { get; set; }
        /// <summary>
        /// >> affiliates
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Affiliates { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "AffiliatorState";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Status.Encode());
            result.AddRange(Affiliates.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Status = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_affiliates.traits.EnumAffiliatableStatus();
            Status.Decode(byteArray, ref p);
            Affiliates = new Substrate.NetApi.Model.Types.Primitive.U32();
            Affiliates.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
