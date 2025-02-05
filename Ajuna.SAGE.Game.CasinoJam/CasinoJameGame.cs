using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Game;
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
        internal static Func<IPlayer, CasinoJamRule, IAsset[], uint, bool> GetVerifyFunction()
        {
            return (player, rule, assets, blocknumber) =>
            {
                switch (rule.CasinoRuleType)
                {
                    case CasinoRuleType.AssetCount:
                        {
                            switch (rule.CasinoRuleOp)
                            {
                                case CasinoRuleOp.EQ:
                                    return assets.Length == BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.GE:
                                    return assets.Length >= BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.GT:
                                    return assets.Length > BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.LT:
                                    return assets.Length < BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.LE:
                                    return assets.Length <= BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.NE:
                                    return assets.Length != BitConverter.ToUInt32(rule.RuleValue);

                                default:
                                    return false;
                            }
                        }

                    case CasinoRuleType.IsOwnerOf:
                        {
                            if (rule.CasinoRuleOp != CasinoRuleOp.Index)
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

                    case CasinoRuleType.IsOwnerOfAll:
                        {
                            if (rule.CasinoRuleOp != CasinoRuleOp.None)
                            {
                                return false;
                            }
                            
                            for (int i = 0; i < assets.Length; i++)
                            {
                                if (!player.IsOwnerOf(assets[i]))
                                {
                                    return false;
                                }
                            }
                                 return true;
                        }

                    case CasinoRuleType.AssetTypeIs:
                        {
                            if (assets.Length == 0)
                            {
                                return false;
                            }

                            if (rule.ValueType == ValueType.None)
                            {
                                return false;
                            }

                            if (assets.Length <= (byte)rule.ValueType)
                            {
                                return false;
                            }

                            var asset = new BaseAsset(assets[(byte)rule.ValueType]);
                            return asset.AssetType == (AssetType)BitConverter.ToUInt32(rule.RuleValue);
                        }

                    case CasinoRuleType.SameExist:
                        {
                            if (player.Assets == null || player.Assets.Count == 0)
                            {
                                return false;
                            }

                            return player.Assets.Any(a => a.MatchType.SequenceEqual(rule.RuleValue));
                        }

                    case CasinoRuleType.SameNotExist:
                        {
                            if (player.Assets == null || player.Assets.Count == 0)
                            {
                                return true;
                            }

                            return !player.Assets.Any(a => a.MatchType.SequenceEqual(rule.RuleValue));
                        }

                    case CasinoRuleType.AssetTypesAt:
                        {
                            if (rule.CasinoRuleOp != CasinoRuleOp.Composite)
                            {
                                return false;
                            }

                            for (int i = 0; i < rule.RuleValue.Length; i++)
                            {
                                byte composite = rule.RuleValue[i];

                                if (composite == 0)
                                {
                                    continue;
                                }

                                byte assetType = (byte)(composite >> 4);
                                byte assetSubType = (byte)(composite & 0x0F);

                                if (assets.Length <= i)
                                {
                                    return false;
                                }

                                var baseAsset = assets[i] as BaseAsset;
                                if (baseAsset == null || (byte)baseAsset.AssetType != assetType || (byte)baseAsset.AssetSubType != assetSubType)
                                {
                                    return false;
                                }
                            }

                            return true;
                        }

                    case CasinoRuleType.ScoreOf:
                        {
                            if (assets.Length == 0)
                            {
                                return false;
                            }

                            if (rule.ValueType == ValueType.None)
                            {
                                return false;
                            }

                            if (assets.Length <= (byte)rule.ValueType)
                            {
                                return false;
                            }

                            var asset = assets[(byte)rule.ValueType];
                            switch (rule.CasinoRuleOp)
                            {
                                case CasinoRuleOp.EQ:
                                    return asset.Score == BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.GE:
                                    return asset.Score >= BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.GT:
                                    return asset.Score > BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.LT:
                                    return asset.Score < BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.LE:
                                    return asset.Score <= BitConverter.ToUInt32(rule.RuleValue);

                                case CasinoRuleOp.NE:
                                    return asset.Score != BitConverter.ToUInt32(rule.RuleValue);

                                default:
                                    return false;
                            }
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

                GetChangeTransition(TokenType.T_1, ValueType.V1),
                GetChangeTransition(TokenType.T_10, ValueType.V1),
                GetChangeTransition(TokenType.T_100, ValueType.V1),
                GetChangeTransition(TokenType.T_1000, ValueType.V1),

                GetGambleTransition(ValueType.V1),
                GetGambleTransition(ValueType.V2),
                GetGambleTransition(ValueType.V3),
                GetGambleTransition(ValueType.V4),

                GetLootTransition()
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
        /// Get Money Change transition set
        /// </summary>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetChangeTransition(TokenType tokenType, ValueType amountType)
        {
            var identifier = CasinoJamIdentifier.Change(tokenType, amountType);
            uint factor = (uint)Math.Pow(10, (uint)tokenType);
            uint value = factor * (uint)amountType;
            byte playerAt = ((byte)AssetType.Player << 4) | (byte)PlayerSubType.None;

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 1),
                new CasinoJamRule(CasinoRuleType.IsOwnerOfAll),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, [playerAt, 0x00, 0x00, 0x00]),
                new CasinoJamRule(CasinoRuleType.ScoreOf, ValueType.V0, CasinoRuleOp.GE, value),
            ];

            ITransitioFee? fee = new TransitioFee(value);

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var player = (PlayerAsset)a.ElementAt(0);

                uint current = player.TokenWallet;

                uint capacity = uint.MaxValue - current;
                uint requested = value;

                uint actual = Math.Min(requested, capacity);

                if (actual <= 0)
                {
                    return [player];
                }

                player.Score -= actual;
                m.Withdraw(player.Id, actual);
                player.TokenWallet += actual;

                return [player];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Gamble transition set
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetGambleTransition(ValueType amountType)
        {
            var identifier = CasinoJamIdentifier.Gamble(0x00, amountType);
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

                var playFee = (uint)amountType;

                // the player does not have enough tokens to gamble
                if (player.TokenWallet < (uint)amountType || bandit.Score > (uint.MaxValue - playFee))
                {
                    return [player, bandit];
                }

                // remove the actual tokens that are gambled from the player and add to the bandit
                player.TokenWallet -= playFee;
                bandit.Score += playFee;
                m.Deposit(bandit.Id, playFee);

                for (int i = 0; i < (byte)amountType; i++)
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

                    bandit.Score -= effectivePayout;
                    m.Withdraw(bandit.Id, effectivePayout);
                    bandit.Token += effectivePayout;
                }

                // 💎 0: DIAMOND
                // 🍒 1: CHERRY
                // 🍊 2: ORANGE
                // 🍋 3: LEMON
                // 🍇 4: GRAPE
                // 🍉 5: WATERMELON
                // 🍀 6: CLOVER
                // 🔔 7: CLOCK 
                // 👑 8: CROWN
                // 💰 9: MONEYBAG

                // 👑 0: CROWN
                // 🍋 1: LEMON
                // 💰 2: MONEYBAG
                // 🍒 3: CHERRY

                return [player, bandit];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Loot transition set
        /// </summary>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetLootTransition()
        {
            var identifier = CasinoJamIdentifier.Loot();
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

                uint capacity = uint.MaxValue - player.TokenWallet;

                // make sure no over flow.
                var banditTokens = Math.Min(bandit.Token, capacity);
                bandit.Token -= banditTokens;

                player.TokenWallet += banditTokens;             

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

                uint capacity = uint.MaxValue - asset.Score;
                if (capacity < fee.Fee)
                {
                    // we can't fund at this point overflow
                    return [asset];
                }

                // deposit funds into the asset
                asset.Score += fee.Fee;
                m.Deposit(asset.Id, fee.Fee);

                return [asset];
            };

            return (identifier, rules, fee, function);
        }
    }
}