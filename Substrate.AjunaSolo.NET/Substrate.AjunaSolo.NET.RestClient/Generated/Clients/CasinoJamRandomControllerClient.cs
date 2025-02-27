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
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
   using Substrate.AjunaSolo.NET.RestClient.Generated.Interfaces;
   
   public sealed class CasinoJamRandomControllerClient : BaseClient, ICasinoJamRandomControllerClient
   {
      private HttpClient _httpClient;
      private BaseSubscriptionClient _subscriptionClient;
      public CasinoJamRandomControllerClient(HttpClient httpClient, BaseSubscriptionClient subscriptionClient)
      {
         _httpClient = httpClient;
         _subscriptionClient = subscriptionClient;
      }
      public async Task<BoundedVecT32> GetRandomMaterial()
      {
         return await SendRequestAsync<BoundedVecT32>(_httpClient, "casinojamrandom/randommaterial");
      }
      public async Task<bool> SubscribeRandomMaterial()
      {
         return await _subscriptionClient.SubscribeAsync("CasinoJamRandom.RandomMaterial");
      }
   }
}
