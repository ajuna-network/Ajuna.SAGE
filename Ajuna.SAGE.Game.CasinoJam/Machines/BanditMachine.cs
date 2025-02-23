using Ajuna.SAGE.Game.CasinoJam.Model;
using System.Diagnostics;
using System.Numerics;
using System;

namespace Ajuna.SAGE.Game.CasinoJam.Machines
{
    public class Bandit
    {
        public const ushort MAX_SPIN_REWARD = 8192;

        public BanditAsset Asset { get; set; }

        public byte MaxSpins => Asset.MaxSpins;
        public uint MinSpinReward => (uint)Math.Pow(10, (byte)Asset.Value1Factor) * (byte)Asset.Value1Multiplier;
        public uint MinSpiJackpot => Asset.Jackpot;

        /// <summary>
        /// Bandit constructor.
        /// </summary>
        /// <param name="asset"></param>
        public Bandit(BanditAsset asset)
        {
            Asset = asset;
        }

        /// <summary>
        /// SingleSpinReward calculates the reward for a single spin.
        /// </summary>
        /// <param name="m">Minimum Reardd</param>
        /// <param name="s">Spin Result</param>
        /// <returns></returns>
        public static uint SingleSpinReward(uint m, SpinResult s)
        {

            uint sFactor = 0;
            if (s.Slot1 == s.Slot2 && s.Slot1 == s.Slot3)
            {
                sFactor = s.Slot1 switch
                {
                    0 =>    0,
                    1 =>    5 * m,
                    2 =>   10 * m,
                    3 =>   25 * m,
                    4 =>   50 * m,
                    5 =>  100 * m,
                    6 =>  200 * m,
                    7 =>  500 * m,
                    8 =>  750 * m,
                    9 => 1500 * m,
                    _ => 0,
                };
            }

            uint bFactor = 0;
            if (s.Bonus1 == s.Bonus2)
            {
                bFactor = s.Bonus1 switch
                {
                    0 => 0,
                    1 => 1 * m,
                    2 => 2 * m,
                    3 => 2 * m,
                    4 => 2 * m,
                    5 => 2 * m,
                    6 => 4 * m,
                    7 => 4 * m,
                    8 => 4 * m,
                    9 => 8 * m,
                    _ => 0,
                };
            }

            var isFullLine = s.Slot1 == s.Bonus1 && sFactor > 0 && bFactor > 0;

            uint reward = sFactor;

            if (sFactor > 0)
            {
                if (isFullLine)
                {
                    reward = sFactor * (128 / bFactor);
                }
                else if (bFactor > 0)
                {
                    reward = sFactor + (32 * bFactor);
                }
            }

            if (reward == 0)
            {
                reward = bFactor / m;
            }

            return reward;
        }

        public static FullSpin Spins(byte spinTimes, uint minSpinReward, uint jackMaxReward, uint specMaxReward, byte[] h)
        {
            // Ensure spinsToDo is within our allowed range.
            if (spinTimes < 1 || spinTimes > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(spinTimes), "Number of spins must be between 1 and 4.");
            }
            // Prepare lists to collect spin outcomes.
            List<SpinResult> spinResultsList = [];

            for (int i = 0; i < spinTimes; i++)
            {
                var offset = (uint)(i * 5);
                var spinResult = new SpinResult(
                    slot1: GetSlot(h[0 + offset]),
                    slot2: GetSlot(h[1 + offset]),
                    slot3: GetSlot(h[2 + offset]),
                    bonus1: GetSlot(h[3 + offset]),
                    bonus2: GetSlot(h[4 + offset]));

                spinResult.Reward = SingleSpinReward(minSpinReward, spinResult);

                spinResultsList.Add(spinResult);
            }

            return new FullSpin
            {
                SpinResults = spinResultsList.ToArray(),
                JackPotResult = "",
                JackPotReward = 0,
                SpecialResult = "",
                SpecialReward = 0
            }; ;
        }

        private static byte GetSlot(byte v)
        {
            // Adjusted weights so that the total equals 256.
            // Original proportions have been preserved by scaling.

            // [51,45,39,34,28,23,17,12,6,1]
            byte[] weights = new byte[]
            {
                 51,  // ⚪ 0: BLANK
                 43,  // 🍒 1: CHERRY
                 38,  // 🍋 2: LEMON
                 34,  // 🍊 3: ORANGE
                 28,  // 🍑 4: PLUM
                 23,  // 🍉 5: WATERMELON
                 17,  // 🍇 6: GRAPE
                 12,  // 🔔 7: BELL
                  6,  // 💰 8: BAR
                  3   // 💎 9: DIAMOND
            };

            byte cumulative = 0;
            for (byte i = 0; i < weights.Length; i++)
            {
                cumulative += weights[i];
                if (v <= cumulative)
                {
                    return i;
                }
            }

            return 0;
        }
    }
}