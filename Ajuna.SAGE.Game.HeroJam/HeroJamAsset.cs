using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class HeroJamAsset : Asset
    {
        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // X....X.. ........ ........ ....XXXX
        public HeroJamAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        public HeroJamAsset(IAsset asset)
            : base(asset.Id, asset.CollectionId, asset.Score, asset.Genesis, asset.Data, asset.Balance)
        { }

        // [0]000000 00111111 11112222 22222233
        // [0]234567 89012345 67890123 45678901
        //  H....... ........ ........ ........
        public AssetType AssetType
        {
            get => (AssetType)Data.Read(0, ByteType.High);
            set => Data?.Set(0, ByteType.High, (byte)value);
        }

        // [0]000000 00111111 11112222 22222233
        // [0]234567 89012345 67890123 45678901
        //  L....... ........ ........ ........
        public AssetSubType AssetSubType
        {
            get => (AssetSubType)Data.Read(0, ByteType.Low);
            set => Data?.Set(0, ByteType.Low, (byte)value);
        }

        //  0000[0]0 00111111 11112222 22222233
        //  0123[5]7 89012345 67890123 45678901
        //  .....X.. ........ ........ ........
        public byte Energy
        {
            get => Data.Read(5, ByteType.Full);
            set => Data?.Set(5, ByteType.Full, value);
        }

        //  00000[0] 00111111 11112222 22222233
        //  01234[6] 89012345 67890123 45678901
        //  ......X. ........ ........ ........
        public byte Fatigue
        {
            get => Data.Read(6, ByteType.Full);
            set => Data?.Set(6, ByteType.Full, value);
        }

        //  00000000 00111111 11112222 2[2]2233
        //  01234567 89012345 67890123 4[6]8901
        //  ........ ........ ........ ..X.....
        public StateType StateType
        {
            get => (StateType)Data.Read(26, ByteType.Full);
            set => Data.Set(26, ByteType.Full, (byte)value);
        }

        //  00000000 00111111 11112222 22[2]233
        //  01234567 89012345 67890123 45[7]901
        //  ........ ........ ........ ...H....
        public byte StateSubType
        {
            get => Data.Read(27, ByteType.High);
            set => Data?.Set(27, ByteType.High, (byte)value);
        }

        //  00000000 00111111 11112222 22[2]233
        //  01234567 89012345 67890123 45[7]901
        //  ........ ........ ........ ...L....
        public byte StateSubValue
        {
            get => Data.Read(27, ByteType.Low);
            set => Data?.Set(27, ByteType.Low, value);
        }

        //  00000000 00111111 11112222 222[2]33
        //  01234567 89012345 67890123 456[8]01
        //  ........ ........ ........ ....XXXX
        public uint StateChangeBlockNumber
        {
            get => BitConverter.ToUInt32(Data.Read(28, 4));
            set => Data?.Set(28, BitConverter.GetBytes(value));
        }

        /// <inheritdoc/>
        public override byte[] MatchType => Data != null && Data.Length > 0 ? [Data[0]] : [];
    }
}