using Ajuna.SAGE.Game.CasinoJam.Model;
using Ajuna.SAGE.Game.Manager;
using Ajuna.SAGE.Game.Model;
using System.Reflection.PortableExecutable;
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
        internal static Func<IPlayer, CasinoJamRule, IAsset[], uint, IBalanceManager, bool> GetVerifyFunction()
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
                                if (baseAsset == null 
                                || (byte)baseAsset.AssetType != assetType 
                                || (assetSubType != (byte)AssetSubType.None && (byte)baseAsset.AssetSubType != assetSubType))
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

                GetRentTransition(AssetType.Seat, AssetSubType.None, MultiplierType.V1),
                GetReserveTransition(AssetType.Seat, AssetSubType.None, MultiplierType.V1),
            };

            return result;
        }

        /// <summary>
        /// Get Create Player transition set
        /// </summary>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetCreatePlayerTransition()
        {
            var identifier = CasinoJamIdentifier.Create(AssetType.Player, (AssetSubType)PlayerSubType.Human);
            byte matchType = CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Human);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 0u),
                new CasinoJamRule(CasinoRuleType.SameNotExist, CasinoRuleOp.MatchType, matchType),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                // initiate the player
                var human = new HumanAsset(b);
                var tracker = new TrackerAsset(b);

                return [human, tracker];
            };

            return (identifier, rules, fee, function);
        }

        /// <summary>
        /// Get Create Machine transition set
        /// </summary>
        /// <returns></returns>
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetCreateMachineTransition(MachineSubType machineSubType)
        {
            var assetType = AssetType.Machine;
            var identifier = CasinoJamIdentifier.Create(assetType, (AssetSubType)machineSubType);
            byte matchType = CasinoJamUtil.MatchType(assetType, (AssetSubType)machineSubType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 0u),
                new CasinoJamRule(CasinoRuleType.SameNotExist, CasinoRuleOp.MatchType, matchType),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                // initiate the bandit machine
                var bandit = new BanditAsset(b)
                {
                    SeatLinked = 0,
                    SeatLimit = 1,
                    MaxSpins = 4,
                    Value1Factor = TokenType.T_1,
                    Value1Multiplier = MultiplierType.V1,
                    Value2Factor = TokenType.T_1,
                    Value2Multiplier = MultiplierType.V0,
                    Value3Factor = TokenType.T_1,
                    Value3Multiplier = MultiplierType.V0,
                };
                return [bandit];
            };

            return (identifier, rules, fee, function);
        }

        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetRentTransition(AssetType assetType, AssetSubType assetSubType, MultiplierType multiplierType)
        {
            var identifier = CasinoJamIdentifier.Rent(assetType, assetSubType, multiplierType);
            byte machineAt = CasinoJamUtil.MatchType(AssetType.Machine);
            uint seatFee = CasinoJamUtil.BASE_SEAT_FEE * (uint)multiplierType;

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 1u),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, machineAt),
                new CasinoJamRule(CasinoRuleType.IsOwnerOf, CasinoRuleOp.Index, 0),
                // TODO: (verify) check can add seat is done in transition ???
            ];

            ITransitioFee? fee = new TransitioFee(seatFee);

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var machine = new MachineAsset(a.ElementAt(0));
                
                // maximum seats linked already reached
                if (machine.SeatLinked >= machine.SeatLimit)
                {
                    return [machine];
                }

                // add new linked machine
                machine.SeatLinked++;

                var seat = new SeatAsset(b)
                {
                    SeatValidityPeriod = (ushort)(10 * 60 * (byte)multiplierType),
                    PlayerFee = 1,
                    PlayerGracePeriod = 30,
                    ReservationStartBlock = 0,
                    ReservationDuration = 0,
                    LastActionBlock = 0,
                    PlayerActionCount = 0,
                    PlayerId = 0,
                    MachineId = machine.Id
                };

                return [machine, seat];
            };

            return (identifier, rules, fee, function);
        }

        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetReserveTransition(AssetType assetType, AssetSubType assetSubType, MultiplierType multiplierType)
        {
            var identifier = CasinoJamIdentifier.Reserve(assetType, assetSubType, multiplierType);
            byte humanAt = CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Human);
            byte seatAt = CasinoJamUtil.MatchType(AssetType.Seat);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 2u),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, humanAt, seatAt),
                new CasinoJamRule(CasinoRuleType.IsOwnerOf, CasinoRuleOp.Index, 0),
                // TODO: (verify) check if seat is empty and usable in transition ???
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var human = new HumanAsset(a.ElementAt(0));
                var seat = new SeatAsset(a.ElementAt(1));

                var result = new IAsset[] { human, seat };

                // seat is already reserved
                if (seat.PlayerId != 0)
                {
                    return result;
                }

                var reservationFee = seat.PlayerFee * (uint)multiplierType;

                // TODO: (implement) this should be verified and flagged on the asset
                if (!m.CanWithdraw(human.Id, reservationFee, out _))
                {
                    return result;
                }

                // TODO: (implement) this should be verified and flagged on the asset
                if (!m.CanDeposit(seat.Id, reservationFee, out _))
                {
                    return result;
                }

                // pay reservation fee now as we know we can
                m.Withdraw(human.Id, reservationFee);
                m.Deposit(seat.Id, reservationFee);

                seat.PlayerId = human.Id;
                seat.ReservationStartBlock = b;
                seat.ReservationDuration = (ushort)(30 * (ushort)multiplierType);
                seat.LastActionBlock = 0;
                seat.PlayerActionCount = 0;

                return result;
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
            byte playerAt = CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Human);
            byte trackerAt = CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Tracker);
            byte seatAt = CasinoJamUtil.MatchType(AssetType.Seat);
            byte banditAt = CasinoJamUtil.MatchType(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 4),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, [playerAt, trackerAt, seatAt, banditAt ]),
                new CasinoJamRule(CasinoRuleType.IsOwnerOf, CasinoRuleOp.Index, 0), // own Player
                new CasinoJamRule(CasinoRuleType.IsOwnerOf, CasinoRuleOp.Index, 1), // own Tracker
                // TODO: (verify) we currently check if the player owns the seat and it's the correct machine only in the transition 
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var player = new HumanAsset(a.ElementAt(0));
                var tracker = new TrackerAsset(a.ElementAt(1));
                var seat = new SeatAsset(a.ElementAt(2));
                var bandit = new BanditAsset(a.ElementAt(3));

                var result = new IAsset[] { player, tracker, seat, bandit };

                // TODO: (verify) that max spins resides on bandit asset, and implies cleanup of the tracker asset
                tracker.LastReward = 0;
                for (byte i = 0; i < bandit.MaxSpins; i++)
                {
                    tracker.SetSlot(i, 0);
                }

                var playFee = (uint)valueType;

                // player needs to be able to pay fee and bandit needs to be able to receive reward
                if (!m.CanWithdraw(player.Id, playFee, out _) || !m.CanDeposit(bandit.Id, playFee, out _))
                {
                    return result;
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
                    return result;
                }

                // TODO: (implement) this should be verified and flagged on the asset
                if (!m.CanDeposit(player.Id, maxReward, out _))
                {
                    return result;
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
                    return result;
                }

                if (!m.CanWithdraw(bandit.Id, reward, out _) || !m.CanDeposit(player.Id, reward, out _))
                {
                    // TODO: (verify) Bandit is not able to pay the reward
                    return result;
                }

                // pay fees now as we know we can
                m.Withdraw(player.Id, playFee);
                m.Deposit(bandit.Id, playFee);

                for (byte i = 0; i < spins.SpinResults.Length; i++)
                {
                    tracker.SetSlot(i, spins.SpinResults[i].Packed);
                }

                m.Withdraw(bandit.Id, reward);
                m.Deposit(player.Id, reward);

                return result;
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
            byte playerAt = CasinoJamUtil.MatchType(AssetType.Player, (AssetSubType)PlayerSubType.Human);
            byte banditAt = CasinoJamUtil.MatchType(AssetType.Machine, (AssetSubType)MachineSubType.Bandit);

            var value = (uint)Math.Pow(10, (byte)tokenType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 2),
                new CasinoJamRule(CasinoRuleType.IsOwnerOfAll),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, playerAt, banditAt),
            ];

            ITransitioFee? fee = default;

            TransitionFunction<CasinoJamRule> function = (r, f, a, h, b, m) =>
            {
                var player = new HumanAsset(a.ElementAt(0));
                var bandit = new BanditAsset(a.ElementAt(1));

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
        private static (CasinoJamIdentifier, CasinoJamRule[], ITransitioFee?, TransitionFunction<CasinoJamRule>) GetFundTransition(AssetType assetType, TokenType tokenType)
        {
            var identifier = CasinoJamIdentifier.Fund(assetType, tokenType);
            byte assetTypeAt = CasinoJamUtil.MatchType(assetType);
            uint value = (uint)Math.Pow(10, (byte)tokenType);

            CasinoJamRule[] rules = [
                new CasinoJamRule(CasinoRuleType.AssetCount, CasinoRuleOp.EQ, 1u),
                new CasinoJamRule(CasinoRuleType.AssetTypesAt, CasinoRuleOp.Composite, assetTypeAt),
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