﻿using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Generic
{
    /// <summary>
    /// Engine Builder class
    /// </summary>
    public class EngineBuilder<TIdentifier, TRules>
        where TIdentifier : ITransitionIdentifier
        where TRules : ITransitionRule
    {
        private readonly IBlockchainInfoProvider _blockchainInfoProvider;

        private readonly Dictionary<TIdentifier, (TRules[] Rules, TransitionFunction<TRules> Function)> _transitions;

        private Func<IPlayer, TRules, Asset[], uint, bool> _verifyFunction;

        /// <summary>
        /// Engine Builder
        /// </summary>
        /// <param name="blockchainInfoProvider"></param>
        public EngineBuilder(IBlockchainInfoProvider blockchainInfoProvider)
        {
            _blockchainInfoProvider = blockchainInfoProvider;
            _transitions = new Dictionary<TIdentifier, (TRules[] Rules, TransitionFunction<TRules> Function)>();
        }

        /// <summary>
        /// Add Transition
        /// </summary>
        /// <param name="idType1"></param>
        /// <param name="idType2"></param>
        /// <param name="transitionFunction"></param>
        /// <returns></returns>
        public EngineBuilder<TIdentifier, TRules> AddTransition(TIdentifier identifier, TRules[] rules, TransitionFunction<TRules> transitionFunction)
        {
            _transitions[identifier] = (rules, transitionFunction);
            return this;
        }

        /// <summary>
        /// Set the Verify function.
        /// </summary>
        /// <param name="verifyFunction"></param>
        /// <returns></returns>
        public EngineBuilder<TIdentifier, TRules> SetVerifyFunction(Func<IPlayer, TRules, Asset[], uint, bool> verifyFunction)
        {
            _verifyFunction = verifyFunction;
            return this;
        }

        /// <summary>
        /// Build
        /// </summary>
        /// <returns></returns>
        public Engine<TIdentifier, TRules> Build()
        {
            // Ensure that a verify function is set
            if (_verifyFunction == null)
            {
                throw new InvalidOperationException("A verify function must be provided before building the engine.");
            }

            // Create a Game instance with the mandatory seed
            var engine = new Engine<TIdentifier, TRules>(_blockchainInfoProvider, _verifyFunction);


            // Add all the configured transitions
            foreach (var transition in _transitions)
            {
                engine.AddTransition(transition.Key, transition.Value.Rules, transition.Value.Function);
            }

            return engine;
        }
    }
}