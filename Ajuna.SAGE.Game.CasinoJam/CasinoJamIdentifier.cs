using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam
{
    public struct CasinoJamIdentifier : ITransitionIdentifier
    {
        public byte TransitionType { get; set; }
        public byte TransitionSubType { get; set; }

        public CasinoJamIdentifier(byte transitionType, byte transitionSubType)
        {
            TransitionType = transitionType;
            TransitionSubType = transitionSubType;
        }

        public CasinoJamIdentifier(byte transitionType) : this(transitionType, 0)
        {
        }

        public static CasinoJamIdentifier Create(AssetType assetType, AssetSubType assetSubType)
            => new((byte)CasinoAction.Create << 4 | (byte)AssetType.None, (byte)(((byte)assetType << 4) + (byte)assetSubType));

        public static CasinoJamIdentifier Fund(AssetType player, TokenType tokenType)
            => new((byte)CasinoAction.Fund << 4 | (byte)AssetType.None, (byte)(((byte)player << 4) + (byte)tokenType));

        public static CasinoJamIdentifier Gamble(TokenType tokenType, MultiplierType valueType)
            => new((byte)CasinoAction.Gamble << 4 | (byte)AssetType.None, (byte)(((byte)tokenType << 4) + (byte)valueType));

        public static CasinoJamIdentifier Loot(TokenType tokenType)
             => new((byte)CasinoAction.Loot << 4 | (byte)AssetType.None, (byte)(((byte)tokenType << 4) + (byte)0x00));

        internal static CasinoJamIdentifier Rent(AssetType assetType, AssetSubType assetSubType, MultiplierType multiplierType)
            => new((byte)CasinoAction.Rent << 4 | (byte)AssetType.None, (byte)(((byte)assetSubType << 4) + (byte)multiplierType));

        internal static CasinoJamIdentifier Reserve(AssetType assetType, AssetSubType assetSubType, MultiplierType multiplierType)
            => new((byte)CasinoAction.Reserve << 4 | (byte)AssetType.None, (byte)(((byte)assetSubType << 4) + (byte)multiplierType));
    }
}