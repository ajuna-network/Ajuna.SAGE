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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager
{
    
    
    /// <summary>
    /// >> 99 - Composite[ajuna_primitives.asset_manager.Lock]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class Lock : BaseType
    {
        
        /// <summary>
        /// >> id
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr8U8 Id { get; set; }
        /// <summary>
        /// >> locker
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 Locker { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "Lock";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Id.Encode());
            result.AddRange(Locker.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Id = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr8U8();
            Id.Decode(byteArray, ref p);
            Locker = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
            Locker.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
