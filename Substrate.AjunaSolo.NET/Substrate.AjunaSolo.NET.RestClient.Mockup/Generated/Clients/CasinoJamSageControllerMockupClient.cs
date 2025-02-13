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
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player;
   using Substrate.NetApi.Model.Types.Base;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
   using Substrate.NetApi.Model.Types.Primitive;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager;
   using Substrate.AjunaSolo.NET.RestClient.Mockup.Generated.Interfaces;
   
   public sealed class CasinoJamSageControllerMockupClient : MockupBaseClient, ICasinoJamSageControllerMockupClient
   {
      private HttpClient _httpClient;
      public CasinoJamSageControllerMockupClient(HttpClient httpClient)
      {
         _httpClient = httpClient;
      }
      public async Task<bool> SetOrganizer(AccountId32 value)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/Organizer", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.OrganizerParams());
      }
      public async Task<bool> SetGeneralConfigStore(GeneralConfig value)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/GeneralConfigStore", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.GeneralConfigStoreParams());
      }
      public async Task<bool> SetTransitionConfigStore(CasinoJamTransitionConfig value)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/TransitionConfigStore", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.TransitionConfigStoreParams());
      }
      public async Task<bool> SetSeasonUnlocks(Arr5U8 value, BaseTuple<U32, EnumLockableFeature> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/SeasonUnlocks", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.SeasonUnlocksParams(key));
      }
      public async Task<bool> SetPlayerSeasonConfigs(PlayerConfig value, BaseTuple<AccountId32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/PlayerSeasonConfigs", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.PlayerSeasonConfigsParams(key));
      }
      public async Task<bool> SetPlayerSeasonStats(PlayerStats value, BaseTuple<AccountId32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/PlayerSeasonStats", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.PlayerSeasonStatsParams(key));
      }
      public async Task<bool> SetAssets(BaseTuple<AccountId32, Asset> value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/Assets", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.AssetsParams(key));
      }
      public async Task<bool> SetAssetOwners(BaseTuple value, BaseTuple<AccountId32, U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/AssetOwners", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.AssetOwnersParams(key));
      }
      public async Task<bool> SetAssetsOwnedCount(U8 value, BaseTuple<AccountId32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/AssetsOwnedCount", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.AssetsOwnedCountParams(key));
      }
      public async Task<bool> SetSeasonTradeFilters(EnumVariantType value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/SeasonTradeFilters", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.SeasonTradeFiltersParams(key));
      }
      public async Task<bool> SetSeasonTransferFilters(EnumVariantType value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/SeasonTransferFilters", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.SeasonTransferFiltersParams(key));
      }
      public async Task<bool> SetAssetTradePrices(U128 value, BaseTuple<U32, U32> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/AssetTradePrices", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.AssetTradePricesParams(key));
      }
      public async Task<bool> SetLockedAssets(Lock value, U32 key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/LockedAssets", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.LockedAssetsParams(key));
      }
      public async Task<bool> SetAssetFunds(U128 value, BaseTuple<U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_payment_handler.withdraw_credit.EnumWithdrawKind> key)
      {
         return await SendMockupRequestAsync(_httpClient, "CasinoJamSage/AssetFunds", value.Encode(), Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage.CasinoJamSageStorage.AssetFundsParams(key));
      }
   }
}
