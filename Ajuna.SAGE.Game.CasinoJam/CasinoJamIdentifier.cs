using Ajuna.SAGE.Game.Model;
using System;

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
            => new((byte)CasinoAction.Create << 4 | (byte)CasinoSubAction.None, (byte)(((byte)assetType << 4) + (byte)assetSubType));

        public static CasinoJamIdentifier Fund(AssetType player, TokenType tokenType)
            => new((byte)CasinoAction.Fund << 4 | (byte)CasinoSubAction.None, (byte)(((byte)player << 4) + (byte)tokenType));

        public static CasinoJamIdentifier Gamble(TokenType tokenType, ValueType valueType)
            => new((byte)CasinoAction.Gamble << 4 | (byte)CasinoSubAction.None, (byte)(((byte)tokenType << 4) + (byte)valueType));

        public static CasinoJamIdentifier Loot(TokenType tokenType)
             => new((byte)CasinoAction.Loot << 4 | (byte)CasinoSubAction.None, (byte)(((byte)tokenType << 4) + (byte)0x00));
    }
}
