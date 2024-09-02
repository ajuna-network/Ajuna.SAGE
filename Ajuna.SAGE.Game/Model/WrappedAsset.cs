namespace Ajuna.SAGE.Generic.Model
{
    public partial class WrappedAsset
    {
        private readonly Asset _asset;

        public Asset Asset => _asset;

        public byte[] Base
        {
            get => _asset.Id;
        }

        public ItemType ItemType
        {
            get => (ItemType)_asset.Data.Read(0, ByteType.High);
            set => _asset.Data.Set(0, ByteType.High, (byte)value);
        }

        public HexType ItemSubType
        {
            get => (HexType)_asset.Data.Read(0, ByteType.Low);
            set => _asset.Data.Set(0, ByteType.Low, (byte)value);
        }

        public HexType ClassType1
        {
            get => (HexType)_asset.Data.Read(1, ByteType.High);
            set => _asset.Data.Set(1, ByteType.High, (byte)value);
        }

        public HexType ClassType2
        {
            get => (HexType)_asset.Data.Read(1, ByteType.Low);
            set => _asset.Data.Set(1, ByteType.Low, (byte)value);
        }

        public HexType CustomType1
        {
            get => (HexType)_asset.Data.Read(2, ByteType.High);
            set => _asset.Data.Set(2, ByteType.High, (byte)value);
        }

        public RarityType RarityType
        {
            get => (RarityType)_asset.Data.Read(2, ByteType.Low);
            set => _asset.Data.Set(2, ByteType.Low, (byte)value);
        }

        public byte Quantity
        {
            get => _asset.Data.Read(3, ByteType.Full);
            set => _asset.Data.Set(3, ByteType.Full, value);
        }

        public byte CustomType2
        {
            get => _asset.Data.Read(4, ByteType.Full);
            set => _asset.Data.Set(4, ByteType.Full, value);
        }

        public WrappedAsset(Asset avatar)
        {
            _asset = avatar;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wrappedAvatar"></param>
        /// <returns></returns>
        public bool SameItemType(WrappedAsset wrappedAvatar)
        {
            return ItemType == wrappedAvatar.ItemType;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wrappedAvatar"></param>
        /// <returns></returns>
        public bool SameItemTypesAndClassTypes(WrappedAsset wrappedAvatar)
        {
            return ItemType == wrappedAvatar.ItemType
                && ItemSubType == wrappedAvatar.ItemSubType
                && ClassType1 == wrappedAvatar.ClassType1
                && ClassType2 == wrappedAvatar.ClassType2;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="wrappedAvatar"></param>
        /// <returns></returns>
        public bool SameAssembleVersion(WrappedAsset wrappedAvatar)
        {
            // VERSION 1 Master: sacrifices need at least to have one similar item then the leader (itemtype, itemsubtype, pettype, slottype)
            //return ItemType == wrappedAvatar.ItemType
            //    && ItemSubType == wrappedAvatar.ItemSubType
            //    && ClassType1 == wrappedAvatar.ClassType1
            //    && ClassType2 == wrappedAvatar.ClassType2;

            // VERSION 2 Expert: sacrifices need at least to have a item that matches in (itemtype, pettype, slottype) the itemsubtype can be different
            return ItemType == wrappedAvatar.ItemType
                && ClassType1 == wrappedAvatar.ClassType1
                && ClassType2 == wrappedAvatar.ClassType2;

            // VERSION 3 Intermediate: sacrifices need at least to have a item that matches in (itemtype, pettype) the itemsubtype and the slottype can be different
            // not supported so far, need change in Forge Type too

            // VERSION 4 Novice: sacrifices need at least to have a item that matches in (itemtype) the itemsubtype, slottype and the pettype can be different
            // not supported so far, need change in Forge Type too
        }

        public static WrappedAsset CreateWrappedAvatar(byte[] key, ItemType itemType, HexType itemSubType, HexType classType1, HexType classType2, HexType customType1, RarityType rarityType, byte quantity, byte customType2, byte[] specBytes, byte[] progressArray)
        {
            return new WrappedAsset(Asset.Empty(key, Constants.COLLECTION_ID))
            {
                ItemType = itemType,
                ItemSubType = itemSubType,
                ClassType1 = classType1,
                ClassType2 = classType2,
                CustomType1 = customType1,
                RarityType = rarityType,
                Quantity = quantity,
                CustomType2 = customType2,
                SpecBytes = specBytes,
            };
        }
    }

    public partial class WrappedAsset
    {
        public byte[] SpecBytes
        {
            get => _asset.Data.Read(5, 27);
            set => _asset.Data.Set(5, value);
        }

        public byte SpecByte1
        {
            get => _asset.Data.Read(5, ByteType.Full);
            set => _asset.Data.Set(5, ByteType.Full, value);
        }

        public byte SpecByte2
        {
            get => _asset.Data.Read(6, ByteType.Full);
            set => _asset.Data.Set(6, ByteType.Full, value);
        }

        public byte SpecByte3
        {
            get => _asset.Data.Read(7, ByteType.Full);
            set => _asset.Data.Set(7, ByteType.Full, value);
        }

        public byte SpecByte4
        {
            get => _asset.Data.Read(8, ByteType.Full);
            set => _asset.Data.Set(8, ByteType.Full, value);
        }

        public byte SpecByte5
        {
            get => _asset.Data.Read(9, ByteType.Full);
            set => _asset.Data.Set(9, ByteType.Full, value);
        }

        public byte SpecByte6
        {
            get => _asset.Data.Read(10, ByteType.Full);
            set => _asset.Data.Set(10, ByteType.Full, value);
        }

        public byte SpecByte7
        {
            get => _asset.Data.Read(11, ByteType.Full);
            set => _asset.Data.Set(11, ByteType.Full, value);
        }

        public byte SpecByte8
        {
            get => _asset.Data.Read(12, ByteType.Full);
            set => _asset.Data.Set(12, ByteType.Full, value);
        }

        public byte SpecByte9
        {
            get => _asset.Data.Read(13, ByteType.Full);
            set => _asset.Data.Set(13, ByteType.Full, value);
        }

        public byte SpecByte10
        {
            get => _asset.Data.Read(14, ByteType.Full);
            set => _asset.Data.Set(14, ByteType.Full, value);
        }

        public byte SpecByte11
        {
            get => _asset.Data.Read(15, ByteType.Full);
            set => _asset.Data.Set(15, ByteType.Full, value);
        }

        public byte SpecByte12
        {
            get => _asset.Data.Read(16, ByteType.Full);
            set => _asset.Data.Set(16, ByteType.Full, value);
        }

        public byte SpecByte13
        {
            get => _asset.Data.Read(17, ByteType.Full);
            set => _asset.Data.Set(17, ByteType.Full, value);
        }

        public byte SpecByte14
        {
            get => _asset.Data.Read(18, ByteType.Full);
            set => _asset.Data.Set(18, ByteType.Full, value);
        }

        public byte SpecByte15
        {
            get => _asset.Data.Read(19, ByteType.Full);
            set => _asset.Data.Set(19, ByteType.Full, value);
        }

        public byte SpecByte16
        {
            get => _asset.Data.Read(20, ByteType.Full);
            set => _asset.Data.Set(20, ByteType.Full, value);
        }

        public byte SpecByte17
        {
            get => _asset.Data.Read(21, ByteType.Full);
            set => _asset.Data.Set(21, ByteType.Full, value);
        }

        public byte SpecByte18
        {
            get => _asset.Data.Read(22, ByteType.Full);
            set => _asset.Data.Set(22, ByteType.Full, value);
        }

        public byte SpecByte19
        {
            get => _asset.Data.Read(23, ByteType.Full);
            set => _asset.Data.Set(23, ByteType.Full, value);
        }

        public byte SpecByte20
        {
            get => _asset.Data.Read(24, ByteType.Full);
            set => _asset.Data.Set(24, ByteType.Full, value);
        }

        public byte SpecByte21
        {
            get => _asset.Data.Read(25, ByteType.Full);
            set => _asset.Data.Set(25, ByteType.Full, value);
        }

        public byte SpecByte22
        {
            get => _asset.Data.Read(26, ByteType.Full);
            set => _asset.Data.Set(26, ByteType.Full, value);
        }

        public byte SpecByte23
        {
            get => _asset.Data.Read(27, ByteType.Full);
            set => _asset.Data.Set(27, ByteType.Full, value);
        }

        public byte SpecByte24
        {
            get => _asset.Data.Read(28, ByteType.Full);
            set => _asset.Data.Set(28, ByteType.Full, value);
        }

        public byte SpecByte25
        {
            get => _asset.Data.Read(29, ByteType.Full);
            set => _asset.Data.Set(29, ByteType.Full, value);
        }

        public byte SpecByte26
        {
            get => _asset.Data.Read(30, ByteType.Full);
            set => _asset.Data.Set(30, ByteType.Full, value);
        }

        public byte SpecByte27
        {
            get => _asset.Data.Read(31, ByteType.Full);
            set => _asset.Data.Set(31, ByteType.Full, value);
        }
    }

    public partial class WrappedAsset
    {
        public byte[] ProgressArray
        {
            get => _asset.Data.Read(21, 11);
            set => _asset.Data.Set(21, value);
        }
    }

    public partial class WrappedAsset
    {
        public (int, int) Coords
        {
            get
            {
                int x = BitConverter.ToUInt16(_asset.Data.Read(24, 2), 0);
                int y = BitConverter.ToUInt16(_asset.Data.Read(26, 2), 0);
                return (x, y);
            }
            set
            {
                _asset.Data.Set(24, BitConverter.GetBytes((ushort)value.Item1));
                _asset.Data.Set(26, BitConverter.GetBytes((ushort)value.Item2));
            }
        }
    }
}