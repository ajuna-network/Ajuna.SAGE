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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_support.dispatch
{
    
    
    /// <summary>
    /// >> 9 - Composite[frame_support.dispatch.PerDispatchClassT1]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class PerDispatchClassT1 : BaseType
    {
        
        /// <summary>
        /// >> normal
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight Normal { get; set; }
        /// <summary>
        /// >> operational
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight Operational { get; set; }
        /// <summary>
        /// >> mandatory
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight Mandatory { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "PerDispatchClassT1";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Normal.Encode());
            result.AddRange(Operational.Encode());
            result.AddRange(Mandatory.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Normal = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight();
            Normal.Decode(byteArray, ref p);
            Operational = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight();
            Operational.Decode(byteArray, ref p);
            Mandatory = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight();
            Mandatory.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
