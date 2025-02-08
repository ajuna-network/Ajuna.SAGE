﻿using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public class SeatAsset : BaseAsset
    {
        public SeatAsset(uint genesis)
            : base(0, genesis)
        {
            AssetType = AssetType.Seat;
            AssetSubType = AssetSubType.None;
        }

        public SeatAsset(IAsset asset)
            : base(asset)
        { }

        /// <summary>
        /// Seat Validity Period, Usage: Genesis + Validity Period = Block of Seat Validity End
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ....XX.. ........ ........ ........
        /// </summary>
        public ushort SeatValidityPeriod
        {
            get => Data.ReadValue<ushort>(4);
            set => Data.SetValue<ushort>(4, value);
        }

        /// <summary>
        /// Player Fee
        /// Usage: Sets the fee for the minimal reservation period for the palyer.
        /// Stored as 2 bytes at offset 16.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ XX...... ........ ........
        /// </summary>
        public ushort PlayerFee
        {
            get => Data.ReadValue<ushort>(8);
            set => Data.SetValue<ushort>(8, value);
        }

        /// <summary>
        /// Blocks that the player can not be kicked out of the seat, after his last action.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ...X.... ........ ........
        /// </summary>
        public byte PlayerGracePeriod
        {
            get => Data.ReadValue<byte>(11);
            set => Data.SetValue<byte>(11, value);
        }

        /// <summary>
        /// The block when the reservation started.
        /// Stored as 4 bytes at offset 12.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ....XXXX ........ ........
        /// </summary>
        public uint ReservationStartBlock
        {
            get => Data.ReadValue<uint>(12);
            set => Data.SetValue<uint>(12, value);
        }

        /// <summary>
        /// Reservation Duration.
        /// Usage: ReservationStartBlock + ReservationDuration = Block when the reservation ends.
        /// Stored as 2 bytes at offset 16.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ XX...... ........
        /// </summary>
        public ushort ReservationDuration
        {
            get => Data.ReadValue<ushort>(16);
            set => Data.SetValue<ushort>(16, value);
        }

        /// <summary>
        /// LastActionBlock, Usage: ReservationStartBlock + LastActionBlock = Block of Last Action of Player
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ....XX.. ........
        /// </summary>
        public ushort LastActionBlock
        {
            get => Data.ReadValue<ushort>(20);
            set => Data.SetValue<ushort>(20, value);
        }

        /// <summary>
        /// PlayerActionCount
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ......XX ........
        /// </summary>
        public ushort PlayerActionCount
        {
            get => Data.ReadValue<ushort>(22);
            set => Data.SetValue<ushort>(22, value);
        }

        /// <summary>
        /// The identifier of the player currently occupying the seat.
        /// Stored as 4 bytes at offset 24.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ........ XXXX....
        /// </summary>
        public uint PlayerId
        {
            get => Data.ReadValue<uint>(24);
            set => Data.SetValue<uint>(24, value);
        }

        /// <summary>
        /// The identifier of the machine associated with this seat.
        /// Stored as 4 bytes at offset 28.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ........ ....XXXX
        /// </summary>
        public uint MachineId
        {
            get => Data.ReadValue<uint>(28);
            set => Data.SetValue<uint>(28, value);
        }
    }
}