using Ajuna.SAGE.Generic.Model;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Generic.Tests")]

namespace Ajuna.SAGE.Generic
{
    public delegate IEnumerable<IAsset> TransitionFunction<TRules>(TRules[] rules, IEnumerable<IAsset> assets, byte[] randomHash, uint blockNumber)
        where TRules : ITransitionRule;

    public class Engine<TIdentifier, TRules>
         where TIdentifier : ITransitionIdentifier
         where TRules : ITransitionRule
    {
        private readonly IBlockchainInfoProvider _blockchainInfo;

        private readonly Func<IPlayer, TRules, IAsset[], uint, bool> _verifyFunction;

        private readonly Dictionary<TIdentifier, (TRules[] Rules, TransitionFunction<TRules> Function)> _transitions;

        /// <summary>
        /// Game
        /// </summary>
        /// <param name="seed"></param>
        public Engine(IBlockchainInfoProvider blockchainInfo, Func<IPlayer, TRules, IAsset[], uint, bool> verifyFunction)
        {
            _blockchainInfo = blockchainInfo;
            _verifyFunction = verifyFunction;
            _transitions = new Dictionary<TIdentifier, (TRules[] Rules, TransitionFunction<TRules> Function)>();
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
        public void AddTransition(TIdentifier identifier, TRules[] rules, TransitionFunction<TRules> function)
        {
            _transitions[identifier] = (rules, function);
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

            if (!_transitions.TryGetValue(identifier, out (TRules[] rules, TransitionFunction<TRules> function) tuple))
            {
                throw new NotSupportedException($"Unsupported Transition for Identifier ({identifier.TransitionType}, {identifier.TransitionSubType}).");
            }

            var rules = tuple.rules;
            var function = tuple.function;

            if (!rules.All(rule => _verifyFunction(executor, rule, assets, blockNumber)))
            {
                result = [];
                return false;
            }

            var list = new List<IAsset>();

            // execute function
            list.AddRange(function(rules, assets, randomHash, blockNumber));

            result = list.ToArray();

            return true;
        }
    }
}