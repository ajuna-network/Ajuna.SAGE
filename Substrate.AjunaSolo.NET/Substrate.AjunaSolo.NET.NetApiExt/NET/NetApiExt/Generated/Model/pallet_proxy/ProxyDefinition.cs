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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_proxy
{
    
    
    /// <summary>
    /// >> 325 - Composite[pallet_proxy.ProxyDefinition]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class ProxyDefinition : BaseType
    {
        
        /// <summary>
        /// >> delegate
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 Delegate { get; set; }
        /// <summary>
        /// >> proxy_type
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.types.proxy.EnumProxyType ProxyType { get; set; }
        /// <summary>
        /// >> delay
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Delay { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "ProxyDefinition";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Delegate.Encode());
            result.AddRange(ProxyType.Encode());
            result.AddRange(Delay.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Delegate = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
            Delegate.Decode(byteArray, ref p);
            ProxyType = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.types.proxy.EnumProxyType();
            ProxyType.Decode(byteArray, ref p);
            Delay = new Substrate.NetApi.Model.Types.Primitive.U32();
            Delay.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
