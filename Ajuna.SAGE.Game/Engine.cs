using Ajuna.SAGE.Game.Model;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.Test")]

namespace Ajuna.SAGE.Game
{
    public delegate IEnumerable<IAsset> TransitionFunction<TRules>(
        TRules[] rules,
        ITransitioFee? fee,
        IEnumerable<IAsset> assets,
        byte[] randomHash,
        uint blockNumber,
        IAssetBalanceManager assetBalances)
        where TRules : ITransitionRule;

    public class Engine<TIdentifier, TRules>
         where TIdentifier : ITransitionIdentifier
         where TRules : ITransitionRule
    {
        private readonly IBlockchainInfoProvider _blockchainInfo;
        public IBlockchainInfoProvider BlockchainInfoProvider => _blockchainInfo;

        private readonly Func<IPlayer, TRules, IAsset[], uint, bool> _verifyFunction;

        private readonly Dictionary<TIdentifier, (TRules[] Rules, ITransitioFee? fee, TransitionFunction<TRules> Function)> _transitions;

        private readonly AssetBalanceManager _assetBalanceManager;
        public uint? AssetBalance(ulong id) => _assetBalanceManager.AssetBalance(id);

        /// <summary>
        /// Game
        /// </summary>
        /// <param name="seed"></param>
        public Engine(IBlockchainInfoProvider blockchainInfo, Func<IPlayer, TRules, IAsset[], uint, bool> verifyFunction)
        {
            _blockchainInfo = blockchainInfo;
            _verifyFunction = verifyFunction;
            _transitions = new Dictionary<TIdentifier, (TRules[] Rules, ITransitioFee? fee, TransitionFunction<TRules> Function)>();
            _assetBalanceManager = new AssetBalanceManager();
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
        public bool Transition(Player executor, TIdentifier identifier, IAsset[]? avatars, out IAsset[] result)
        {
            return Transition(executor, identifier, avatars, _blockchainInfo.GenerateRandomHash(), _blockchainInfo.CurrentBlockNumber, out result);
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="assets"></param>
        /// <param name="randomHash"></param>
        /// <param name="blockNumber"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        internal bool Transition(IPlayer executor, TIdentifier identifier, IAsset[]? assets, byte[] randomHash, uint blockNumber, out IAsset[] result)
        {
            // initialize to avoid null checks
            assets ??= [];

            // duplicate check
            if (assets.Distinct().Count() != assets.Length)
            {
                throw new NotSupportedException("Trying to Forge duplicates.");
            }

            if (!_transitions.TryGetValue(identifier, out (TRules[] rules, ITransitioFee? fee, TransitionFunction<TRules> function) tuple))
            {
                throw new NotSupportedException($"Unsupported Transition for Identifier ({identifier.TransitionType}, {identifier.TransitionSubType}).");
            }

            TRules[] rules = tuple.rules;
            ITransitioFee? fee = tuple.fee;
            TransitionFunction<TRules> function = tuple.function;

            // check if the executor has the assets and the rules are all okay
            if (!rules.All(rule => _verifyFunction(executor, rule, assets, blockNumber)))
            {
                result = [];
                return false;
            }

            // check if the executor has enough balance to pay the fee
            if (fee != null && fee.Fee > 0 && !executor.Balance.Withdraw(fee.Fee))
            {
                result = [];
                return false;
            }

            // execute the transition function
            IEnumerable<IAsset> functionResult = function(rules, fee, assets, randomHash, blockNumber, _assetBalanceManager);

            result = functionResult != null ? [.. functionResult] : [];

            return true;
        }

    }
}