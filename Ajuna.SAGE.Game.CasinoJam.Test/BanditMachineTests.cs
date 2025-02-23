using Ajuna.SAGE.Game.CasinoJam;
using Ajuna.SAGE.Game.CasinoJam.Machines;
using Ajuna.SAGE.Game.CasinoJam.Model;
using Ajuna.SAGE.Game.Model;
using System.Security.Cryptography;

namespace Ajuna.SAGE.Game.HeroJam.Test
{
    [TestFixture]
    public class BanditMachineTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
        }

        [Test]
        public void Single_ProbabilityTests()
        {
            // Run a larger number of spins to get statistically meaningful results.
            int totalSpins = 10000;
            int winningSpins = 0;
            ulong totalReward = 0;

            for (int i = 0; i < totalSpins; i++)
            {
                // Here, spins returns a FullSpin object with one SpinResult since we pass spinTimes = 1.
                FullSpin fullSpin = Bandit.Spins(1, 1, 0, 0, RandomNumberGenerator.GetBytes(32));
                SpinResult spinResult = fullSpin.SpinResults[0];

                if (spinResult.Reward > 0)
                {
                    winningSpins++;
                }
                totalReward += spinResult.Reward;
            }

            // Calculate statistics
            double winProbability = (double)winningSpins / totalSpins;
            double averageReward = (double)totalReward / totalSpins;

            // Log the statistics (optional)
            TestContext.WriteLine($"Win Probability: {winProbability:P2}");
            TestContext.WriteLine($"Average Reward: {averageReward}");

            // Check that the win probability is within an expected range.
            // For example, if you expect around 5% winning spins,
            // adjust these thresholds as necessary based on your game logic.
            Assert.That(winProbability, Is.InRange(0.03, 0.07), "Win probability is out of the expected range.");

            // Similarly, assert that the average reward is within an expected range.
            // Here we assume an expected average reward between 1 and 1.5.
            Assert.That(averageReward, Is.InRange(1.0, 1.5), "Average reward is out of the expected range.");
        }

        [Test]
        public void Detailed_ProbabilityAndOutcomeStatisticsTest()
        {
            // Set the number of spins to simulate
            int totalSpins = 1000000;
            int winningSpins = 0;
            ulong totalReward = 0;

            // Counters for specific patterns:
            int sameSymbSlotCount = 0;
            int sameSymbBonusCount = 0;
            int sameSymbSlotAndBonusCount = 0;

            // Count occurrence of each symbol when all three slots are equal.
            Dictionary<byte, uint[]> sameSymbSlot = [];
            for (byte i = 0; i < 10; i++)
                sameSymbSlot[i] = [0,0];

            // Count occurrences for each bonus value when Bonus1 equals Bonus2.
            Dictionary<byte, uint[]> sameSymbBonus = [];
            for (byte i = 0; i < 10; i++)
                sameSymbBonus[i] = [0, 0];

            Dictionary<byte, uint[]> sameSymbSlotAndBonus = [];
            for (byte i = 0; i < 10; i++)
                sameSymbSlotAndBonus[i] = [0, 0];

            // List to track rewards for distribution analysis.
            List<uint> rewards = new(totalSpins);

            for (int i = 0; i < totalSpins; i++)
            {
                // Generate random bytes for one spin (5 bytes per spin: 3 for slots, 2 for bonuses)
                FullSpin fullSpin = Bandit.Spins(1, 1, 0, 0, RandomNumberGenerator.GetBytes(5));
                SpinResult spin = fullSpin.SpinResults[0];

                // Record win information
                if (spin.Reward > 0)
                {
                    winningSpins++;
                }

                totalReward += spin.Reward;
                rewards.Add(spin.Reward);

                var slotsMatch = spin.Slot1 != 0 && (spin.Slot1 == spin.Slot2 && spin.Slot2 == spin.Slot3);
                var bonusMatch = spin.Bonus1 != 0 && (spin.Bonus1 == spin.Bonus2);

                if (slotsMatch && bonusMatch)
                {
                    // Full line win
                    sameSymbSlotAndBonusCount++;
                    sameSymbSlotAndBonus[spin.Slot1][0]++;
                    sameSymbSlotAndBonus[spin.Slot1][1]+= spin.Reward;
                }
                else if (slotsMatch)
                {
                    // All slots are equal
                    sameSymbSlotCount++;
                    sameSymbSlot[spin.Slot1][0]++;
                    sameSymbSlot[spin.Slot1][1] += spin.Reward;
                }
                else if (bonusMatch)
                {
                    // Bonus values are equal
                    sameSymbBonusCount++;
                    sameSymbBonus[spin.Slot1][0]++;
                    sameSymbBonus[spin.Slot1][1] += spin.Reward;
                }
            }

            // Calculate overall statistics
            double winProbability = (double)winningSpins / totalSpins;
            double averageReward = (double)totalReward / totalSpins;
            uint minReward = rewards.Min();
            uint maxReward = rewards.Max();
            double medianReward = CalculateMedian(rewards);

            // Log the detailed statistics.
            TestContext.WriteLine($"Total Spins: {totalSpins}");
            TestContext.WriteLine($"Winning Spins: {winningSpins} ({winProbability:P2})");
            TestContext.WriteLine($"Total Reward: {totalReward}");
            TestContext.WriteLine($"Average Reward: {averageReward:F2}");
            TestContext.WriteLine($"Min Reward: {minReward}");
            TestContext.WriteLine($"Max Reward: {maxReward}");
            TestContext.WriteLine($"Median Reward: {medianReward:F2}");

            TestContext.WriteLine($"Same Symbol Count: {sameSymbSlotCount}");
            foreach (var kvp in sameSymbSlot)
            {
                TestContext.WriteLine($"  Same Symbol {kvp.Key} SPIN: {kvp.Value[0]} TOT: {kvp.Value[1]}");
            }

            TestContext.WriteLine($"Same Bonus Count: {sameSymbBonusCount}");
            foreach (var kvp in sameSymbBonus)
            {
                TestContext.WriteLine($"  Same Bonus {kvp.Key} SPIN: {kvp.Value[0]} TOT: {kvp.Value[1]}");
            }

            TestContext.WriteLine($"Same Symbol & Bonus Count: {sameSymbSlotAndBonusCount}");
            foreach (var kvp in sameSymbSlotAndBonus)
            {
                TestContext.WriteLine($"  Same Symbol & Bonus {kvp.Key} SPIN: {kvp.Value[0]} TOT: {kvp.Value[1]}");
            }

            // Example assertions - adjust these thresholds as appropriate for your game logic.
            Assert.That(winProbability, Is.InRange(0.03, 0.07), "Win probability is out of the expected range.");
            Assert.That(averageReward, Is.InRange(1.0, 1.5), "Average reward is out of the expected range.");
        }

        /// <summary>
        /// Helper method to calculate the median of a list of unsigned integers.
        /// </summary>
        /// <param name="values">List of reward values.</param>
        /// <returns>Median value.</returns>
        private double CalculateMedian(List<uint> values)
        {
            var sorted = values.OrderBy(x => x).ToList();
            int count = sorted.Count;
            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            else
                return sorted[count / 2];
        }
    }
}