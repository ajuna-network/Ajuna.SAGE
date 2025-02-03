using Ajuna.SAGE.Generic.Model;

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

        public CasinoJamIdentifier(byte transitionType) : this(transitionType, 0) { }

        public static CasinoJamIdentifier Create(AssetType assetType, AssetSubType assetSubType)
            => new((byte)CasinoAction.Create, (byte)(((byte)assetType << 4) + (byte)assetSubType));

        public static CasinoJamIdentifier Gamble(TokenType tokenType, AmountType amountType)
            => new((byte)CasinoAction.Gamble, (byte)(((byte)tokenType << 4) + (byte)amountType));

        public static CasinoJamIdentifier Change(TokenType tokenType, AmountType amountType)
            => new((byte)CasinoAction.Change, (byte)(((byte)tokenType << 4) + (byte)amountType));

        public static CasinoJamIdentifier Loot()
             => new((byte)CasinoAction.Loot,(0x00 << 4) + 0x00);

    }
}
