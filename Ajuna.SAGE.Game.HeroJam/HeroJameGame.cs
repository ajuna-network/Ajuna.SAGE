using Ajuna.SAGE.Game.Model;
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
            foreach (var (identifier, rules, fee, transition) in rulesAndTransitions)
            {
                engineBuilder.AddTransition(identifier, rules, fee, transition);
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
        private static IEnumerable<(HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>)> GetRulesAndTranstionSets()
        {
            var result = new List<(HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>)>
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
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetCreateHero()
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Create, (byte)AssetType.Hero);
            byte matchType = (byte)AssetType.Hero << 4 + (byte)AssetSubType.None;
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 0),
                new HeroJamRule(HeroRuleType.SameExist, HeroRuleOp.MatchType, matchType),
            ];

            ITransitioFee fee = new TransitioFee(10);

            TransitionFunction<HeroJamRule> function = (r, f, a, h, b) =>
            {
                var asset = new HeroJamAssetBuilder(null, HeroJamUtil.COLLECTION_ID, AssetType.Hero, AssetSubType.None)
                    .SetGeneses(b)
                    .SetEnergy(100)
                    .SetStateType(StateType.Idle)
                    .SetStateChangeBlockNumber(0)
                    .Build();

                asset.Balance.Deposit(fee.Fee);

                return [asset];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get the transition set for the sleep action
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetSleepTransitionSet(ActionTime actionTime)
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Sleep, (byte)actionTime);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.EQ, (uint)StateType.Idle),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, f, a, h, b) =>
            {
                //var heroJamAsset = a.ElementAt(0) as HeroJamAsset;
                var heroJamAsset = new HeroJamAsset(a.ElementAt(0))
                {
                    StateType = StateType.Sleep,
                    StateSubType = (byte) SleepType.Normal,
                    StateSubValue = (byte)actionTime,
                    StateChangeBlockNumber = b + GetBlockTimeFrom(actionTime)
                };

                return [heroJamAsset];
            };

            return (identifier, rules, default, function);
        }

        /// <summary>
        /// Get the transition set for the work action
        /// </summary>
        /// <param name="workType"></param>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetWorkTransitionSet(WorkType workType, ActionTime actionTime)
        {
            var subIdentifier = (byte)workType << 4 + (byte)actionTime;
            var identifier = new HeroJamIdentifier((byte)HeroAction.Work, (byte)subIdentifier);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.EQ, (uint)StateType.Idle),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, f, a, h, b) =>
            {
                var heroJamAsset = new HeroJamAsset(a.ElementAt(0))
                {
                    StateType = StateType.Work,
                    StateSubType = (byte)workType,
                    StateSubValue = (byte)actionTime,
                    StateChangeBlockNumber = b + GetBlockTimeFrom(actionTime)
                };

                return [heroJamAsset];
            };

            return (identifier, rules, default, function);
        }

        /// <summary>
        /// Get the transition set for the sleep action
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetIdleTransitionSet()
        {
            var identifier = new HeroJamIdentifier((byte)HeroAction.Idle, (byte)0);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.AllStateType, HeroRuleOp.NE, (uint)StateType.Idle),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (r, f, a, h, b) =>
            {
                var hero = (HeroJamAsset)a.ElementAt(0);

                int fatigue = HeroJamUtil.GetRessourceFatigue(hero.StateType, hero.StateSubType, hero.StateSubValue);
                int energy = HeroJamUtil.GetRessourceEnergy(hero.StateType, hero.StateSubType, hero.StateSubValue);

                hero.Fatigue = (byte)Math.Clamp(hero.Fatigue + fatigue, 0, 255);
                hero.Energy = (byte)Math.Clamp(hero.Energy + energy, 0, 255);

                switch (hero.StateType)
                {
                    case StateType.Sleep:
                        {
                            // execute other stuff beside fatigue and energy,
                            // maybe some dream stuff, buffs or motivation.
                        }
                        break;

                    case StateType.Work:
                        {
                            // calculate reward based on work type and action time

                        }
                        break;
                }

                //var heroJamAsset = a.ElementAt(0) as HeroJamAsset;
                var heroJamAsset = new HeroJamAsset(a.ElementAt(0))
                {
                    StateType = StateType.Idle,
                    StateSubValue = (byte)0,
                    StateChangeBlockNumber = b
                };

                return [heroJamAsset];
            };

            return (identifier, rules, default, function);
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
                ActionTime.Short => 1 * HeroJamUtil.Hour,
                ActionTime.Medium => 6 * HeroJamUtil.Hour,
                ActionTime.Long => 12 * HeroJamUtil.Hour,
                _ => throw new NotSupportedException($"Unsupported ActionTime {actionTime}!"),
            };
        }
    }
}