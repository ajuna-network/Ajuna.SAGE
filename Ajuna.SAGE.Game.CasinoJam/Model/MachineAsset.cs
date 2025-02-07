using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam.Model
{
    public class MachineAsset : BaseAsset
    {
        public MachineAsset(uint genesis)
            : base(0, genesis)
        {
            AssetType = AssetType.Machine;
        }

        public MachineAsset(IAsset asset)
            : base(asset)
        { }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ H....... ........ ........
        public TokenType Value1Factor
        {
            get => (TokenType)Data.Read(8, ByteType.High);
            set => Data?.Set(8, ByteType.High, (byte)value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ L....... ........ ........
        public MultiplierType Value1Multiplier
        {
            get => (MultiplierType)Data.Read(8, ByteType.Low);
            set => Data?.Set(8, ByteType.Low, (byte)value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ .H...... ........ ........
        public TokenType Value2Factor
        {
            get => (TokenType)Data.Read(9, ByteType.High);
            set => Data?.Set(9, ByteType.High, (byte)value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ .L...... ........ ........
        public MultiplierType Value2Multiplier
        {
            get => (MultiplierType)Data.Read(9, ByteType.Low);
            set => Data?.Set(9, ByteType.Low, (byte)value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ..H..... ........ ........
        public TokenType Value3Factor
        {
            get => (TokenType)Data.Read(10, ByteType.High);
            set => Data?.Set(10, ByteType.High, (byte)value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ..L..... ........ ........
        public MultiplierType Value3Multiplier
        {
            get => (MultiplierType)Data.Read(10, ByteType.Low);
            set => Data?.Set(10, ByteType.Low, (byte)value);
        }
    }

    public class BanditAsset : MachineAsset
    {
        public BanditAsset(uint genesis)
            : base(genesis)
        {
            AssetSubType = (AssetSubType)MachineSubType.Bandit;
            MaxSpins = 4;
        }

        public BanditAsset(IAsset asset)
            : base(asset)
        { }

        /// <summary>
        /// Amount of maximum spins allowed.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ .......L ........ ........
        /// </summary>
        public byte MaxSpins
        {
            get => Data.Read(15, ByteType.Low);
            set => Data?.Set(15, ByteType.Low, value);
        }

        /// <summary>
        /// SetSlot is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data starting at positions 16 till 22.
        public void SetSlot(byte index, ushort packed)
        {
            if (index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            byte offset = 16;
            byte[] bytes = BitConverter.GetBytes(packed);
            Data[offset + index * 2] = bytes[0];
            Data[offset + index * 2 + 1] = bytes[1];
        }

        /// <summary>
        /// GetSlot is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data starting at positions 16 till 22.
        public ushort GetSlot(byte index)
        {
            if (index > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            var offset = 16;
            return BitConverter.ToUInt16(Data, offset + index * 2);
        }

        /// <summary>
        /// SlotResult is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data at positions 16 and 17.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ XX...... ........
        /// </summary>
        public ushort SlotAResult
        {
            get => BitConverter.ToUInt16(Data, 16);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Data[16] = bytes[0];
                Data[17] = bytes[1];
            }
        }

        /// <summary>
        /// SlotResult is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data at positions 18 and 19.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ..XX.... ........
        /// </summary>
        public ushort SlotBResult
        {
            get => BitConverter.ToUInt16(Data, 18);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Data[18] = bytes[0];
                Data[19] = bytes[1];
            }
        }

        /// <summary>
        /// SlotResult is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data at positions 20 and 21.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ....XX.. ........
        /// </summary>
        public ushort SlotCResult
        {
            get => BitConverter.ToUInt16(Data, 20);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Data[20] = bytes[0];
                Data[21] = bytes[1];
            }
        }

        /// <summary>
        /// SlotResult is a 16-bit field that encodes:
        /// Bits 15-12: Slot1 (4 bits)
        /// Bits 11-8:  Slot2 (4 bits)
        /// Bits 7-4:   Slot3 (4 bits)
        /// Bits 3-2:   Bonus1 (2 bits)
        /// Bits 1-0:   Bonus2 (2 bits)
        /// Stored in Data at positions 22 and 23.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ......XX ........
        /// </summary>
        public ushort SlotDResult
        {
            get => BitConverter.ToUInt16(Data, 22);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Data[22] = bytes[0];
                Data[23] = bytes[1];
            }
        }

        /// <summary>
        /// Jackpot is a 32-bit field that encodes the jackpot value.
        /// Stored in Data at positions 24 and 25.
        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// ........ ........ ........ XX......
        /// </summary>
        public uint Jackpot
        {
            get => BitConverter.ToUInt16(Data, 24);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                Data[24] = bytes[0];
                Data[25] = bytes[1];
            }
        }
    }
}