using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public struct HeroJamIdentifier : ITransitionIdentifier
    {
        public byte TransitionType { get; set; }
        public byte TransitionSubType { get; set; }

        public HeroJamIdentifier(byte transitionType, byte transitionSubType)
        {
            TransitionType = transitionType;
            TransitionSubType = transitionSubType;
        }

        public HeroJamIdentifier(byte transitionType) : this(transitionType, 0) { }

        public static HeroJamIdentifier Create(AssetType assetType, AssetSubType assetSubType)
            => new((byte)HeroAction.Create, (byte)(((byte)assetType << 4) + (byte)assetSubType));
    }
}
