using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamAsset : Asset
    {
        // 01234567 89012345 67890123 45678901
        // X....X.. ........ ........ ....XXXX
        public HeroJamAsset(byte[] id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis) { }

        public AssetType AssetType
        {
            get => (AssetType)Data.Read(0, ByteType.High);
            set => Data.Set(0, ByteType.High, (byte)value);
        }

        public AssetSubType AssetSubType
        {
            get => (AssetSubType)Data.Read(0, ByteType.Low);
            set => Data.Set(0, ByteType.Low, (byte)value);
        }

        public byte Energy
        {
            get => Data.Read(5, ByteType.Full);
            set => Data.Set(5, ByteType.Full, value);
        }

        public StateType StateType
        {
            get => (StateType)Data.Read(27, ByteType.Full);
            set => Data.Set(27, ByteType.Full, (byte)value);
        }

        public uint StateChangeBlockNumber
        {
            get => BitConverter.ToUInt32(Data.Read(28, 4));
            set => Data.Set(28, BitConverter.GetBytes(value));
        }

        /// <inheritdoc/>
        public override byte[] MatchType => Data != null && Data.Length > 0 ? [Data[0]] : [];
    }
}