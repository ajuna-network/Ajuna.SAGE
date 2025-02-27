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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types
{
    
    
    /// <summary>
    /// >> 173 - Composite[pallet_assets.types.AssetMetadata]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class AssetMetadata : BaseType
    {
        
        /// <summary>
        /// >> deposit
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 Deposit { get; set; }
        /// <summary>
        /// >> name
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 Name { get; set; }
        /// <summary>
        /// >> symbol
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 Symbol { get; set; }
        /// <summary>
        /// >> decimals
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 Decimals { get; set; }
        /// <summary>
        /// >> is_frozen
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.Bool IsFrozen { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "AssetMetadata";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Deposit.Encode());
            result.AddRange(Name.Encode());
            result.AddRange(Symbol.Encode());
            result.AddRange(Decimals.Encode());
            result.AddRange(IsFrozen.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Deposit = new Substrate.NetApi.Model.Types.Primitive.U128();
            Deposit.Decode(byteArray, ref p);
            Name = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
            Name.Decode(byteArray, ref p);
            Symbol = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
            Symbol.Decode(byteArray, ref p);
            Decimals = new Substrate.NetApi.Model.Types.Primitive.U8();
            Decimals.Decode(byteArray, ref p);
            IsFrozen = new Substrate.NetApi.Model.Types.Primitive.Bool();
            IsFrozen.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
