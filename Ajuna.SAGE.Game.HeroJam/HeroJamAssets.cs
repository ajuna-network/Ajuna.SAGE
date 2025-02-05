using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public class BaseAsset : Asset
    {
        public BaseAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis, new byte[Constants.DNA_SIZE])
        { }

        public BaseAsset(IAsset asset)
            : base(asset.Id, asset.CollectionId, asset.Score, asset.Genesis, asset.Data)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // H....... ........ ........ ........
        public AssetType AssetType
        {
            get => (AssetType)Data.Read(0, ByteType.High);
            set => Data?.Set(0, ByteType.High, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // L....... ........ ........ ........
        public AssetSubType AssetSubType
        {
            get => (AssetSubType)Data.Read(0, ByteType.Low);
            set => Data?.Set(0, ByteType.Low, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ ........ ........ ........
        public AssetFlags AssetFlags
        {
            get => new(Data.Read(1, ByteType.Full));
            set => Data?.Set(1, ByteType.Full, value.ToByte());
        }

        /// <inheritdoc/>
        public override byte[] MatchType => Data != null && Data.Length > 0 ? [Data[0]] : [];
    }

    public class HeroAsset : BaseAsset
    {
        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // X..XXX.. ........ ........ ....XXXX
        public HeroAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis)
        {
            AssetType = AssetType.Hero;
        }

        public HeroAsset(IAsset asset)
            : base(asset)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ...X.... ........ ........ ........
        public byte LocationX
        {
            get => Data[3];
            set => Data[3] = value;
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ....X... ........ ........ ........
        public byte LocationY
        {
            get => Data[4];
            set => Data[4] = value;
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // .....X.. ........ ........ ........
        public byte Energy
        {
            get => Data.Read(5, ByteType.Full);
            set => Data?.Set(5, ByteType.Full, value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ......X. ........ ........ ........
        public byte Fatigue
        {
            get => Data.Read(6, ByteType.Full);
            set => Data?.Set(6, ByteType.Full, value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ ........ ........ ..X.....
        public StateType StateType
        {
            get => (StateType)Data.Read(26, ByteType.Full);
            set => Data.Set(26, ByteType.Full, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ ........ ........ ...H....
        public byte StateSubType
        {
            get => Data.Read(27, ByteType.High);
            set => Data?.Set(27, ByteType.High, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ ........ ........ ...L....
        public byte StateSubValue
        {
            get => Data.Read(27, ByteType.Low);
            set => Data?.Set(27, ByteType.Low, value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ ........ ........ ....XXXX
        public uint StateChangeBlockNumber
        {
            get => BitConverter.ToUInt32(Data.Read(28, 4));
            set => Data?.Set(28, BitConverter.GetBytes(value));
        }
    }

    public class ItemAsset : BaseAsset
    {
        public ItemAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis)
        { 
            AssetType = AssetType.Item;
        }

        public ItemAsset(IAsset asset)
            : base(asset)
        { }
    }

    public class MapAsset : ItemAsset
    {
        public MapAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis)
        {
            AssetSubType = (AssetSubType) ItemSubType.Map;
        }

        public MapAsset(IAsset asset)
            : base(asset)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ...X.... ........ ........ ........
        public byte TargetX
        {
            get => Data[3];
            set => Data[3] = value;
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ....X... ........ ........ ........
        public byte TargetY
        {
            get => Data[4];
            set => Data[4] = value;
        }
    }

    public class DisassemblableAsset : BaseAsset
    {
        public DisassemblableAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis)
        { }

        public DisassemblableAsset(IAsset asset)
            : base(asset)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ H....... ........ ........
        public AssetType Result1AssetType
        {
            get => (AssetType)Data.Read(8, ByteType.High);
            set => Data?.Set(8, ByteType.High, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ L....... ........ ........
        public AssetSubType Result1AssetSubType
        {
            get => (AssetSubType)Data.Read(8, ByteType.Low);
            set => Data?.Set(8, ByteType.Low, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ .F...... ........ ........
        public byte Result1Amount
        {
            get => Data.Read(9, ByteType.Full);
            set => Data?.Set(9, ByteType.Full, value);
        }
    }

    public class ConsumableAsset : BaseAsset
    {
        public ConsumableAsset(ulong id, byte collectionId, uint score, uint genesis)
            : base(id, collectionId, score, genesis)
        { }

        public ConsumableAsset(IAsset asset)
            : base(asset)
        { }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ H....... ........ ........
        public HeroStats Effect1HeroStats
        {
            get => (HeroStats)Data.Read(8, ByteType.High);
            set => Data?.Set(8, ByteType.High, (byte)value);
        }

        // 00000000 00111111 11112222 22222233
        // 01234567 89012345 67890123 45678901
        // ........ .F...... ........ ........
        public sbyte Effect1Value
        {
            get => (sbyte)Data.Read(9, ByteType.Full);
            set => Data?.Set(9, ByteType.Full, (byte)value);
        }
    }
}