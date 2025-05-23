//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using Substrate.ServiceLayer.Attributes;
using Substrate.ServiceLayer.Storage;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Substrate.AjunaSolo.NET.RestService.Generated.Storage
{
    
    
    /// <summary>
    /// ICasinoJamSageStorage interface definition.
    /// </summary>
    public interface ICasinoJamSageStorage : IStorage
    {
        
        /// <summary>
        /// >> Organizer
        ///  Organizer of the game. Essentially the administrator with certain privileges.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetOrganizer();
        
        /// <summary>
        /// >> GeneralConfigStore
        ///  Tracks global configuration values that can be changed by the organizer only.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.GeneralConfig GetGeneralConfigStore();
        
        /// <summary>
        /// >> TransitionConfigStore
        ///  Configuration values specific to the transition being used.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.CasinoJamTransitionConfig GetTransitionConfigStore();
        
        /// <summary>
        /// >> SeasonUnlocks
        ///  Some features need to be unlocked fulfilling certain criteria.
        /// 
        ///  This storage keeps track of the `UnlockRule` that needs to be satisfied to unlock the
        ///  feature. If there is no unlock rule, the feature can't be unlocked in that season.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8 GetSeasonUnlocks(string key);
        
        /// <summary>
        /// >> PlayerSeasonConfigs
        ///  Tracks player configs per season. This can be mutated by unlocking certain privileges, e.g.
        ///  upgrading the storage inventory size.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig GetPlayerSeasonConfigs(string key);
        
        /// <summary>
        /// >> PlayerSeasonStats
        ///  Tracks player stats per season.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats GetPlayerSeasonStats(string key);
        
        /// <summary>
        /// >> Assets
        ///  Maps the `AssetId` to its owner and the asset.
        /// </summary>
        Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset> GetAssets(string key);
        
        /// <summary>
        /// >> AssetOwners
        ///  Keeps track of the assets owned by an account and in which season the asset was created.
        /// 
        ///  We mostly do ownership checks on this in the runtime. Whereas the frontends want to display
        ///  a list. This has to be queried with a `state.getKeysPaged` followed by a `state.getStorage`
        ///  call. Maybe it makes sense to implement a runtime api call for this to reduce networking
        ///  bandwidth.
        /// </summary>
        Substrate.NetApi.Model.Types.Base.BaseTuple GetAssetOwners(string key);
        
        /// <summary>
        /// >> AssetsOwnedCount
        ///  Keeps track of how many assets an account owns.
        /// </summary>
        Substrate.NetApi.Model.Types.Primitive.U8 GetAssetsOwnedCount(string key);
        
        /// <summary>
        /// >> SeasonTradeFilters
        ///  A filter that assets need to pass in order to be traded.
        /// 
        ///  The filter can be changed by the organizer.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType GetSeasonTradeFilters(string key);
        
        /// <summary>
        /// >> SeasonTransferFilters
        ///  A filter that assets need to pass in order to be transfer.
        /// 
        ///  The filter can be changed by the organizer.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType GetSeasonTransferFilters(string key);
        
        /// <summary>
        /// >> AssetTradePrices
        ///  Tracks assets that have been put on the market with a certain price.
        /// </summary>
        Substrate.NetApi.Model.Types.Primitive.U128 GetAssetTradePrices(string key);
        
        /// <summary>
        /// >> LockedAssets
        ///  Tracks assets that have been locked either through the `lock_asset` extrinsic, or by
        ///  other pallets via this pallet's `AssetManager` implementation.
        /// 
        ///  A locked asset can't be transferred, traded, consumed or mutated.
        /// </summary>
        Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock GetLockedAssets(string key);
        
        /// <summary>
        /// >> AssetFunds
        ///  Tracks how many funds assets have, which will be returned to the owner, once the
        ///  asset is consumed
        /// </summary>
        Substrate.NetApi.Model.Types.Primitive.U128 GetAssetFunds(string key);
    }
    
    /// <summary>
    /// CasinoJamSageStorage class definition.
    /// </summary>
    public sealed class CasinoJamSageStorage : ICasinoJamSageStorage
    {
        
        /// <summary>
        /// _organizerTypedStorage typed storage field
        /// </summary>
        private TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> _organizerTypedStorage;
        
        /// <summary>
        /// _generalConfigStoreTypedStorage typed storage field
        /// </summary>
        private TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.GeneralConfig> _generalConfigStoreTypedStorage;
        
        /// <summary>
        /// _transitionConfigStoreTypedStorage typed storage field
        /// </summary>
        private TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.CasinoJamTransitionConfig> _transitionConfigStoreTypedStorage;
        
        /// <summary>
        /// _seasonUnlocksTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8> _seasonUnlocksTypedStorage;
        
        /// <summary>
        /// _playerSeasonConfigsTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig> _playerSeasonConfigsTypedStorage;
        
        /// <summary>
        /// _playerSeasonStatsTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats> _playerSeasonStatsTypedStorage;
        
        /// <summary>
        /// _assetsTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset>> _assetsTypedStorage;
        
        /// <summary>
        /// _assetOwnersTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple> _assetOwnersTypedStorage;
        
        /// <summary>
        /// _assetsOwnedCountTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U8> _assetsOwnedCountTypedStorage;
        
        /// <summary>
        /// _seasonTradeFiltersTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType> _seasonTradeFiltersTypedStorage;
        
        /// <summary>
        /// _seasonTransferFiltersTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType> _seasonTransferFiltersTypedStorage;
        
        /// <summary>
        /// _assetTradePricesTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128> _assetTradePricesTypedStorage;
        
        /// <summary>
        /// _lockedAssetsTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock> _lockedAssetsTypedStorage;
        
        /// <summary>
        /// _assetFundsTypedStorage typed storage field
        /// </summary>
        private TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128> _assetFundsTypedStorage;
        
        /// <summary>
        /// CasinoJamSageStorage constructor.
        /// </summary>
        public CasinoJamSageStorage(IStorageDataProvider storageDataProvider, List<IStorageChangeDelegate> storageChangeDelegates)
        {
            this.OrganizerTypedStorage = new TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>("CasinoJamSage.Organizer", storageDataProvider, storageChangeDelegates);
            this.GeneralConfigStoreTypedStorage = new TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.GeneralConfig>("CasinoJamSage.GeneralConfigStore", storageDataProvider, storageChangeDelegates);
            this.TransitionConfigStoreTypedStorage = new TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.CasinoJamTransitionConfig>("CasinoJamSage.TransitionConfigStore", storageDataProvider, storageChangeDelegates);
            this.SeasonUnlocksTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8>("CasinoJamSage.SeasonUnlocks", storageDataProvider, storageChangeDelegates);
            this.PlayerSeasonConfigsTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig>("CasinoJamSage.PlayerSeasonConfigs", storageDataProvider, storageChangeDelegates);
            this.PlayerSeasonStatsTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats>("CasinoJamSage.PlayerSeasonStats", storageDataProvider, storageChangeDelegates);
            this.AssetsTypedStorage = new TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset>>("CasinoJamSage.Assets", storageDataProvider, storageChangeDelegates);
            this.AssetOwnersTypedStorage = new TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple>("CasinoJamSage.AssetOwners", storageDataProvider, storageChangeDelegates);
            this.AssetsOwnedCountTypedStorage = new TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U8>("CasinoJamSage.AssetsOwnedCount", storageDataProvider, storageChangeDelegates);
            this.SeasonTradeFiltersTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType>("CasinoJamSage.SeasonTradeFilters", storageDataProvider, storageChangeDelegates);
            this.SeasonTransferFiltersTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType>("CasinoJamSage.SeasonTransferFilters", storageDataProvider, storageChangeDelegates);
            this.AssetTradePricesTypedStorage = new TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128>("CasinoJamSage.AssetTradePrices", storageDataProvider, storageChangeDelegates);
            this.LockedAssetsTypedStorage = new TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock>("CasinoJamSage.LockedAssets", storageDataProvider, storageChangeDelegates);
            this.AssetFundsTypedStorage = new TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128>("CasinoJamSage.AssetFunds", storageDataProvider, storageChangeDelegates);
        }
        
        /// <summary>
        /// _organizerTypedStorage property
        /// </summary>
        public TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> OrganizerTypedStorage
        {
            get
            {
                return _organizerTypedStorage;
            }
            set
            {
                _organizerTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _generalConfigStoreTypedStorage property
        /// </summary>
        public TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.GeneralConfig> GeneralConfigStoreTypedStorage
        {
            get
            {
                return _generalConfigStoreTypedStorage;
            }
            set
            {
                _generalConfigStoreTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _transitionConfigStoreTypedStorage property
        /// </summary>
        public TypedStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.CasinoJamTransitionConfig> TransitionConfigStoreTypedStorage
        {
            get
            {
                return _transitionConfigStoreTypedStorage;
            }
            set
            {
                _transitionConfigStoreTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _seasonUnlocksTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8> SeasonUnlocksTypedStorage
        {
            get
            {
                return _seasonUnlocksTypedStorage;
            }
            set
            {
                _seasonUnlocksTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _playerSeasonConfigsTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig> PlayerSeasonConfigsTypedStorage
        {
            get
            {
                return _playerSeasonConfigsTypedStorage;
            }
            set
            {
                _playerSeasonConfigsTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _playerSeasonStatsTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats> PlayerSeasonStatsTypedStorage
        {
            get
            {
                return _playerSeasonStatsTypedStorage;
            }
            set
            {
                _playerSeasonStatsTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _assetsTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset>> AssetsTypedStorage
        {
            get
            {
                return _assetsTypedStorage;
            }
            set
            {
                _assetsTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _assetOwnersTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.NetApi.Model.Types.Base.BaseTuple> AssetOwnersTypedStorage
        {
            get
            {
                return _assetOwnersTypedStorage;
            }
            set
            {
                _assetOwnersTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _assetsOwnedCountTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U8> AssetsOwnedCountTypedStorage
        {
            get
            {
                return _assetsOwnedCountTypedStorage;
            }
            set
            {
                _assetsOwnedCountTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _seasonTradeFiltersTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType> SeasonTradeFiltersTypedStorage
        {
            get
            {
                return _seasonTradeFiltersTypedStorage;
            }
            set
            {
                _seasonTradeFiltersTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _seasonTransferFiltersTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType> SeasonTransferFiltersTypedStorage
        {
            get
            {
                return _seasonTransferFiltersTypedStorage;
            }
            set
            {
                _seasonTransferFiltersTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _assetTradePricesTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128> AssetTradePricesTypedStorage
        {
            get
            {
                return _assetTradePricesTypedStorage;
            }
            set
            {
                _assetTradePricesTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _lockedAssetsTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock> LockedAssetsTypedStorage
        {
            get
            {
                return _lockedAssetsTypedStorage;
            }
            set
            {
                _lockedAssetsTypedStorage = value;
            }
        }
        
        /// <summary>
        /// _assetFundsTypedStorage property
        /// </summary>
        public TypedMapStorage<Substrate.NetApi.Model.Types.Primitive.U128> AssetFundsTypedStorage
        {
            get
            {
                return _assetFundsTypedStorage;
            }
            set
            {
                _assetFundsTypedStorage = value;
            }
        }
        
        /// <summary>
        /// Connects to all storages and initializes the change subscription handling.
        /// </summary>
        public async Task InitializeAsync(Substrate.ServiceLayer.Storage.IStorageDataProvider dataProvider)
        {
            await OrganizerTypedStorage.InitializeAsync("CasinoJamSage", "Organizer");
            await GeneralConfigStoreTypedStorage.InitializeAsync("CasinoJamSage", "GeneralConfigStore");
            await TransitionConfigStoreTypedStorage.InitializeAsync("CasinoJamSage", "TransitionConfigStore");
            await SeasonUnlocksTypedStorage.InitializeAsync("CasinoJamSage", "SeasonUnlocks");
            await PlayerSeasonConfigsTypedStorage.InitializeAsync("CasinoJamSage", "PlayerSeasonConfigs");
            await PlayerSeasonStatsTypedStorage.InitializeAsync("CasinoJamSage", "PlayerSeasonStats");
            await AssetsTypedStorage.InitializeAsync("CasinoJamSage", "Assets");
            await AssetOwnersTypedStorage.InitializeAsync("CasinoJamSage", "AssetOwners");
            await AssetsOwnedCountTypedStorage.InitializeAsync("CasinoJamSage", "AssetsOwnedCount");
            await SeasonTradeFiltersTypedStorage.InitializeAsync("CasinoJamSage", "SeasonTradeFilters");
            await SeasonTransferFiltersTypedStorage.InitializeAsync("CasinoJamSage", "SeasonTransferFilters");
            await AssetTradePricesTypedStorage.InitializeAsync("CasinoJamSage", "AssetTradePrices");
            await LockedAssetsTypedStorage.InitializeAsync("CasinoJamSage", "LockedAssets");
            await AssetFundsTypedStorage.InitializeAsync("CasinoJamSage", "AssetFunds");
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.Organizer
        /// </summary>
        [StorageChange("CasinoJamSage", "Organizer")]
        public void OnUpdateOrganizer(string data)
        {
            OrganizerTypedStorage.Update(data);
        }
        
        /// <summary>
        /// >> Organizer
        ///  Organizer of the game. Essentially the administrator with certain privileges.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetOrganizer()
        {
            return OrganizerTypedStorage.Get();
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.GeneralConfigStore
        /// </summary>
        [StorageChange("CasinoJamSage", "GeneralConfigStore")]
        public void OnUpdateGeneralConfigStore(string data)
        {
            GeneralConfigStoreTypedStorage.Update(data);
        }
        
        /// <summary>
        /// >> GeneralConfigStore
        ///  Tracks global configuration values that can be changed by the organizer only.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.GeneralConfig GetGeneralConfigStore()
        {
            return GeneralConfigStoreTypedStorage.Get();
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.TransitionConfigStore
        /// </summary>
        [StorageChange("CasinoJamSage", "TransitionConfigStore")]
        public void OnUpdateTransitionConfigStore(string data)
        {
            TransitionConfigStoreTypedStorage.Update(data);
        }
        
        /// <summary>
        /// >> TransitionConfigStore
        ///  Configuration values specific to the transition being used.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.CasinoJamTransitionConfig GetTransitionConfigStore()
        {
            return TransitionConfigStoreTypedStorage.Get();
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.SeasonUnlocks
        /// </summary>
        [StorageChange("CasinoJamSage", "SeasonUnlocks")]
        public void OnUpdateSeasonUnlocks(string key, string data)
        {
            SeasonUnlocksTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> SeasonUnlocks
        ///  Some features need to be unlocked fulfilling certain criteria.
        /// 
        ///  This storage keeps track of the `UnlockRule` that needs to be satisfied to unlock the
        ///  feature. If there is no unlock rule, the feature can't be unlocked in that season.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8 GetSeasonUnlocks(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (SeasonUnlocksTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr5U8 result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.PlayerSeasonConfigs
        /// </summary>
        [StorageChange("CasinoJamSage", "PlayerSeasonConfigs")]
        public void OnUpdatePlayerSeasonConfigs(string key, string data)
        {
            PlayerSeasonConfigsTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> PlayerSeasonConfigs
        ///  Tracks player configs per season. This can be mutated by unlocking certain privileges, e.g.
        ///  upgrading the storage inventory size.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig GetPlayerSeasonConfigs(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (PlayerSeasonConfigsTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerConfig result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.PlayerSeasonStats
        /// </summary>
        [StorageChange("CasinoJamSage", "PlayerSeasonStats")]
        public void OnUpdatePlayerSeasonStats(string key, string data)
        {
            PlayerSeasonStatsTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> PlayerSeasonStats
        ///  Tracks player stats per season.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats GetPlayerSeasonStats(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (PlayerSeasonStatsTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.config.player.PlayerStats result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.Assets
        /// </summary>
        [StorageChange("CasinoJamSage", "Assets")]
        public void OnUpdateAssets(string key, string data)
        {
            AssetsTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> Assets
        ///  Maps the `AssetId` to its owner and the asset.
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset> GetAssets(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (AssetsTypedStorage.Dictionary.TryGetValue(key, out Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.Asset> result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.AssetOwners
        /// </summary>
        [StorageChange("CasinoJamSage", "AssetOwners")]
        public void OnUpdateAssetOwners(string key, string data)
        {
            AssetOwnersTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> AssetOwners
        ///  Keeps track of the assets owned by an account and in which season the asset was created.
        /// 
        ///  We mostly do ownership checks on this in the runtime. Whereas the frontends want to display
        ///  a list. This has to be queried with a `state.getKeysPaged` followed by a `state.getStorage`
        ///  call. Maybe it makes sense to implement a runtime api call for this to reduce networking
        ///  bandwidth.
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseTuple GetAssetOwners(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (AssetOwnersTypedStorage.Dictionary.TryGetValue(key, out Substrate.NetApi.Model.Types.Base.BaseTuple result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.AssetsOwnedCount
        /// </summary>
        [StorageChange("CasinoJamSage", "AssetsOwnedCount")]
        public void OnUpdateAssetsOwnedCount(string key, string data)
        {
            AssetsOwnedCountTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> AssetsOwnedCount
        ///  Keeps track of how many assets an account owns.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 GetAssetsOwnedCount(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (AssetsOwnedCountTypedStorage.Dictionary.TryGetValue(key, out Substrate.NetApi.Model.Types.Primitive.U8 result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.SeasonTradeFilters
        /// </summary>
        [StorageChange("CasinoJamSage", "SeasonTradeFilters")]
        public void OnUpdateSeasonTradeFilters(string key, string data)
        {
            SeasonTradeFiltersTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> SeasonTradeFilters
        ///  A filter that assets need to pass in order to be traded.
        /// 
        ///  The filter can be changed by the organizer.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType GetSeasonTradeFilters(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (SeasonTradeFiltersTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.SeasonTransferFilters
        /// </summary>
        [StorageChange("CasinoJamSage", "SeasonTransferFilters")]
        public void OnUpdateSeasonTransferFilters(string key, string data)
        {
            SeasonTransferFiltersTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> SeasonTransferFilters
        ///  A filter that assets need to pass in order to be transfer.
        /// 
        ///  The filter can be changed by the organizer.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType GetSeasonTransferFilters(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (SeasonTransferFiltersTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset.EnumVariantType result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.AssetTradePrices
        /// </summary>
        [StorageChange("CasinoJamSage", "AssetTradePrices")]
        public void OnUpdateAssetTradePrices(string key, string data)
        {
            AssetTradePricesTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> AssetTradePrices
        ///  Tracks assets that have been put on the market with a certain price.
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 GetAssetTradePrices(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (AssetTradePricesTypedStorage.Dictionary.TryGetValue(key, out Substrate.NetApi.Model.Types.Primitive.U128 result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.LockedAssets
        /// </summary>
        [StorageChange("CasinoJamSage", "LockedAssets")]
        public void OnUpdateLockedAssets(string key, string data)
        {
            LockedAssetsTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> LockedAssets
        ///  Tracks assets that have been locked either through the `lock_asset` extrinsic, or by
        ///  other pallets via this pallet's `AssetManager` implementation.
        /// 
        ///  A locked asset can't be transferred, traded, consumed or mutated.
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock GetLockedAssets(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (LockedAssetsTypedStorage.Dictionary.TryGetValue(key, out Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.asset_manager.Lock result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Implements any storage change for CasinoJamSage.AssetFunds
        /// </summary>
        [StorageChange("CasinoJamSage", "AssetFunds")]
        public void OnUpdateAssetFunds(string key, string data)
        {
            AssetFundsTypedStorage.Update(key, data);
        }
        
        /// <summary>
        /// >> AssetFunds
        ///  Tracks how many funds assets have, which will be returned to the owner, once the
        ///  asset is consumed
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 GetAssetFunds(string key)
        {
            if ((key == null))
            {
                return null;
            }
            if (AssetFundsTypedStorage.Dictionary.TryGetValue(key, out Substrate.NetApi.Model.Types.Primitive.U128 result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
