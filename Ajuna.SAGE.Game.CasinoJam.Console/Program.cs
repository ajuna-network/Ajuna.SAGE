using Ajuna.SAGE.Game.CasinoJam.Model;
using System.Security.Cryptography;

namespace Ajuna.SAGE.Game.CasinoJam
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(GetWeights(10)));

            Probabilities(1_000_000);
        }

        public static void Probabilities(int totalSpins)
        {
            int winningSpins = 0;
            ulong totalReward = 0;

            // Counters for specific patterns:
            uint threeKindCount = 0;
            uint threeKindRewards = 0;
            uint onePairCount = 0;
            uint onePairRewards = 0;
            uint fullHouseCount = 0;
            uint fullHouseRewards = 0;
            uint royalFlushCount = 0;
            uint royalFlushRewards = 0;

            // symbol occurances
            Dictionary<byte, int> symbolOccurance = [];
            for (byte i = 0; i < 10; i++)
                symbolOccurance[i] = 0;

            // Count occurrence of each symbol when all three slots are equal.
            Dictionary<byte, uint[]> threeKind = [];
            for (byte i = 0; i < 10; i++)
                threeKind[i] = [0, 0];

            // Count occurrences for each bonus value when Bonus1 equals Bonus2.
            Dictionary<byte, uint[]> onePair = [];
            for (byte i = 0; i < 10; i++)
                onePair[i] = [0, 0];

            // Count occurrences for each symbol when all three slots are equal and both bonuses are equal, bot not the same.
            Dictionary<byte, uint[]> fullHouse = [];
            for (byte i = 0; i < byte.MaxValue; i++)
                fullHouse[i] = [0, 0];

            // Count occurrences for each symbol when all three slots and both bonuses are equal.
            Dictionary<byte, uint[]> royalFlush = [];
            for (byte i = 0; i < 10; i++)
                royalFlush[i] = [0, 0];

            // List to track rewards for distribution analysis.
            List<uint> rewards = new(totalSpins);

            var bandit = new BanditAsset(1, 1)
            {
                MaxSpins = 4,
                Value1Factor = TokenType.T_1,
                Value1Multiplier = MultiplierType.V1,
                Jackpot = 0
            };

            for (int i = 0; i < totalSpins; i++)
            {
                // Generate random bytes for one spin (5 bytes per spin: 3 for slots, 2 for bonuses)
                FullSpin fullSpin = bandit.Spins(1, RandomNumberGenerator.GetBytes(5));
                SpinResult spin = fullSpin.SpinResults[0];

                var slotsMatch = spin.Slot1 != 0 && spin.Slot1 == spin.Slot2 && spin.Slot2 == spin.Slot3;
                var bonusMatch = spin.Bonus1 != 0 && spin.Bonus1 == spin.Bonus2;
                var shifted = (byte)((spin.Slot1 << 4) + spin.Bonus1);

                symbolOccurance[spin.Slot1]++;
                symbolOccurance[spin.Slot2]++;
                symbolOccurance[spin.Slot3]++;
                symbolOccurance[spin.Bonus1]++;
                symbolOccurance[spin.Bonus2]++;

                // Record win information
                if (spin.Reward > 0)
                {
                    winningSpins++;
                }

                totalReward += spin.Reward;
                rewards.Add(spin.Reward);

                if (slotsMatch && bonusMatch)
                {
                    if (spin.Slot1 == spin.Bonus1)
                    {
                        // Full line win
                        royalFlushCount++;
                        royalFlush[spin.Slot1][0]++;
                        royalFlush[spin.Slot1][1] += spin.Reward;
                        royalFlushRewards += spin.Reward;
                    }
                    else
                    {
                        fullHouseCount++;
                        fullHouse[shifted][0]++;
                        fullHouse[shifted][1] += spin.Reward;
                        fullHouseRewards += spin.Reward;
                    }
                }
                else if (slotsMatch)
                {
                    // All slots are equal
                    threeKindCount++;
                    threeKind[spin.Slot1][0]++;
                    threeKind[spin.Slot1][1] += spin.Reward;
                    threeKindRewards += spin.Reward;
                }
                else if (bonusMatch)
                {
                    // Bonus values are equal
                    onePairCount++;
                    onePair[spin.Bonus1][0]++;
                    onePair[spin.Bonus1][1] += spin.Reward;
                    onePairRewards += spin.Reward;
                }
            }

            // Calculate overall statistics
            double winProbability = (double)winningSpins / totalSpins;
            double rewardToEarn = (double)totalReward / totalSpins;
            double averageReward = (double)totalReward / totalSpins;
            uint minReward = rewards.Min();
            uint maxReward = rewards.Max();
            double medianReward = CalculateMedian(rewards);

            // Log the detailed statistics.
            Console.WriteLine($"{totalSpins,11:N0} Spins done!");
            Console.WriteLine($"{winningSpins,11:N0} Winners [{winProbability,7:P2}]");
            Console.WriteLine($"{totalReward,11:N0} Payouts [{rewardToEarn,7:P2}]");
            Console.WriteLine($"{averageReward,11:F2}  Ø - Reward");
            Console.WriteLine($"{minReward,11:N0} min. Reward");
            Console.WriteLine($"{maxReward,11:N0} max. Reward");
            Console.WriteLine($"{medianReward,11:F2} med. Reward");

            Console.WriteLine($"\n- ONE PAIR ----- [{(double)onePairCount / totalSpins,8:P4}] {onePairCount,10} Spins {onePairRewards,10:N0} Rewards");
            foreach (var kvp in onePair)
            {
                var s = SymbolMap(kvp.Key);
                var avg = (int)(kvp.Value[0] > 0 ? (double)kvp.Value[1] / kvp.Value[0] : 0);
                Console.WriteLine($" ⚪⚪⚪|{s}{s} ... [{(double)kvp.Value[0] / totalSpins,8:P4}] {kvp.Value[0],10} Spins {kvp.Value[1],10} Rewards {avg,6} Ø");
            }

            Console.WriteLine($"\n- THREE KIND --- [{(double)threeKindCount / totalSpins,8:P4}] {threeKindCount,10} Spins {threeKindRewards,10:N0} Rewards");
            foreach (var kvp in threeKind)
            {
                var s = SymbolMap(kvp.Key);
                var avg = (int)(kvp.Value[0] > 0 ? (double)kvp.Value[1] / kvp.Value[0] : 0);
                Console.WriteLine($" {s}{s}{s}|⚪⚪ ... [{(double)kvp.Value[0] / totalSpins,8:P4}] {kvp.Value[0],10} Spins {kvp.Value[1],10} Rewards {avg,6} Ø");
            }

            Console.WriteLine($"\n- FULL HOUSE --- [{(double)fullHouseCount / totalSpins,8:P4}] {fullHouseCount,10} Spins {fullHouseRewards,10:N0} Rewards");
            foreach (var kvp in fullHouse)
            {
                byte sKey = (byte)(kvp.Key & 0x0F);
                byte bKey = (byte)(kvp.Key >> 4);

                if (sKey == 0 || sKey > 9 || bKey == 0 || bKey > 9 || sKey == bKey)
                    continue;

                var s = SymbolMap(sKey);
                var b = SymbolMap(bKey);
                var avg = (int)(kvp.Value[0] > 0 ? (double)kvp.Value[1] / kvp.Value[0] : 0);
                Console.WriteLine($" {s}{s}{s}|{b}{b} ... [{(double)kvp.Value[0] / totalSpins,8:P4}] {kvp.Value[0],10} Spins {kvp.Value[1],10} Rewards {avg,6} Ø");
            }

            Console.WriteLine($"\n- ROYAL FLASH -- [{(double)royalFlushCount / totalSpins,8:P4}] {royalFlushCount,10} Spins {royalFlushRewards,10:N0} Rewards");
            foreach (var kvp in royalFlush)
            {
                var s = SymbolMap(kvp.Key);
                var avg = (int)(kvp.Value[0] > 0 ? (double)kvp.Value[1] / kvp.Value[0] : 0);
                Console.WriteLine($" {s}{s}{s}|{s}{s} ... [{(double)kvp.Value[0] / totalSpins,8:P4}] {kvp.Value[0],10} Spins {kvp.Value[1],10} Rewards {avg,6} Ø");
            }

            var totalOccurances = symbolOccurance.Values.Sum();
            Console.WriteLine($"\n- SYMBOL PROB.     {totalOccurances,13:N0}");
            foreach (var kvp in symbolOccurance)
            {
                var s = SymbolMap(kvp.Key);
                Console.WriteLine($" {s} = [{(double)kvp.Value / totalOccurances,8:P4}] = {kvp.Value,13:N0} Occurances");
            }

            Console.WriteLine($"\n{onePairRewards + threeKindRewards + fullHouseRewards + royalFlushRewards} Accumulated Rewards");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private static string SymbolMap(byte v)
        {
            return v switch
            {
                0 => "⚪",// ⚪ 0: BLANK
                1 => "🍒",// 🍒 1: CHERRY
                2 => "🍋",// 🍋 2: LEMON
                3 => "🍊",// 🍊 3: ORANGE
                4 => "🍑",// 🍑 4: PLUM
                5 => "🍉",// 🍉 5: WATERMELON
                6 => "🍇",// 🍇 6: GRAPE
                7 => "🔔",// 🔔 7: BELL
                8 => "💰",// 💰 8: BAR
                9 => "💎",// 💎 9: DIAMOND
                _ => throw new NotImplementedException($"Not implemented Symbol for {v}!"),
            };
        }

        /// <summary>
        /// Helper method to calculate the median of a list of unsigned integers.
        /// </summary>
        /// <param name="values">List of reward values.</param>
        /// <returns>Median value.</returns>
        private static double CalculateMedian(List<uint> values)
        {
            var sorted = values.OrderBy(x => x).Select(p => (double)p).ToList();
            int count = sorted.Count;
            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            else
                return sorted[count / 2];
        }

        private static int[] GetWeights(int symbolCount = 10)
        {
            // Ideal parameters: aiming for weight(0) around 50 and weight(9) near 1.
            double A = 50.2;
            double d = 5.4667;

            // Compute ideal weights.
            double[] ideal = new double[symbolCount];
            for (int i = 0; i < symbolCount; i++)
            {
                ideal[i] = A - i * d;
            }

            // Round the ideal weights.
            byte[] computedWeights = new byte[symbolCount];
            int total = 0;
            for (int i = 0; i < symbolCount; i++)
            {
                computedWeights[i] = (byte)Math.Round(ideal[i]);
                total += computedWeights[i];
            }

            // Adjust the weights so that the total equals 256.
            int diff = 256 - total;
            int index = 0;
            while (diff != 0)
            {
                if (diff > 0 && computedWeights[index] < 255)
                {
                    computedWeights[index]++;
                    diff--;
                }
                else if (diff < 0 && computedWeights[index] > 1)
                {
                    computedWeights[index]--;
                    diff++;
                }
                index = (index + 1) % symbolCount;
            }

            return computedWeights.Select(p => (int)p).ToArray();
        }
    }
}