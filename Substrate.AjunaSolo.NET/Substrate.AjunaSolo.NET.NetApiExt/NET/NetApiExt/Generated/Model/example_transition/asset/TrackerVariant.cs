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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset
{
    
    
    /// <summary>
    /// >> 358 - Composite[example_transition.asset.TrackerVariant]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class TrackerVariant : BaseType
    {
        
        /// <summary>
        /// >> slot_a_result
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 SlotAResult { get; set; }
        /// <summary>
        /// >> slot_b_result
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 SlotBResult { get; set; }
        /// <summary>
        /// >> slot_c_result
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 SlotCResult { get; set; }
        /// <summary>
        /// >> slot_d_result
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U16 SlotDResult { get; set; }
        /// <summary>
        /// >> last_reward
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 LastReward { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "TrackerVariant";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(SlotAResult.Encode());
            result.AddRange(SlotBResult.Encode());
            result.AddRange(SlotCResult.Encode());
            result.AddRange(SlotDResult.Encode());
            result.AddRange(LastReward.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            SlotAResult = new Substrate.NetApi.Model.Types.Primitive.U16();
            SlotAResult.Decode(byteArray, ref p);
            SlotBResult = new Substrate.NetApi.Model.Types.Primitive.U16();
            SlotBResult.Decode(byteArray, ref p);
            SlotCResult = new Substrate.NetApi.Model.Types.Primitive.U16();
            SlotCResult.Decode(byteArray, ref p);
            SlotDResult = new Substrate.NetApi.Model.Types.Primitive.U16();
            SlotDResult.Decode(byteArray, ref p);
            LastReward = new Substrate.NetApi.Model.Types.Primitive.U32();
            LastReward.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
