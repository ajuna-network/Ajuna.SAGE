//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Substrate.AjunaSolo.NET.RestClient.Generated.Clients
{
   using System;
   using System.Threading.Tasks;
   using System.Net.Http;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_multisig;
   using Substrate.AjunaSolo.NET.RestClient.Generated.Interfaces;
   
   public sealed class MultisigControllerClient : BaseClient, IMultisigControllerClient
   {
      private HttpClient _httpClient;
      private BaseSubscriptionClient _subscriptionClient;
      public MultisigControllerClient(HttpClient httpClient, BaseSubscriptionClient subscriptionClient)
      {
         _httpClient = httpClient;
         _subscriptionClient = subscriptionClient;
      }
      public async Task<Multisig> GetMultisigs(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8> key)
      {
         return await SendRequestAsync<Multisig>(_httpClient, "multisig/multisigs", Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.MultisigStorage.MultisigsParams(key));
      }
      public async Task<bool> SubscribeMultisigs(Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8> key)
      {
         return await _subscriptionClient.SubscribeAsync("Multisig.Multisigs", Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.MultisigStorage.MultisigsParams(key));
      }
   }
}
