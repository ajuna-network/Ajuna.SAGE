using Ajuna.SAGE.Game.Manager;
using Ajuna.SAGE.Game.Model;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.HeroJam.Test")]

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
        private static Func<IPlayer, HeroJamRule, IAsset[], uint, IBalanceManager, bool> GetVerifyFunction()
        {
            return (p, r, a, b, m) =>
            {
                switch (r.RuleType)
                {
                    case (byte)HeroRuleType.AssetCount:
                        {
                            switch (r.RuleOp)
                            {
                                case (byte)HeroRuleOp.EQ:
                                    return a.Length == BitConverter.ToUInt32(r.RuleValue);

                                case (byte)HeroRuleOp.GE:
                                    return a.Length >= BitConverter.ToUInt32(r.RuleValue);

                                case (byte)HeroRuleOp.GT:
                                    return a.Length > BitConverter.ToUInt32(r.RuleValue);

                                case (byte)HeroRuleOp.LT:
                                    return a.Length < BitConverter.ToUInt32(r.RuleValue);

                                case (byte)HeroRuleOp.LE:
                                    return a.Length <= BitConverter.ToUInt32(r.RuleValue);

                                case (byte)HeroRuleOp.NE:
                                    return a.Length != BitConverter.ToUInt32(r.RuleValue);

                                default:
                                    return false;
                            }
                        }

                    case (byte)HeroRuleType.IsOwnerOf:
                        {
                            if (r.RuleOp != (byte)HeroRuleOp.Index)
                            {
                                return false;
                            }
                            var assetIndex = BitConverter.ToUInt32(r.RuleValue);
                            if (a.Length <= assetIndex)
                            {
                                return false;
                            }

                            return p.IsOwnerOf(a[assetIndex]);
                        }

                    case (byte)HeroRuleType.AllAssetType:
                        {
                            return a.All(a =>
                            {
                                BaseAsset? baseAsset = a as BaseAsset;
                                var expectedAssetType = (AssetType)BitConverter.ToUInt32(r.RuleValue);
                                return baseAsset != null && baseAsset.AssetType == expectedAssetType;
                            });
                        }

                    case (byte)HeroRuleType.AllStateType:
                        {
                            return a.All(a =>
                            {
                                HeroAsset? heroJameAsset = a as HeroAsset;
                                var expectedStateType = (StateType)BitConverter.ToUInt32(r.RuleValue);
                                return r.RuleOp switch
                                {
                                    (byte)HeroRuleOp.EQ => heroJameAsset != null && heroJameAsset.StateType == expectedStateType,
                                    (byte)HeroRuleOp.NE => heroJameAsset != null && heroJameAsset.StateType != expectedStateType,
                                    _ => throw new NotSupportedException($"Unsupported RuleOp {r.RuleOp} for RuleType {r.RuleType}!"),
                                };
                            });
                        }

                    case (byte)HeroRuleType.CanStateChange:
                        {
                            if (r.RuleOp != (byte)HeroRuleOp.Index)
                            {
                                return false;
                            }
                            var assetIndex = BitConverter.ToUInt32(r.RuleValue);
                            if (a.Length <= assetIndex)
                            {
                                return false;
                            }
                            return a[assetIndex] is HeroAsset heroJamAsset && heroJamAsset.StateChangeBlockNumber < b;
                        }

                    case (byte)HeroRuleType.SameExist:
                        {
                            if (p.Assets == null || p.Assets.Count == 0)
                            {
                                return false;
                            }

                            return p.Assets.Any(a => a.MatchType.SequenceEqual(r.RuleValue));
                        }

                    case (byte)HeroRuleType.SameNotExist:
                        {
                            if (p.Assets == null || p.Assets.Count == 0)
                            {
                                return true;
                            }

                            return !p.Assets.Any(a => a.MatchType.SequenceEqual(r.RuleValue));
                        }

                    case (byte)HeroRuleType.AssetTypeAt:
                        {
                            if (r.RuleOp != (byte)HeroRuleOp.Composite)
                            {
                                return false;
                            }

                            byte composite = r.RuleValue[0];
                            byte index = (byte)(composite & 0x0F);
                            byte assetType = (byte)(composite >> 4);

                            if (a.Length <= index)
                            {
                                return false;
                            }
                            // We assume all assets are HeroJamAsset for this game.
                            var baseAsset = a[index] as BaseAsset;
                            return baseAsset != null && ((uint)baseAsset.AssetType) == assetType;
                        }

                    case (byte)HeroRuleType.AssetFlagAt:
                        {
                            if (r.RuleOp != (byte)HeroRuleOp.Composite)
                            {
                                return false;
                            }

                            byte composite = r.RuleValue[0];
                            byte index = (byte)(composite & 0x0F);
                            byte flagIndex = (byte)(composite >> 4);

                            if (a.Length <= index)
                            {
                                return false;
                            }
                            // We assume all assets are HeroJamAsset for this game.
                            var baseAsset = a[index] as BaseAsset;
                            return baseAsset.AssetFlags[flagIndex];
                        }

                    default:
                        throw new NotSupportedException($"Unsupported RuleType {r.RuleType}!");
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
                GetCreateMap(),

                GetSleepTransitionSet(SleepType.Normal, ActionTime.Short),
                GetSleepTransitionSet(SleepType.Normal, ActionTime.Medium),
                GetSleepTransitionSet(SleepType.Normal, ActionTime.Long),

                GetWorkTransitionSet(WorkType.Hunt, ActionTime.Short),
                GetWorkTransitionSet(WorkType.Hunt, ActionTime.Medium),
                GetWorkTransitionSet(WorkType.Hunt, ActionTime.Long),

                GetUseTransitionSet(UseType.Disassemble),
                GetUseTransitionSet(UseType.Consume),
            };

            return result;
        }

        /// <summary>
        /// Get the transition set for the create hero action
        /// </summary>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetCreateHero()
        {
            var identifier = HeroJamIdentifier.Create(AssetType.Hero, AssetSubType.None);
            byte matchType = (byte)AssetType.Hero << 4 + (byte)AssetSubType.None;

            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 0),
                new HeroJamRule(HeroRuleType.SameNotExist, HeroRuleOp.MatchType, matchType),
            ];

            ITransitioFee fee = new TransitioFee(10);

            TransitionFunction<HeroJamRule> function = (e, r, f, a, h, b, m) =>
            {
                var baseAsset = new BaseAssetBuilder(null, HeroJamUtil.COLLECTION_ID, AssetType.Hero, AssetSubType.None)
                    .SetGenesis(b)
                    .Build();

                var hero = new HeroAsset(baseAsset)
                {
                    LocationX = 0,
                    LocationY = 0,
                    Energy = 100,
                    StateType = StateType.None,
                    StateChangeBlockNumber = 0,
                };

                m.Deposit(hero.Id, fee.Fee);

                return [hero];
            };

            return (identifier, rules, fee, function);
        }

        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetCreateMap()
        {
            var identifier = HeroJamIdentifier.Create(AssetType.Item, (AssetSubType)ItemSubType.Map);
            byte matchType = (byte)AssetType.Item << 4 + (byte)(AssetSubType)ItemSubType.Map;
            byte heroAt0 = ((byte)AssetType.Hero << 4) | 0;

            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AssetTypeAt, HeroRuleOp.Composite, [ heroAt0, 0x00, 0x00, 0x00 ]),
                new HeroJamRule(HeroRuleType.SameNotExist, HeroRuleOp.MatchType, matchType),
            ];

            TransitionFunction<HeroJamRule> function = (e, r, f, a, h, b, m) =>
            {
                var baseAsset = new BaseAssetBuilder(null, HeroJamUtil.COLLECTION_ID, AssetType.Item, (AssetSubType)ItemSubType.Map)
                    .SetGenesis(b)
                    .Build();

                var hero = (HeroAsset)a.ElementAt(0);
                hero.LocationX = h[0];
                hero.LocationY = h[1];

                var map = new MapAsset(baseAsset)
                {
                    TargetX = 0,
                    TargetY = 0,
                };

                return [hero, map];
            };

            return (identifier, rules, default, function);
        }

        /// <summary>
        /// Get the transition set for the sleep action
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetSleepTransitionSet(SleepType sleepType, ActionTime actionTime)
        {
            var identifier = HeroJamIdentifier.Sleep(sleepType, actionTime);
            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AllAssetType, HeroRuleOp.EQ, (uint)AssetType.Hero),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (e, r, f, a, h, b, m) =>
            {
                var hero = (HeroAsset)a.ElementAt(0);

                hero = HeroJamUtil.GetAssetStateTransition(hero, h, out Asset[] assets);

                // create a new element to return
                var heroJamAsset = new HeroAsset(hero)
                {
                    StateType = StateType.Sleep,
                    StateSubType = (byte)SleepType.Normal,
                    StateSubValue = (byte)actionTime,
                    StateChangeBlockNumber = b + HeroJamUtil.GetBlockTimeFrom(actionTime)
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
            var identifier = HeroJamIdentifier.Work(workType, actionTime);
            byte heroAt0 = ((byte)AssetType.Hero << 4) | 0;

            HeroJamRule[] rules = [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 1),
                new HeroJamRule(HeroRuleType.AssetTypeAt, HeroRuleOp.Composite, [heroAt0, 0x00, 0x00, 0x00]),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            TransitionFunction<HeroJamRule> function = (e, r, f, a, h, b, m) =>
            {
                var hero = (HeroAsset)a.ElementAt(0);

                hero = HeroJamUtil.GetAssetStateTransition(hero, h, out Asset[] assets);

                switch (workType)
                {
                    case WorkType.Hunt:
                        // ... do something
                        break;

                    default:
                        throw new NotSupportedException($"Unsupported WorkType {workType}!");
                }

                // create a new element to return
                var heroJamAsset = new HeroAsset(hero)
                {
                    StateType = StateType.Work,
                    StateSubType = (byte)workType,
                    StateSubValue = (byte)actionTime,
                    StateChangeBlockNumber = b + HeroJamUtil.GetBlockTimeFrom(actionTime)
                };

                var list = new List<Asset>() { heroJamAsset };
                if (assets != null)
                {
                    list.AddRange(assets);
                }

                return [heroJamAsset];
            };

            return (identifier, rules, default, function);
        }

        /// <summary>
        /// Get the transition set for the use action
        /// </summary>
        /// <returns></returns>
        private static (HeroJamIdentifier, HeroJamRule[], ITransitioFee?, TransitionFunction<HeroJamRule>) GetUseTransitionSet(UseType useType)
        {
            var identifier = HeroJamIdentifier.Use(useType);
            byte heroAt0 = ((byte)AssetType.Hero << 4) | 0;
            byte isUseTypeAt1 = (byte)(((byte)useType << 4) | 1);

            HeroJamRule[] rules =
            [
                new HeroJamRule(HeroRuleType.AssetCount, HeroRuleOp.EQ, 2),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 0),
                new HeroJamRule(HeroRuleType.AssetTypeAt, HeroRuleOp.Composite, [ heroAt0, 0x00, 0x00, 0x00 ]),
                new HeroJamRule(HeroRuleType.IsOwnerOf, HeroRuleOp.Index, 1),
                new HeroJamRule(HeroRuleType.AssetFlagAt, HeroRuleOp.Composite, [isUseTypeAt1, 0x00, 0x00, 0x00 ]),
                new HeroJamRule(HeroRuleType.CanStateChange, HeroRuleOp.Index, 0)
            ];

            ITransitioFee fee = default;

            TransitionFunction<HeroJamRule> function = (e, r, f, a, h, b, m) =>
            {
                var hero = (HeroAsset)a.ElementAt(0);

                // Retrieve the hero and the animal asset.
                switch (useType)
                {

                    case UseType.Disassemble:
                        {
                            var usable = (DisassemblableAsset)a.ElementAt(1);
                            return HeroJamUtil.Disassemble(hero, usable);
                        }
                    case UseType.Consume:
                        {
                            var consume = (ConsumableAsset)a.ElementAt(1);
                            return HeroJamUtil.Consume(hero, consume);
                        }

                    default:
                        throw new NotSupportedException($"Unsupported UseType {useType}!");
                }

            };

            return (identifier, rules, fee, function);
        }
    }
}