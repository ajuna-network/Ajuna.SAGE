//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Substrate.AjunaSolo.NET.RestClient.Mockup.Generated.Clients
{
   using System;
   using System.Threading.Tasks;
   using System.Net.Http;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;
   using Substrate.NetApi.Model.Types.Primitive;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
   using Substrate.AjunaSolo.NET.RestClient.Mockup.Generated.Interfaces;
   
   public sealed class CasinoJamTournamentControllerMockupClient : MockupBaseClient, ICasinoJamTournamentControllerMockupClient
   {
      private HttpClient _httpClient;
      public CasinoJamTournamentControllerMockupClient(HttpClient httpClient)
      {
         _httpClient = httpClient;
      }
      public async Task<bool> SetTournamentSchedules(EnumTournamentScheduledAction value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/TournamentSchedules", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.TournamentSchedulesParams(key));
      }
      public async Task<bool> SetTreasuryAccountsCache(AccountId32 value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/TreasuryAccountsCache", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.TreasuryAccountsCacheParams(key));
      }
      public async Task<bool> SetNextTournamentIds(U32 value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/NextTournamentIds", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.NextTournamentIdsParams(key));
      }
      public async Task<bool> SetTournaments(TournamentConfig value, Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/Tournaments", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.TournamentsParams(key));
      }
      public async Task<bool> SetActiveTournaments(EnumTournamentState value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/ActiveTournaments", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.ActiveTournamentsParams(key));
      }
      public async Task<bool> SetTournamentRankings(BoundedVecT31 value, Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/TournamentRankings", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.TournamentRankingsParams(key));
      }
      public async Task<bool> SetTournamentRewardClaims(EnumRewardClaimState value, Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/TournamentRewardClaims", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.TournamentRewardClaimsParams(key));
      }
      public async Task<bool> SetGoldenDucks(EnumGoldenDuckState value, Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/GoldenDucks", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.GoldenDucksParams(key));
      }
      public async Task<bool> SetGoldenDuckRewardClaims(EnumRewardClaimState value, Substrate.NetApi.Model.Types.Base.BaseTuple<U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamTournament/GoldenDuckRewardClaims", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamTournamentStorage.GoldenDuckRewardClaimsParams(key));
      }
   }
}
