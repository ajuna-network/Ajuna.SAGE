using Ajuna.SAGE.Game.Model;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.CasinoJam.Test")]

namespace Ajuna.SAGE.Game.CasinoJam
{
    public class CasinoJameGame
    {
        /// <summary>
        /// Create an instance of the HeroJam game engine
        /// </summary>
        /// <param name="blockchainInfoProvider"></param>
        /// <returns></returns>
        public static Engine<CasinoJamIdentifier, CasinoJamRule> Create(IBlockchainInfoProvider blockchainInfoProvider)
        {
            var engineBuilder = new EngineBuilder<CasinoJamIdentifier, CasinoJamRule>(blockchainInfoProvider);

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
        internal static Func<IPlayer, CasinoJamRule, IAsset[], uint, IAssetBalanceManager, bool> GetVerifyFunction()
        {
            return (p, r, a, b, m) =>
            {
                switch (r.CasinoRuleType)
                {
                    case CasinoRuleType.AssetCount:
                        {
                            switch (r.CasinoRuleOp)
                            {
                                case CasinoRuleOp.EQ:
                                    return a.Length == BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.GE:
                                    return a.Length >= BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.GT:
                                    return a.Length > BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.LT:
                                    return a.Length < BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.LE:
                                    return a.Length <= BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.NE:
                                    return a.Length != BitConverter.ToUInt32(r.RuleValue);

                                default:
                                    return false;
                            }
                        }

                    case CasinoRuleType.IsOwnerOf:
                        {
                            if (r.CasinoRuleOp != CasinoRuleOp.Index)
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

                    case CasinoRuleType.IsOwnerOfAll:
                        {
                            if (r.CasinoRuleOp != CasinoRuleOp.None)
                            {
                                return false;
                            }

                            for (int i = 0; i < a.Length; i++)
                            {
                                if (!p.IsOwnerOf(a[i]))
                                {
                                    return false;
                                }
                            }
                            return true;
                        }

                    case CasinoRuleType.AssetTypeIs:
                        {
                            if (a.Length == 0)
                            {
                                return false;
                            }

                            if (r.ValueType == ValueType.None)
                            {
                                return false;
                            }

                            if (a.Length <= (byte)r.ValueType)
                            {
                                return false;
                            }

                            var asset = new BaseAsset(a[(byte)r.ValueType]);
                            return asset.AssetType == (AssetType)BitConverter.ToUInt32(r.RuleValue);
                        }

                    case CasinoRuleType.SameExist:
                        {
                            if (p.Assets == null || p.Assets.Count == 0)
                            {
                                return false;
                            }

                            return p.Assets.Any(a => a.MatchType.SequenceEqual(r.RuleValue));
                        }

                    case CasinoRuleType.SameNotExist:
                        {
                            if (p.Assets == null || p.Assets.Count == 0)
                            {
                                return true;
                            }

                            return !p.Assets.Any(a => a.MatchType.SequenceEqual(r.RuleValue));
                        }

                    case CasinoRuleType.AssetTypesAt:
                        {
                            if (r.CasinoRuleOp != CasinoRuleOp.Composite)
                            {
                                return false;
                            }

                            for (int i = 0; i < r.RuleValue.Length; i++)
                            {
                                byte composite = r.RuleValue[i];

                                if (composite == 0)
                                {
                                    continue;
                                }

                                byte assetType = (byte)(composite >> 4);
                                byte assetSubType = (byte)(composite & 0x0F);

                                if (a.Length <= i)
                                {
                                    return false;
                                }

                                var baseAsset = a[i] as BaseAsset;
                                if (baseAsset == null || (byte)baseAsset.AssetType != assetType || (byte)baseAsset.AssetSubType != assetSubType)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }

                    case CasinoRuleType.BalanceOf:
                        {
                            if (a.Length == 0)
                            {
                                return false;
                            }

                            if (r.ValueType == ValueType.None)
                            {
                                return false;
                            }

                            if (a.Length <= (byte)r.ValueType)
                            {
                                return false;
                            }

                            var asset = a[(byte)r.ValueType];
                            var balance = m.AssetBalance(asset.Id);

                            if (!balance.HasValue)
                            {
                                return false;
                            }

                            switch (r.CasinoRuleOp)
                            {
                                case CasinoRuleOp.EQ:
                                    return balance.Value == BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.GE:
                                    return balance.Value >= BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.GT:
                                    return balance.Value > BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.LT:
                                    return balance.Value < BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.LE:
                                    return balance.Value <= BitConverter.ToUInt32(r.RuleValue);

                                case CasinoRuleOp.NE:
                                    return balance.Value != BitConverter.ToUInt32(r.RuleValue);

                                default:
                                    return false;
                            }
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
        internal static IEnumerable<(CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>)> GetRulesAndTranstionSets()
        {
            var result = new List<(CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>)>
            {
                GetCreatePlayerTransition(),
                GetFundTransition(AssetType.Player, TokenType.T_1),
                GetFundTransition(AssetType.Player, TokenType.T_10),
                GetFundTransition(AssetType.Player, TokenType.T_100),
                GetFundTransition(AssetType.Player, TokenType.T_1000),

                GetFundTransition(AssetType.Machine, TokenType.T_1000),
                GetFundTransition(AssetType.Machine, TokenType.T_10000),
                GetFundTransition(AssetType.Machine, TokenType.T_100000),
                GetFundTransition(AssetType.Machine, TokenType.T_1000000),

                GetCreateMachineTransition(MachineSubType.Bandit),

                GetGambleTransition(ValueType.V1),
                GetGambleTransition(ValueType.V2),
                GetGambleTransition(ValueType.V3),
                GetGambleTransition(ValueType.V4),

                GetLootTransition(TokenType.T_1),
                GetLootTransition(TokenType.T_10),
                GetLootTransition(TokenType.T_100),
                GetLootTransition(TokenType.T_1000),
                GetLootTransition(TokenType.T_10000),
                GetLootTransition(TokenType.T_100000),
                GetLootTransition(TokenType.T_1000000),
            };

            return result;
        }

        /// <summary>
        /// Get Create Player transition set
        /// </summary>
        /// <returns></returns>
        internal static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetCreatePlayerTransition()
        {
            var identifier = CasinoJamIdentifier.Create(AssetType.Player, AssetSubType.None);
            byte matchType = (byte)AssetType.Player << 4 + (byte)AssetSubType.None;

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 0u),
                new CasinoJamRule(CasinoRuleType.SameNotExist, CasinoRuleOp.MatchType, matchType),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                // initiate the player
                var asset = new PlayerAsset(b);

                return [asset];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Create Machine transition set
        /// </summary>
        /// <returns></returns>
        internal static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetCreateMachineTransition(MachineSubType machineSubType)
        {
            var assetType = AssetType.Machine;
            var identifier = CasinoJamIdentifier.Create(assetType, (AssetSubType)machineSubType);
            byte matchType = (byte)((byte)assetType << 4 + (byte)machineSubType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 0u),
                new CasinoJamRule(CasinoRuleType.SameNotExist, CasinoRuleOp.MatchType, matchType),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                // initiate the bandit machine
                var asset = new BanditAsset(b);

                return [asset];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Gamble transition set
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetGambleTransition(ValueType valueType)
        {
            var identifier = CasinoJamIdentifier.Gamble(0x00, valueType);
            byte playerAt = ((byte)AssetType.Player << 4) | (byte)PlayerSubType.None;
            byte banditAt = ((byte)AssetType.Machine << 4) | (byte)MachineSubType.Bandit;

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 2),
                new CasinoJamRule(CasinoRuleType.IsOwnerOfAll),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, [playerAt, banditAt, 0x00, 0x00 ]),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var player = new PlayerAsset(a.ElementAt(0));
                var bandit = new BanditAsset(a.ElementAt(1));

                bandit.SlotAResult = 0;
                bandit.SlotBResult = 0;
                bandit.SlotCResult = 0;
                bandit.SlotDResult = 0;

                var playFee = (uint)valueType;

                // player needs to be able to pay fee and bandit needs to be able to receive reward
                if (!m.CanWithdraw(player.Id, playFee, out _) || !m.CanDeposit(bandit.Id, playFee, out _))
                {
                    return [player, bandit];
                }

                // remove the actual tokens that are gambled from the player and add to the bandit
                m.Withdraw(player.Id, playFee);
                m.Deposit(bandit.Id, playFee);

                // TODO: Need to make sure that we don't allow play if the machine can't execute the max reward possible

                for (int i = 0; i < (byte)valueType; i++)
                {
                    var offset = (uint)(i * 5);
                    var Slot1 = (byte)(h[0 + offset] % 10);
                    var Slot2 = (byte)(h[1 + offset] % 10);
                    var Slot3 = (byte)(h[2 + offset] % 10);
                    var Bonus1 = (byte)(h[3 + offset] % 4);
                    var Bonus2 = (byte)(h[4 + offset] % 4);

                    ushort packed = CasinoJamUtil.PackSlotResult(Slot1, Slot2, Slot3, Bonus1, Bonus2);
                    switch (i)
                    {
                        case 0:
                            bandit.SlotAResult = packed;
                            break;

                        case 1:
                            bandit.SlotBResult = packed;
                            break;

                        case 2:
                            bandit.SlotCResult = packed;
                            break;

                        case 3:
                            bandit.SlotDResult = packed;
                            break;
                    }

                    uint finalReward = CasinoJamUtil.SlotReward(Slot1, Slot2, Slot3, Bonus1, Bonus2);

                    var effectivePayout = Math.Min(finalReward, bandit.Score);

                    if (!m.CanWithdraw(bandit.Id, effectivePayout, out _) || !m.CanDeposit(player.Id, effectivePayout, out _))
                    {
                        // TODO: if the bandit can't pay the reward, then the player gets the fee back, but it also is a very problematic case
                        return [player, bandit];
                    }

                    m.Withdraw(bandit.Id, effectivePayout);
                    m.Deposit(player.Id, effectivePayout);
                }

                return [player, bandit];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Loot transition set
        /// </summary>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetLootTransition(TokenType tokenType)
        {
            var identifier = CasinoJamIdentifier.Loot(tokenType);
            byte playerAt = ((byte)AssetType.Player << 4) | (byte)PlayerSubType.None;
            byte banditAt = ((byte)AssetType.Machine << 4) | (byte)MachineSubType.Bandit;

            var value = (uint)Math.Pow(10, (byte)tokenType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 2),
                new CasinoJamRule(CasinoRuleType.IsOwnerOfAll),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, [playerAt, banditAt, 0x00, 0x00 ]),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var player = new PlayerAsset(a.ElementAt(0));
                var bandit = new BanditAsset(a.ElementAt(1));

                bandit.SlotAResult = 0;
                bandit.SlotBResult = 0;
                bandit.SlotCResult = 0;
                bandit.SlotDResult = 0;

                if (m.CanDeposit(player.Id, value, out _) && m.Withdraw(bandit.Id, value))
                {
                    m.Deposit(player.Id, value);
                }

                return [player, bandit];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Fund AssetType transition set
        /// </summary>
        /// <returns></returns>
        internal static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetFundTransition(AssetType assetType, TokenType tokenType)
        {
            var identifier = CasinoJamIdentifier.Fund(assetType, tokenType);
            uint matchType = Convert.ToUInt32(assetType);
            uint value = (uint)Math.Pow(10, (byte)tokenType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 1u),
                new CasinoJamRule(CasinoRuleType.AssetTypeIs, ValueType.V0, CasinoRuleOp.MatchType, matchType),
                new CasinoJamRule(CasinoRuleType.IsOwnerOfAll),
            ];

            ITransitioFee fee = new TransitioFee(value);

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var asset = new BaseAsset(a.ElementAt(0));

                m.Deposit(asset.Id, fee.Fee);

                return [asset];
            };

            return (identifier, rules, fee, function);
        }
    }
}