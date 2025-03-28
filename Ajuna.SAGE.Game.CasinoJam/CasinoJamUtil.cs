﻿using System.Runtime.CompilerServices;

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
        /// <param name="s1">Slot A1 value (0-15)</param>
        /// <param name="s2">Slot A2 value (0-15)</param>
        /// <param name="s3">Slot A3 value (0-15)</param>
        /// <param name="b1">Bonus AS1 value (0-3)</param>
        /// <param name="b2">Bonus AS2 value (0-3)</param>
        /// <returns>A ushort containing the packed values.</returns>
        public static byte[] PackSlotResult(byte s1, byte s2, byte s3, byte b1, byte b2)
        {
            if (s1 < 0 || s1 > 15)
                throw new ArgumentOutOfRangeException(nameof(s1));
            if (s2 < 0 || s2 > 15)
                throw new ArgumentOutOfRangeException(nameof(s2));
            if (s3 < 0 || s3 > 15)
                throw new ArgumentOutOfRangeException(nameof(s3));
            if (b1 < 0 || b1 > 15)
                throw new ArgumentOutOfRangeException(nameof(b1));
            if (b2 < 0 || b2 > 15)
                throw new ArgumentOutOfRangeException(nameof(b2));

            byte s4 = 0x00;

            byte p0 = 0;
            p0 |= (byte)((s1 & 0x0F) << 4); // Bits 7-4
            p0 |= (byte)((s2 & 0x0F) << 0); // Bits 3-0

            byte p1 = 0;
            p1 |= (byte)((s3 & 0x0F) << 4); // Bits 7-4
            p1 |= (byte)((s4 & 0x0F) << 0); // Bits 3-0

            byte p2 = 0;
            p2 |= (byte)((b1 & 0x0F) << 4); // Bits 7-4
            p2 |= (byte)((b2 & 0x0F) << 0); // Bits 3-0

            return [p0, p1, p2];
        }

        /// <summary>
        /// Unpacks the 16-bit slot machine result into its individual components.
        /// Returns a tuple with:
        /// (Slot1, Slot2, Slot3, Bonus1, Bonus2)
        /// </summary>
        /// <param name="sr">The packed 16-bit slot result.</param>
        /// <returns>A tuple of integers representing the unpacked values.</returns>
        public static (int slot1, int slot2, int slot3, int bonus1, int bonus2) UnpackSlotResult(byte[] p)
        {
            var p0 = p[0];
            int s1 = (p0 >> 4) & 0x0F;
            int s2 = (p0 >> 0) & 0x0F;

            var p1 = p[1];
            int s3 = (p1 >> 4) & 0x0F;
            int s4 = (p1 >> 0) & 0x0F;

            var p2 = p[2];
            int b1 = (p2 >> 4) & 0x0F;
            int b2 = (p2 >> 0) & 0x0F;
            return (s1, s2, s3, b1, b2);
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