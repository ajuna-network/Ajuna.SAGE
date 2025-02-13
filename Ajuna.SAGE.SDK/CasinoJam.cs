using Ajuna.SAGE.SDK.Model.CasinoJamSage;
using Ajuna.SAGE.SDK.Model.CasinoJamSeasons;
using Serilog;
using StrobeNet.Extensions;
using Substrate.AjunaSolo.NET.NetApiExt.Client;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_payment_handler.withdraw_credit;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.season_manager;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_seasons.types;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.Integration.Model;
using Substrate.NetApi;
using Substrate.NetApi.Extensions;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Substrate.Integration
{
    /// <summary>
    /// Substrate network
    /// </summary>
    public partial class SubstrateNetwork : BaseClient
    {
        #region MultiStorage

        public async Task<uint[]?> GetAssetIdByAsync(AccountId32 accountId32, uint seasonId, string blockhash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            //Substrate.NetApi.Model.Types.Base.BaseTuple<AccountId32, Asset>

            var list = new List<byte>();
            list.AddRange(accountId32.Encode());
            list.AddRange(new U32(seasonId).Encode());
            byte[]? subKey = list.ToArray();
            var result = await GetAllStorageAsync<BaseTuple<AccountId32, U32, U32>, BaseTuple>("CasinoJamSage", "AssetOwners", blockhash, subKey, 0, token);
            if (result == null)
            {
                Log.Debug("Result is null!");
                return null;
            }
            foreach (var item in result.Keys)
            {
                var tupAccountId = ((AccountId32)item.Value[0]).ToAddress();
                var tupSeasonId = ((U32)item.Value[1]).Value;
                var tupAssetId = ((U32)item.Value[2]).Value;
            }
            var assetIds = result.Keys.Select(p => ((U32)p.Value[2]).Value).ToArray();
            if (assetIds.Length == 0)
            {
                Log.Debug("AssetIds is empty!");
                return null;
            }

            return assetIds;
        }

        public async Task<Dictionary<uint, (string, AssetSharp)>?> GetAssetByKeysAsync(uint[] keys, string blockhash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var hexKeys = keys.Select(p => Utils.Bytes2HexString(new U32(p).Encode(), Utils.HexStringFormat.Pure)).ToList();

            var result = await GetStorageByKeysAsync<BaseTuple<AccountId32, Asset>>("CasinoJamSage", "Assets", hexKeys, blockhash, token);
            if (result == null)
            {
                Log.Debug("Result is null!");
                return null;
            }

            var dict = new Dictionary<uint, (string, AssetSharp)>();
            for ( var i = 0; i < result.Count; i++)
            {
                var key = new U32();
                key.Create(result[i].Item1);
                var item = result[i].Item2;
                var accountId = ((AccountId32)item.Value[0]).ToAddress();
                var asset = new AssetSharp((Asset)item.Value[1]);
                dict.Add(key.Value, (accountId, asset));
            }
            return dict;
        }

        #endregion MultiStorage

        #region storage

        /// <summary>
        /// Get the organizer address as string
        /// </summary>
        /// <param name="blockhash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> GetOrganizerAsync(string? blockhash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var result = await SubstrateClient.CasinoJamSageStorage.Organizer(blockhash, token);

            if (result == null)
            {
                Log.Debug("Organizer is null!");
                return null;
            }

            return result.ToAddress();
        }

        #endregion storage

        #region call


        /// <summary>
        /// Set the organizer
        /// </summary>
        /// <param name="sudo"></param>
        /// <param name="organizer"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> SetOrganizerAsync(Account sudo, AccountId32 organizer, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "CasinoJamSageCalls.SetOrganizer";

            if (!IsConnected || sudo == null)
            {
                return null;
            }

            var enumCall = new EnumRuntimeCall();
            var palletCall = new AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.pallet.EnumCall();
            palletCall.Create(AjunaSolo.NET.NetApiExt.Generated.Model.pallet_sage.pallet.Call.set_organizer, organizer);
            enumCall.Create(RuntimeCall.CasinoJamSage, palletCall);
            var extrinsic = SudoCalls.Sudo(enumCall);

            return await GenericExtrinsicAsync(sudo, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Create or Update a season
        /// </summary>
        /// <param name="organizer"></param>
        /// <param name="seasonId"></param>
        /// <param name="seasonConfig"></param>
        /// <param name="seasonMetadata"></param>
        /// <param name="seasonSchedule"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> UpdateSeasonAsync(Account organizer, uint seasonId, SeasonConfigSharp seasonConfig, SeasonMetadataSharp seasonMetadata, SeasonScheduleSharp seasonSchedule, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "CasinoJamSageCalls.SetOrganizer";

            if (!IsConnected || organizer == null)
            {
                return null;
            }

            var seasonIdU32 = new U32(seasonId);
            var seasonOpt = new BaseOpt<SeasonConfig>(seasonConfig.ToSubstrate());
            var seasonMetaOpt = new BaseOpt<SeasonMetadata>(seasonMetadata.ToSubstrate());
            var seasonScheduleOpt = new BaseOpt<SeasonSchedule>(seasonSchedule.ToSubstrate());

            var extrinsic = CasinoJamSeasonsCalls.UpdateSeason(
                new U32(seasonId),
                seasonOpt,
                seasonMetaOpt,
                seasonScheduleOpt);

            return await GenericExtrinsicAsync(organizer, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// State transition
        /// </summary>
        /// <param name="account"></param>
        /// <param name="casinoActionSharp"></param>
        /// <param name="assetIds"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> StateTransitionAsync(Account account, EnumCasinoAction enumCasinoAction, uint[]? assetIds, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "CasinoJamSageCalls.StateTransitionAsync";

            if (!IsConnected || account == null)
            {
                return null;
            }

            var extrinsic = CasinoJamSageCalls.StateTransition(
                enumCasinoAction,
                new BaseVec<U32>(assetIds.Select(p => new U32(p)).ToArray()),
                new BaseTuple(),
                new BaseOpt<EnumWithdrawKind>());

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        #endregion call
    }
}