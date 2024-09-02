using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJameGame
    {
        /// <summary>
        /// Create an instance of the HeroJam game engine
        /// </summary>
        /// <param name="blockchainInfoProvider"></param>
        /// <returns></returns>
        public static Engine<HeroJamIdentifier, HeroJamRule> Create(IBlockchainInfoProvider blockchainInfoProvider)
        {
            var engineBuilder = new EngineBuilder<HeroJamIdentifier, HeroJamRule>(blockchainInfoProvider);

            engineBuilder.SetVerifyFunction(GetVerifyFunction());

            var rulesAndTransitions = GetRulesAndTranstionSets();
            foreach (var (identifier, rules, transition) in rulesAndTransitions)
            {
                engineBuilder.AddTransition(identifier, rules, transition);
            }

            return engineBuilder.Build();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static Func<IPlayer, HeroJamRule, Asset[], uint, bool> GetVerifyFunction()
        {
            return (player, rule, assets, blocknumber) =>
            {
                switch (rule.RuleType)
                {
                    case (byte)HeroRuleType.AssetCount:
                        {
                            switch(rule.RuleOp)
                            {
                                case (byte)HeroRuleOp.EQ:
                                    return assets.Length == BitConverter.ToUInt32(rule.RuleValue);

                                case (byte)HeroRuleOp.GE:
                                    return assets.Length >= BitConverter.ToUInt32(rule.RuleValue);

                                case (byte)HeroRuleOp.GT:
                                    return assets.Length > BitConverter.ToUInt32(rule.RuleValue);

                                case (byte)HeroRuleOp.LT:
                                    return assets.Length < BitConverter.ToUInt32(rule.RuleValue);

                                case (byte)HeroRuleOp.LE:
                                    return assets.Length <= BitConverter.ToUInt32(rule.RuleValue);

                                case (byte)HeroRuleOp.NE:
                                    return assets.Length != BitConverter.ToUInt32(rule.RuleValue);

                                default:
                                    return false;
                            }
                        }

                    case (byte)HeroRuleType.IsOwnerOf:
                        {
                            if (rule.RuleOp != (byte)HeroRuleOp.Index)
                            {
                                return false;
                            }
                            var assetIndex = BitConverter.ToUInt32(rule.RuleValue);
                            if (assets.Length <= assetIndex)
                            {
                                return false;
                            }
                            return player.IsOwnerOf(assets[assetIndex]);
                        }

                    case (byte)HeroRuleType.AllAssetType:
                        {
                            return assets.All(a =>
                            {
                                var heroJameAsset = a as HeroJamAsset;
                                return heroJameAsset != null && heroJameAsset.AssetType == (AssetType)BitConverter.ToUInt32(rule.RuleValue);
                            });
                        }

                    case (byte)HeroRuleType.CanStateChange:
                        {
                            if (rule.RuleOp != (byte)HeroRuleOp.Index)
                            {
                                return false;
                            }
                            var assetIndex = BitConverter.ToUInt32(rule.RuleValue);
                            if (assets.Length <= assetIndex)
                            {
                                return false;
                            }
                            var heroJamAsset = assets[assetIndex] as HeroJamAsset;
                            return heroJamAsset != null && heroJamAsset.StateChangeBlockNumber < blocknumber;
                        }

                    case (byte)HeroRuleType.SameExist:
                        {
                            // TODO: Implement this rule
                            return false;
                        }

                    default:
                        return false;
                }
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<(HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>)> GetRulesAndTranstionSets()
        {
            var result = new List<(HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>)>
            {
                GetSleepTransitionSet(ActionTime.FullDay),
                GetSleepTransitionSet(ActionTime.HalfDay),
                GetSleepTransitionSet(ActionTime.Hour),
            };

            return result;
        }

        private static (HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>) GetSleepTransitionSet(ActionTime actionTime)
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)actionTime);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.NE, (uint)StateType.Sleep),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, w, h, b) =>
            {
                var asset = w.First().Asset;
                asset.Score += 10;
                return [asset];
            };

            return (identifier, rules, function);
        }
    }
}