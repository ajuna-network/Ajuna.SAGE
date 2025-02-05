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

                            if (r.ValueType == MultiplierType.None)
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

                            if (r.ValueType == MultiplierType.None)
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

                GetGambleTransition(MultiplierType.V1),
                GetGambleTransition(MultiplierType.V2),
                GetGambleTransition(MultiplierType.V3),
                GetGambleTransition(MultiplierType.V4),

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
                asset.Value1Factor = TokenType.T_1;
                asset.Value1Multiplier = MultiplierType.V1;
                return [asset];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Gamble transition set
        /// </summary>
        /// <param name="actionTime"></param>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetGambleTransition(MultiplierType valueType)
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

                var spinTimes = (byte)valueType;

                var value1 = (uint)Math.Pow(10, (byte)bandit.Value1Factor) * (byte)bandit.Value1Multiplier;
                var maxReward2 = (uint)Math.Pow(10, (byte)bandit.Value2Factor) * (byte)bandit.Value2Multiplier;
                var maxReward3 = (uint)Math.Pow(10, (byte)bandit.Value3Factor) * (byte)bandit.Value3Multiplier;

                // calculate minimum of funds required for the bandit to pay the fix max rewards possible
                uint minReward = value1;
                uint jackMaxReward = maxReward2;
                uint specMaxReward = maxReward3;

                var spinMaxReward = minReward * 8192;
                var maxReward = (spinMaxReward * spinTimes) + specMaxReward;

                // TODO: (implement) this should be verified and flagged on the asset
                if (!m.CanWithdraw(bandit.Id, maxReward, out _))
                {
                    return [player, bandit];
                }

                // TODO: (implement) this should be verified and flagged on the asset
                if (!m.CanDeposit(player.Id, maxReward, out _))
                {
                    return [player, bandit];
                }

                FullSpin spins = CasinoJamUtil.Spins(spinTimes, minReward, jackMaxReward, specMaxReward, h);
                
                uint reward = 0;
                try
                {
                    reward = checked((uint)spins.SpinResults.Sum(s => s.Reward) 
                        + spins.JackPotReward 
                        + spins.SpecialReward);
                }
                catch (OverflowException)
                {
                    // TODO: (verify) Overflow detected; handle by aborting the play.
                    return [player, bandit];
                }

                if (!m.CanWithdraw(bandit.Id, reward, out _) || !m.CanDeposit(player.Id, reward, out _))
                {
                    // TODO: (verify) Bandit is not able to pay the reward
                    return [player, bandit];
                }

                // pay fees now as we know we can
                m.Withdraw(player.Id, playFee);
                m.Deposit(bandit.Id, playFee);

                for (byte i = 0; i < spins.SpinResults.Length; i++)
                {
                    bandit.SetSlot(i, spins.SpinResults[i].Packed);
                }

                m.Withdraw(bandit.Id, reward);
                m.Deposit(player.Id, reward);

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
                new CasinoJamRule(CasinoRuleType.AssetTypeIs, MultiplierType.V0, CasinoRuleOp.MatchType, matchType),
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