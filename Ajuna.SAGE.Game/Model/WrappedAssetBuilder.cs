namespace Ajuna.SAGE.Generic.Model
{
    public abstract class WrappedAssetBuilder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static WrappedAsset CreateItemSubTypeA(byte[] array, byte[]? specBytes, byte[]? progressArray, uint soulPoints)
        {
            RarityType rarityType = RarityType.Legendary;

            Celestial result = new(array, ItemSubTypeA.ItemSubTypeA1)
            {
                ClassType1 = HexType.X0,
                ClassType2 = HexType.X0,

                CustomType1 = HexType.X0, // (Temp) Prime Moons are not stackable
                CustomType2 = 0x00,

                RarityType = rarityType,
                Quantity = 1,

                SpecBytes = specBytes ?? (new byte[16]),

                Coords = (0, 0)
            };
            result.Asset.Score = soulPoints;
            result.Asset.Genesis = 0;
            return result;
        }
    }
}