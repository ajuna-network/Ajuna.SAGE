using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;
using System.Security.Cryptography;

namespace Ajuna.SAGE.Game.CasinoJam
{
    public class BaseAsset : Asset
    {
        public BaseAsset(uint score = 0, uint genesis = 0)
            : base(CasinoJamUtil.GenerateRandomId(), CasinoJamUtil.COLLECTION_ID, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        public BaseAsset(byte collectionId, uint score, uint genesis)
            : base(CasinoJamUtil.GenerateRandomId(), collectionId, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        public BaseAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        public BaseAsset(IAsset asset)
            : base(asset.Id, asset.CollectionId, asset.Score, asset.Genesis, asset.Data)
        { }

        public AssetType AssetType
        {
            get => (AssetType)Data.Read(0, ByteType.High);
            set => Data?.Set(0, ByteType.High, (byte)value);
        }

        public AssetSubType AssetSubType
        {
            get => (AssetSubType)Data.Read(0, ByteType.Low);
            set => Data?.Set(0, ByteType.Low, (byte)value);
        }

        /// <inheritdoc/>
        public override byte[] MatchType => Data != null && Data.Length > 0 ? [Data[0]] : [];
    }

    public class PlayerAsset : BaseAsset
    {
        public PlayerAsset(uint genesis)
            : base(0, genesis)
        {
            AssetType = AssetType.Player;
            LastReward = 0;
        }

        public PlayerAsset(IAsset asset)
            : base(asset)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ XXXX.... ........ ........
        public uint LastReward
        {
            get => BitConverter.ToUInt32(Data, 8);
            set
            {
                byte[] bytes = BitConverter.GetBytes(value);
                for (int i = 0; i < 4; i++)
                {
                    Data[8 + i] = bytes[i];
                }
            }
        }

    }

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
        }

        public BanditAsset(IAsset asset)
            : base(asset)
        { }

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
            Data[offset + (index * 2)] = bytes[0];
            Data[offset + (index * 2) + 1] = bytes[1];
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
            return BitConverter.ToUInt16(Data, offset + (index * 2));
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