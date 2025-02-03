using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;

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
        public static ushort PackSlotResult(int slot1, int slot2, int slot3, int bonus1, int bonus2)
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
    }
}