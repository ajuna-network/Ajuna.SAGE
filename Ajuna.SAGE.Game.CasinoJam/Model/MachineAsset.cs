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
        /// .......H ........ ........ ........
        public byte SeatLinked
        {
            get => Data.Read(7, ByteType.High);
            set => Data?.Set(7, ByteType.High, value);
        }

        /// 00000000 00111111 11112222 22222233
        /// 01234567 89012345 67890123 45678901
        /// .......L ........ ........ ........
        public byte SeatLimit
        {
            get => Data.Read(7, ByteType.Low);
            set => Data?.Set(7, ByteType.Low, value);
        }

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