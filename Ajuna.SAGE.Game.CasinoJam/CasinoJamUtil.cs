using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ajuna.SAGE.Game.CasinoJam.Test")]

namespace Ajuna.SAGE.Game.CasinoJam
{
    public partial class CasinoJamUtil
    {
        public const byte COLLECTION_ID = 1;

        public const byte BLOCKTIME_SEC = 6;

        public const byte BANDIT_MAX_SPINS = 4;

        public const uint BASE_RESERVATION_TIME = 5 * BLOCKS_PER_MINUTE; // 5 Minutes

        public const uint BLOCKS_PER_DAY = 24 * BLOCKS_PER_HOUR;
        public const uint BLOCKS_PER_HOUR = 60 * BLOCKS_PER_MINUTE;
        public const uint BLOCKS_PER_MINUTE = 10;

        public const uint BASE_RENT_FEE = 10;
        public const uint SEAT_USAGE_FEE_PERC = 1;

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

        public static byte MatchType(AssetType assetType)
        {
            return MatchType(assetType, AssetSubType.None);
        }

        public static byte MatchType(AssetType assetType, AssetSubType machineSubType)
        {
            var highHalfByte = (byte)assetType << 4;
            var lowHalfByte = (byte)machineSubType;
            return (byte)(highHalfByte | lowHalfByte);
        }

        /// <summary>
        /// ReservationDuration in blocks
        /// </summary>
        /// <param name="reservationDuration"></param>
        /// <returns></returns>
        public static uint GetReservationDurationBlocks(ReservationDuration reservationDuration)
        {
            uint multiplier = 0;
            switch (reservationDuration)
            {
                case ReservationDuration.None:
                    multiplier = 0;
                    break;
                case ReservationDuration.Mins5:
                    multiplier = 1;
                    break;
                case ReservationDuration.Mins10:
                    multiplier = 10 / 5;
                    break;
                case ReservationDuration.Mins15:
                    multiplier = 15 / 5;
                    break;
                case ReservationDuration.Mins30:
                    multiplier = 30 / 5;
                    break;
                case ReservationDuration.Mins45:
                    multiplier = 45 / 5;
                    break;
                case ReservationDuration.Hour1:
                    multiplier = (1 * 60) / 5;
                    break;
                case ReservationDuration.Hours2:
                    multiplier = (2* 60) / 5;
                    break;
                case ReservationDuration.Hours3:
                    multiplier = (3 * 60) / 5;
                    break;
                case ReservationDuration.Hours4:
                    multiplier = (4 * 60) / 5;
                    break;
                case ReservationDuration.Hours6:
                    multiplier = (6 * 60) / 5;
                    break;
                case ReservationDuration.Hours8:
                    multiplier = (8 * 60) / 5;
                    break;
                case ReservationDuration.Hours12:
                    multiplier = (12 * 60) / 5;
                    break;
            }

            return multiplier * CasinoJamUtil.BASE_RESERVATION_TIME;
        }

        /// <summary>
        /// TODO: Verify Fees!
        /// </summary>
        /// <param name="playerFee"></param>
        /// <param name="reservationDuration"></param>
        /// <returns></returns>
        public static uint GetReservationDurationFees(ushort playerFee, ReservationDuration reservationDuration)
        {
            return playerFee * (uint)reservationDuration;
        }

        /// <summary>
        /// RentDuration in blocks
        /// </summary>
        /// <param name="rentDuration"></param>
        /// <returns></returns>
        public static uint GetRentDurationBlocks(RentDuration rentDuration)
        {
            uint multiplier = 0;
            switch (rentDuration)
            {
                case RentDuration.None:
                    multiplier = 0;
                    break;
                case RentDuration.Day1:
                    multiplier = 1;
                    break;
                case RentDuration.Days2:
                    multiplier = 2;
                    break;
                case RentDuration.Days3:
                    multiplier = 3;
                    break;
                case RentDuration.Days5:
                    multiplier = 5;
                    break;
                case RentDuration.Days7:
                    multiplier = 7;
                    break;
                case RentDuration.Days14:
                    multiplier = 14;
                    break;
                case RentDuration.Days28:
                    multiplier = 28;
                    break;
                case RentDuration.Days56:
                    multiplier = 56;
                    break;
                case RentDuration.Days112:
                    multiplier = 112;
                    break;
            }

            return multiplier * CasinoJamUtil.BLOCKS_PER_DAY;
        }

        /// <summary>
        /// TODO: Verify Fees!
        /// </summary>
        /// <param name="bASE_SEAT_FEE"></param>
        /// <param name="rentDuration"></param>
        /// <returns></returns>
        public static uint GetRentDurationFees(uint bASE_SEAT_FEE, RentDuration rentDuration)
        {
            return bASE_SEAT_FEE * (uint)bASE_SEAT_FEE;
        }


    }
}