//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Substrate.AjunaSolo.NET.RestClient.Generated.Interfaces
{
   using System;
   using System.Threading.Tasks;
   using Substrate.NetApi.Model.Types.Primitive;
   
   public interface ITimestampControllerClient
   {
      Task<U64> GetNow();
      Task<bool> SubscribeNow();
      Task<Bool> GetDidUpdate();
      Task<bool> SubscribeDidUpdate();
   }
}
