using Ajuna.SAGE.Game.Manager;
using Ajuna.SAGE.Game.Model;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.Test")]

namespace Ajuna.SAGE.Game
{
    public delegate IEnumerable<IAsset> TransitionFunction<TRules>(
        IAccount executor,
        TRules[] rules,
        ITransitioFee? fee,
        IEnumerable<IAsset> assets,
        byte[] randomHash,
        uint blockNumber,
        IBalanceManager assetBalances)
        where TRules : ITransitionRule;

    public class Engine<TIdentifier, TRules>
         where TIdentifier : ITransitionIdentifier
         where TRules : ITransitionRule
    {
        private readonly IBlockchainInfoProvider _blockchainInfo;
        public IBlockchainInfoProvider BlockchainInfoProvider => _blockchainInfo;

        private readonly Func<IAccount, TRules, IAsset[], uint, IBalanceManager, bool> _verifyFunction;

        private readonly Dictionary<TIdentifier, (TRules[] Rules, ITransitioFee? fee, TransitionFunction<TRules> Function)> _transitions;

        private readonly AccountManager _accountManager;
        public IAccountManager AccountManager => _accountManager;

        private readonly AssetManager _assetManager;
        public IAssetManager AssetManager => _assetManager;

        private readonly BalanceManager _assetBalanceManager;
        public IBalanceManager AssetBalanceManager => _assetBalanceManager;

        // only for testing
        public uint? AssetBalance(ulong id) => _assetBalanceManager.AssetBalance(id);

        /// <summary>
        /// Game
        /// </summary>
        /// <param name="seed"></param>
        public Engine(IBlockchainInfoProvider blockchainInfo, Func<IAccount, TRules, IAsset[], uint, IBalanceManager, bool> verifyFunction)
        {
            _blockchainInfo = blockchainInfo;
            _verifyFunction = verifyFunction;
            _transitions = [];

            _accountManager = new AccountManager();
            _assetManager = new AssetManager();
            _assetBalanceManager = new BalanceManager();
        }

        /// <summary>
        /// Blockchain Info Provider
        /// </summary>
        public IBlockchainInfoProvider BlockchainInfo => _blockchainInfo;

        /// <summary>
        ///
        /// </summary>
        /// <param name="idType1"></param>
        /// <param name="idType2"></param>
        /// <param name="transitionFunction"></param>
        public void AddTransition(TIdentifier identifier, TRules[] rules, ITransitioFee? fee, TransitionFunction<TRules> function)
        {
            _transitions[identifier] = (rules, fee, function);
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="avatars"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        public bool Transition(IAccount executor, TIdentifier identifier, IAsset[]? avatars, out IAsset[] result)
        {
            return Transition(executor, identifier, avatars, _blockchainInfo.GenerateRandomHash(), _blockchainInfo.CurrentBlockNumber, out result);
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="inAssets"></param>
        /// <param name="randomHash"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal bool Transition(IAccount executor, TIdentifier identifier, IAsset[]? inAssets, byte[] randomHash, uint blockNumber, out IAsset[] outAssets)
        {
            // initialize to avoid null checks
            inAssets ??= [];

            // duplicate check
            if (inAssets.Distinct().Count() != inAssets.Length)
            {
                throw new NotSupportedException("Trying to transition duplicates.");
            }

            // lockable check
            if (inAssets.Any(p => p.IsLockable))
            {
                throw new NotSupportedException("Trying to transition lockable.");
            }

            if (!_transitions.TryGetValue(identifier, out (TRules[] rules, ITransitioFee? fee, TransitionFunction<TRules> function) tuple))
            {
                throw new NotSupportedException($"Unsupported Transition for Identifier ({identifier.TransitionType}, {identifier.TransitionSubType}).");
            }

            TRules[] rules = tuple.rules;
            ITransitioFee? fee = tuple.fee;
            TransitionFunction<TRules> function = tuple.function;

            // check if the executor has the assets and the rules are all okay
            if (!rules.All(rule => _verifyFunction(executor, rule, inAssets, blockNumber, _assetBalanceManager)))
            {
                outAssets = [];
                return false;
            }

            // check if the executor has enough balance to pay the fee
            if (fee != null && fee.Fee > 0 && !executor.Balance.Withdraw(fee.Fee))
            {
                outAssets = [];
                return false;
            }

            // execute the transition function
            IEnumerable<IAsset> functionResult = function(executor, rules, fee, inAssets, randomHash, blockNumber, _assetBalanceManager);

            outAssets = functionResult != null ? [.. functionResult] : [];

            Transition(inAssets, outAssets);

            return true;
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        internal void Transition(IAsset[]? inputs, IAsset[]? outputs)
        {
            var inputIds = inputs?.Select(a => a.Id).ToHashSet();
            var outputIds = outputs?.Select(a => a.Id).ToHashSet();
            var updateIds = new List<uint>();
            var deleteIds = new List<uint>();
            var createIds = new List<uint>();
            if (inputIds != null)
            {
                foreach (var inputId in inputIds)
                {
                    if (outputIds != null && outputIds.Contains(inputId))
                    {

                        updateIds.Add(inputId);
                     }
                    else
                    {
                        deleteIds.Add(inputId);
                    }
                }
            }

            if (outputs != null)
            {
                foreach (var output in outputIds)
                {
                    if (inputIds == null || !inputIds.Contains(output))
                    {
                        createIds.Add(output);
                    }
                }
            }

            if (outputs != null)
            {
                foreach (var id in createIds)
                {
                    _assetManager.Create(outputs.First(p => p.Id == id));
                }
            }

            if (inputs != null)
            {
                foreach (var id in updateIds)
                {
                    _assetManager.Update(inputs.First(p => p.Id == id));
                }
            }

            foreach (var id in deleteIds)
            {
                _assetManager.Delete(id);
            }
        }

    }
}