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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.frame_support.traits.tokens.fungible.union_of
{
    
    
    /// <summary>
    /// >> NativeOrWithId
    /// </summary>
    public enum NativeOrWithId
    {
        
        /// <summary>
        /// >> Native
        /// </summary>
        Native = 0,
        
        /// <summary>
        /// >> WithId
        /// </summary>
        WithId = 1,
    }
    
    /// <summary>
    /// >> 269 - Variant[frame_support.traits.tokens.fungible.union_of.NativeOrWithId]
    /// </summary>
    public sealed class EnumNativeOrWithId : BaseEnumRust<NativeOrWithId>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumNativeOrWithId()
        {
				AddTypeDecoder<BaseVoid>(NativeOrWithId.Native);
				AddTypeDecoder<Substrate.NetApi.Model.Types.Primitive.U32>(NativeOrWithId.WithId);
        }
    }
}
