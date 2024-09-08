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
        private static Func<IPlayer, HeroJamRule, IAsset[], uint, bool> GetVerifyFunction()
        {
            return (player, rule, assets, blocknumber) =>
            {
                switch (rule.RuleType)
                {
                    case (byte)HeroRuleType.AssetCount:
                        {
                            switch (rule.RuleOp)
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
                                HeroJamAsset? heroJameAsset = a as HeroJamAsset;
                                var expectedAssetType = (AssetType)BitConverter.ToUInt32(rule.RuleValue);
                                return heroJameAsset != null && heroJameAsset.AssetType == expectedAssetType;
                            });
                        }

                    case (byte)HeroRuleType.AllStateType:
                        {
                            return assets.All(a =>
                            {
                                HeroJamAsset? heroJameAsset = a as HeroJamAsset;
                                var expectedStateType = (StateType)BitConverter.ToUInt32(rule.RuleValue);
                                return rule.RuleOp switch
                                {
                                    (byte)HeroRuleOp.EQ => heroJameAsset != null && heroJameAsset.StateType == expectedStateType,
                                    (byte)HeroRuleOp.NE => heroJameAsset != null && heroJameAsset.StateType != expectedStateType,
                                    _ => throw new NotSupportedException($"Unsupported RuleOp {rule.RuleOp} for RuleType {rule.RuleType}!"),
                                };
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
                            return assets[assetIndex] is HeroJamAsset heroJamAsset && heroJamAsset.StateChangeBlockNumber < blocknumber;
                        }

                    case (byte)HeroRuleType.SameExist:
                        {
                            if (player.Assets == null || player.Assets.Count == 0)
                            {
                                return true;
                            }

                            return player.Assets.Any(a => a.MatchType.SequenceEqual(rule.RuleValue));
                        }

                    default:
                        throw new NotSupportedException($"Unsupported RuleType {rule.RuleType}!");
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
                GetCreateHero(),
                GetSleepTransitionSet(ActionTime.Short),
                GetSleepTransitionSet(ActionTime.Medium),
                GetSleepTransitionSet(ActionTime.Long),
            };

            return result;
        }

        /// <summary>
        /// Get the transition set for the create hero action
        /// </summary>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>) GetCreateHero()
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Create, (byte)AssetType.Hero);
            byte matchType = (byte)AssetType.Hero << 4 + (byte)AssetSubType.None;
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 0),
                new HeroJamRule(HeroRuleType.SameExist, HeroRuleOp.MatchType, matchType),
            ];

            TransitionFunction<HeroJamRule> function = (r, a, h, b) =>
            {
                var asset = new HeroJamAssetBuilder(null, HeroJamConstant.COLLECTION_ID, AssetType.Hero, AssetSubType.None)
                    .SetEnergy(100)
                    .SetStateType(StateType.Idle)
                    .SetStateChangeBlockNumber(0)
                    .Build();
                return [asset];
            };

            return (identifier, rules, function);
        }

        /// <summary>
        /// Get the transition set for the sleep action
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>) GetSleepTransitionSet(ActionTime actionTime)
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)actionTime);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.EQ, (uint)StateType.Idle),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, a, h, b) =>
            {
                //var heroJamAsset = a.ElementAt(0) as HeroJamAsset;
                var heroJamAsset = new HeroJamAsset(a.ElementAt(0))
                {
                    StateType = StateType.Sleep,
                    StateValue = (byte)actionTime,
                    StateChangeBlockNumber = b + GetBlockTimeFrom(actionTime)
                };

                return [heroJamAsset];
            };

            return (identifier, rules, function);
        }

        /// <summary>
        /// Get the transition set for the work action
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], TransitionFunction<HeroJamRule>) GetWorkTransitionSet(WorkType workType, ActionTime actionTime)
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)actionTime);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.EQ, (uint)StateType.Idle),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, a, h, b) =>
            {
                //var heroJamAsset = a.ElementAt(0) as HeroJamAsset;
                var heroJamAsset = new HeroJamAsset(a.ElementAt(0))
                {
                    StateType = StateType.Work,
                    StateValue = (byte)workType,
                    StateChangeBlockNumber = b + GetBlockTimeFrom(actionTime)
                };

                return [heroJamAsset];
            };

            return (identifier, rules, function);
        }

        /// <summary>
        /// Get the block time from the action time
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private static uint GetBlockTimeFrom(ActionTime actionTime)
        {
            return actionTime switch
            {
                ActionTime.Short => 1 * HeroJamConstant.Hour,
                ActionTime.Medium => 6 * HeroJamConstant.Hour,
                ActionTime.Long => 12 * HeroJamConstant.Hour,
                _ => throw new NotSupportedException($"Unsupported ActionTime {actionTime}!"),
            };
        }
    }
}