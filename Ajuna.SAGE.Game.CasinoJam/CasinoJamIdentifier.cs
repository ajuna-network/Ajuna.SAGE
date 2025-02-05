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
            => new((byte)CasinoAction.Create, (byte)(((byte)assetType << 4) + (byte)assetSubType));

        public static CasinoJamIdentifier Fund(AssetType player, TokenType tokenType)
            => new((byte)CasinoAction.Fund, (byte)(((byte)player << 4) + (byte)tokenType));

        public static CasinoJamIdentifier Gamble(TokenType tokenType, ValueType valueType)
            => new((byte)CasinoAction.Gamble, (byte)(((byte)tokenType << 4) + (byte)valueType));

        public static CasinoJamIdentifier Change(TokenType tokenType, ValueType valueType)
            => new((byte)CasinoAction.Change, (byte)(((byte)tokenType << 4) + (byte)valueType));

        public static CasinoJamIdentifier Loot(TokenType tokenType)
             => new((byte)CasinoAction.Loot, (byte)(((byte)tokenType << 4) + (byte)0x00));
    }
}
