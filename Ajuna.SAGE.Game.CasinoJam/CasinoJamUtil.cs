﻿using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.CasinoJam.Test")]

namespace Ajuna.SAGE.Game.CasinoJam
{
    public class CasinoJamUtil
    {
        public const byte COLLECTION_ID = 1;

        public const byte BLOCKTIME_SEC = 6;

        public static uint Hour => 60 * 60 / BLOCKTIME_SEC;

        public static ulong GenerateRandomId()
        {
            var id = new byte[8];
            RandomNumberGenerator.Fill(id);

            return BitConverter.ToUInt64(id, 0);
        }

        /// <summary>
        /// Packs the slot machine result into a 16-bit unsigned integer.
        /// Layout:
        /// Bits 15-12: Slot1 (0-15)
        /// Bits 11-8:  Slot2 (0-15)
        /// Bits 7-4:   Slot3 (0-15)
        /// Bits 3-2:   Bonus1 (0-3)
        /// Bits 1-0:   Bonus2 (0-3)
        /// </summary>
        /// <param name="slot1">Slot A1 value (0-15)</param>
        /// <param name="slot2">Slot A2 value (0-15)</param>
        /// <param name="slot3">Slot A3 value (0-15)</param>
        /// <param name="bonus1">Bonus AS1 value (0-3)</param>
        /// <param name="bonus2">Bonus AS2 value (0-3)</param>
        /// <returns>A ushort containing the packed values.</returns>
        public static ushort PackSlotResult(byte slot1, byte slot2, byte slot3, byte bonus1, byte bonus2)
        {
            if (slot1 < 0 || slot1 > 15)
                throw new ArgumentOutOfRangeException(nameof(slot1));
            if (slot2 < 0 || slot2 > 15)
                throw new ArgumentOutOfRangeException(nameof(slot2));
            if (slot3 < 0 || slot3 > 15)
                throw new ArgumentOutOfRangeException(nameof(slot3));
            if (bonus1 < 0 || bonus1 > 3)
                throw new ArgumentOutOfRangeException(nameof(bonus1));
            if (bonus2 < 0 || bonus2 > 3)
                throw new ArgumentOutOfRangeException(nameof(bonus2));

            ushort result = 0;
            result |= (ushort)((slot1 & 0x0F) << 12); // Bits 15-12
            result |= (ushort)((slot2 & 0x0F) << 8);  // Bits 11-8
            result |= (ushort)((slot3 & 0x0F) << 4);  // Bits 7-4
            result |= (ushort)((bonus1 & 0x03) << 2);  // Bits 3-2
            result |= (ushort)(bonus2 & 0x03);         // Bits 1-0
            return result;
        }

        /// <summary>
        /// Unpacks the 16-bit slot machine result into its individual components.
        /// Returns a tuple with:
        /// (Slot1, Slot2, Slot3, Bonus1, Bonus2)
        /// </summary>
        /// <param name="slotResult">The packed 16-bit slot result.</param>
        /// <returns>A tuple of integers representing the unpacked values.</returns>
        public static (int slot1, int slot2, int slot3, int bonus1, int bonus2) UnpackSlotResult(ushort slotResult)
        {
            int slot1 = (slotResult >> 12) & 0x0F;
            int slot2 = (slotResult >> 8) & 0x0F;
            int slot3 = (slotResult >> 4) & 0x0F;
            int bonus1 = (slotResult >> 2) & 0x03;
            int bonus2 = slotResult & 0x03;
            return (slot1, slot2, slot3, bonus1, bonus2);
        }

        /// <summary>
        /// Calculates the reward for a slot machine result.
        /// </summary>
        /// <param name="slot1"></param>
        /// <param name="slot2"></param>
        /// <param name="slot3"></param>
        /// <param name="bonus1"></param>
        /// <param name="bonus2"></param>
        /// <returns></returns>
        public static uint SingleSpinReward(uint m, SpinResult s)
        {
            /*
                -- SLOT -----     -- BONUS +4 -  
                🍊 0: ORANGE      🍒 0: CHERRY 
                🍋 1: LEMON       🔔 1: CLOCK
                🍇 2: GRAPE       💰 2: MONEYBAG
                🍉 3: WATERMELON  💎 3: DIAMOND
                🍒 4: CHERRY
                🔔 5: CLOCK
                💰 6: MONEYBAG
                💎 7: DIAMOND

            */

            var sss = $"{s.Slot1}{s.Slot2}{s.Slot3}";

            uint sFactor = sss switch
            {
                "000" => 2 * m,
                "111" => 4 * m,
                "222" => 8 * m,
                "333" => 16 * m,
                "444" => 1 * m,
                "555" => 32 * m,
                "666" => 64 * m,
                "777" => 128 * m,
                "888" => 256 * m,
                "999" => 512 * m,
                _ => 0,
            };

            var bb = $"{s.Bonus1 + 4}{s.Bonus1 + 4}";

            uint bFactor = bb switch
            {
                "44" => 1,
                "55" => 2,
                "66" => 4,
                "77" => 8,
                _ => 0,
            };

            var isFullLine = s.Slot1 == s.Slot2 && s.Slot2 == s.Slot3 && s.Slot3 == s.Bonus1 && s.Bonus1 == s.Bonus2;

            uint reward = sFactor;
            if (isFullLine)
            {
                reward = sFactor * (128 / bFactor);
            }
            else if (sFactor > 0 && bFactor > 0)
            {
                reward = sFactor + (512 * bFactor);
            }

            if (reward == 0)
            {
                switch (bb)
                {
                    case "44":
                        // CHERRY
                        reward = 1;
                        break;

                    case "55":
                        // CLOCK
                        break;

                    case "66":
                        // MONEYBAG
                        break;

                    case "77":
                        // DIAMOND
                        break;
                }
            }

            return reward;
        }

        internal static FullSpin Spins(byte spinTimes, uint minSpinReward, uint jackMaxReward, uint specMaxReward, byte[] h)
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
                    slot1: (byte)(h[0 + offset] % 8),
                    slot2: (byte)(h[1 + offset] % 8),
                    slot3: (byte)(h[2 + offset] % 8),
                    bonus1: (byte)(h[3 + offset] % 4),
                    bonus2: (byte)(h[4 + offset] % 4));

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
    }
}