namespace Ajuna.SAGE.Generic.Model
{
    public class Celestial : WrappedAsset
    {
        public Celestial(Asset avatar) : base(avatar)
        {
        }

        public Celestial(byte[] array, ItemSubTypeA itemSubType) : base(Asset.Empty(array, Constants.COLLECTION_ID))
        {
            ItemType = ItemType.ItemTypeA;
            ItemSubType = (HexType)itemSubType;
        }
    }
}