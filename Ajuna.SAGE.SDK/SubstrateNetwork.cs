using Serilog;
using Substrate.AjunaSolo.NET.NetApiExt.Client;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_runtime.multiaddress;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Storage;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.Integration.Model;
using Substrate.NetApi;
using Substrate.NetApi.Model.Types;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Decimals
        /// </summary>
        public const long DECIMALS = 1_000_000_000_000;

        public const long EX_DEPO = 1_000_000_000;

        /// <summary>
        /// Account
        /// </summary>
        public Account Account { get; set; }

        /// <summary>
        /// Substrate network constructor
        /// </summary>
        /// <param name="account"></param>
        /// <param name="url"></param>
        public SubstrateNetwork(Account account, string url) : base(url)
        {
            Account = account;
        }

        #region storage

        /// <summary>
        /// Get the current block number.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<uint?> GetBlocknumberAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var result = await SubstrateClient.SystemStorage.Number(null, token);

            if (result == null)
            {
                return null;
            }

            return result.Value;
        }

        /// <summary>
        /// Get the total issuance.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BigInteger?> GetTotalIssuanceAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var result = await SubstrateClient.BalancesStorage.TotalIssuance(null, token);

            if (result == null)
            {
                return null;
            }

            return result.Value;
        }

        /// <summary>
        /// Get the inactive issuance.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<BigInteger?> GetInactiveIssuanceAsync(CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            var result = await SubstrateClient.BalancesStorage.InactiveIssuance(null, token);

            if (result == null)
            {
                return null;
            }

            return result.Value;
        }

        /// <summary>
        /// Get owner account informations.
        /// </summary>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfoSharp?> GetAccountAsync(string? blockHash, CancellationToken token)
            => await GetAccountAsync(Account, blockHash, token);

        /// <summary>
        /// Get owner account informations.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfoSharp?> GetAccountAsync(Account key, string? blockHash, CancellationToken token)
            => await GetAccountAsync(key.ToAccountId32(), blockHash, token);

        /// <summary>
        /// Get owner account informations.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<AccountInfoSharp?> GetAccountAsync(AccountId32 key, string? blockHash, CancellationToken token)
        {
            if (!IsConnected)
            {
                Log.Warning("Currently not connected to the network!");
                return null;
            }

            if (key == null || key.Value == null)
            {
                Log.Debug("No account reference given as key!");
                return null;
            }

            var result = await SubstrateClient.SystemStorage.Account(key, blockHash, token);
            if (result == null)
            {
                return null;
            }

            return new AccountInfoSharp(result);
        }

        #endregion storage

        #region storage_all_generic

        /// <summary>
        /// Get all storage as dictionary.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Dictionary<T1, T2>> GetAllStorageAsync<T1, T2>(string module, string item, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            return await GetAllStorageAsync<T1, T2>(module, item, null, null, 0, token);
        }

        /// <summary>
        /// Get all storage as dictionary.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="subKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Dictionary<T1, T2>> GetAllStorageAsync<T1, T2>(string module, string item, byte[] subKey, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            return await GetAllStorageAsync<T1, T2>(module, item, null, subKey, 0, token);
        }

        /// <summary>
        /// Get all storage as dictionary.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Dictionary<T1, T2>> GetAllStorageAsync<T1, T2>(string module, string item, string blockHash, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            return await GetAllStorageAsync<T1, T2>(module, item, blockHash, null, 0, token);
        }

        /// <summary>
        /// Get all storage as dictionary.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="blockHash"></param>
        /// <param name="subKey"></param>
        /// <param name="keyOffset"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<Dictionary<T1, T2>> GetAllStorageAsync<T1, T2>(string module, string item, string blockHash, byte[]? subKey, int keyOffset, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            byte[]? nextStorageKey = null;
            List<(byte[], T1, T2)> storageEntries;

            var result = new Dictionary<T1, T2>();

            uint batchSize = 1000;

            do
            {
                storageEntries = await GetStoragePagedAsync<T1, T2>(module, item, nextStorageKey, batchSize, blockHash, subKey, keyOffset, token);

                foreach (var storageEntry in storageEntries)
                {
                    result.Add(storageEntry.Item2, storageEntry.Item3);

                    nextStorageKey = storageEntry.Item1;
                }

                Log.Debug("{0}.{1} storage +{2} records parsed.", module, item, batchSize);
            } while (storageEntries.Any());

            Log.Debug("{0}.{1} storage {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        /// Get all storage paged.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="startKey"></param>
        /// <param name="page"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<(byte[], T1, T2)>> GetStoragePagedAsync<T1, T2>(string module, string item, byte[] startKey, uint page, string blockHash, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            return await GetStoragePagedAsync<T1, T2>(module, item, startKey, page, blockHash, null, 0, token);
        }

        /// <summary>
        /// Get all storage paged.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="startKey"></param>
        /// <param name="page"></param>
        /// <param name="blockHash"></param>
        /// <param name="subKey"></param>
        /// <param name="keyOffset"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<(byte[], T1, T2)>> GetStoragePagedAsync<T1, T2>(string module, string item, byte[]? startKey, uint page, string blockHash, byte[]? subKey, int keyOffset, CancellationToken token)
            where T1 : IType, new()
            where T2 : IType, new()
        {
            if (!IsConnected)
            {
                return null;
            }

            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var result = new List<(byte[], T1, T2)>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash(module, item);

            var extKeyBytes = (byte[])keyBytes.Clone();  // Clone the original array to keep it intact
            if (subKey != null)
            {
                int oldLength = extKeyBytes.Length;
                Array.Resize(ref extKeyBytes, oldLength + subKey.Length);
                Array.Copy(subKey, 0, extKeyBytes, oldLength, subKey.Length);
            }

            var storageKeys = await SubstrateClient.State.GetKeysPagedAsync(extKeyBytes, page, startKey, token);
            if (storageKeys == null || !storageKeys.Any())
            {
                return result;
            }

            var keySize = Utils.Bytes2HexString(keyBytes).Length + keyOffset;

            var storageChangeSets = await SubstrateClient.State.GetQueryStorageAtAsync(storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList(), blockHash, token);
            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.First().Changes)
                {
                    var storageKeyString = storageChangeSet[0];

                    var keyParam = new T1();
                    keyParam.Create(storageKeyString[keySize..]);

                    T2 valueParam = default;
                    if (storageChangeSet[1] != null)
                    {
                        valueParam = new T2();
                        valueParam.Create(storageChangeSet[1]);
                    }

                    result.Add((Utils.HexToByteArray(storageKeyString), keyParam, valueParam));
                }
            }

            return result;
        }

        /// <summary>
        /// Get all storage keys as list.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="blockHash"></param>
        /// <param name="subKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<byte[]>> GetAllStorageKeysAsync(string module, string item, string blockHash, byte[] subKey, CancellationToken token)
        {
            var result = new List<byte[]>();

            byte[] nextStorageKey = null;
            List<byte[]> storageEntries;
            do
            {
                storageEntries = await GetStorageKeysPagedAsync(module, item, nextStorageKey, 1000, blockHash, subKey, token);

                if (storageEntries != null && storageEntries.Any())
                {
                    result.AddRange(storageEntries);
                    //nextStorageKey = storageEntries[^1];
                    nextStorageKey = storageEntries[storageEntries.Count - 1];
                }
            }
            while (storageEntries != null && storageEntries.Any());

            Log.Debug("{0}.{1} storage key {2} records parsed.", module, item, result.Count);

            return result;
        }

        /// <summary>
        /// Get all storage keys paged.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="startKey"></param>
        /// <param name="page"></param>
        /// <param name="blockHash"></param>
        /// <param name="subKey"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<byte[]>> GetStorageKeysPagedAsync(string module, string item, byte[] startKey, uint page, string blockHash, byte[] subKey, CancellationToken token)
        {
            if (!IsConnected)
            {
                return null;
            }

            if (page < 2 || page > 1000)
            {
                throw new NotSupportedException("Page size must be in the range of 2 - 1000");
            }

            var result = new List<byte[]>();
            var keyBytes = RequestGenerator.GetStorageKeyBytesHash(module, item);

            var extKeyBytes = (byte[])keyBytes.Clone();  // Clone the original array to keep it intact
            if (subKey != null)
            {
                int oldLength = extKeyBytes.Length;
                Array.Resize(ref extKeyBytes, oldLength + subKey.Length);
                Array.Copy(subKey, 0, extKeyBytes, oldLength, subKey.Length);
            }

            var storageKeys = await SubstrateClient.State.GetKeysPagedAsync(extKeyBytes, page, startKey, token);
            if (storageKeys == null || !storageKeys.Any())
            {
                return result;
            }

            return storageKeys.Select(p => Utils.HexToByteArray(p.ToString())).ToList();
        }

        /// <summary>
        /// Get all storage by keys.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="module"></param>
        /// <param name="item"></param>
        /// <param name="keys"></param>
        /// <param name="blockHash"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<List<(string, T1)>> GetStorageByKeysAsync<T1>(string module, string item, List<string> keys, string blockHash, CancellationToken token)
            where T1 : IType, new()
        {
            if (!IsConnected)
            {
                return null;
            }

            if (keys.Count > 1000)
            {
                throw new NotSupportedException("Max keys to query is 1000");
            }

            var keyBytesHex = Utils.Bytes2HexString(RequestGenerator.GetStorageKeyBytesHash(module, item));
            var keySize = keyBytesHex.Length;
            var result = new List<(string, T1)>();

            var storageChangeSets = await SubstrateClient.State.GetQueryStorageAtAsync(keys.Select(p => Utils.HexToByteArray(keyBytesHex + p.ToString())).ToList(), blockHash, token);
            if (storageChangeSets != null)
            {
                foreach (var storageChangeSet in storageChangeSets.First().Changes)
                {
                    var storageKeyString = storageChangeSet[0];
                    var valueParam = new T1();
                    if (storageChangeSet[1] != null)
                    {
                        valueParam.Create(storageChangeSet[1]);
                    }
                    //result.Add((storageKeyString[keySize..], valueParam));
                    result.Add((storageKeyString.Substring(keySize), valueParam));
                }
            }

            return result;
        }

        #endregion storage_all_generic

        #region extrinsics

        /// <summary>
        /// Remarks
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> RemarksAsync(BaseVec<U8> remark, int concurrentTasks, CancellationToken token)
        {
            return await RemarksAsync(Account, remark, concurrentTasks, token);
        }

        /// <summary>
        /// Remarks
        /// </summary>
        /// <param name="account"></param>
        /// <param name="remark"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> RemarksAsync(Account account, BaseVec<U8> remark, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "SystemCalls.Remark";

            if (!IsConnected || account == null)
            {
                return null;
            }

            var extrinsic = SystemCalls.Remark(remark);

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Transfer keep alive
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="value"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> TransferKeepAliveAsync(AccountId32 dest, BigInteger value, int concurrentTasks, CancellationToken token)
        {
            return await TransferKeepAliveAsync(Account, dest, value, concurrentTasks, token);
        }

        /// <summary>
        /// Transfer keep alive
        /// </summary>
        /// <param name="account"></param>
        /// <param name="dest"></param>
        /// <param name="value"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> TransferKeepAliveAsync(Account account, AccountId32 dest, BigInteger value, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "BalancesCalls.TransferKeepAlive";

            if (!IsConnected || account == null)
            {
                return null;
            }

            var multiAddress = new EnumMultiAddress();
            multiAddress.Create(MultiAddress.Id, dest);

            var balance = new BaseCom<U128>();
            balance.Create(value);

            var extrinsic = BalancesCalls.TransferKeepAlive(multiAddress, balance);

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        /// <summary>
        /// Batch all extrinsics in one transaction.
        /// </summary>
        /// <param name="callList"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> BatchAllAsync(List<EnumRuntimeCall> callList, int concurrentTasks, CancellationToken token)
        {
            return await BatchAllAsync(Account, callList, concurrentTasks, token);
        }

        /// <summary>
        /// Batch all extrinsics in one transaction.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="callList"></param>
        /// <param name="concurrentTasks"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<string?> BatchAllAsync(Account account, List<EnumRuntimeCall> callList, int concurrentTasks, CancellationToken token)
        {
            var extrinsicType = "UtilityCalls.BatchAll";

            if (!IsConnected || Account == null || callList == null || callList.Count == 0)
            {
                return null;
            }

            var calls = new BaseVec<EnumRuntimeCall>();
            calls.Create(callList.ToArray());

            var extrinsic = UtilityCalls.BatchAll(calls);

            return await GenericExtrinsicAsync(account, extrinsicType, extrinsic, concurrentTasks, token);
        }

        #endregion extrinsics
    }
}