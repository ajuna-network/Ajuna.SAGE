using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public struct HeroJamIdentifier : ITransitionIdentifier
    {
        public byte TransitionType { get; private set; }
        public byte TransitionSubType { get; private set; }

        public HeroJamIdentifier(byte transitionType, byte transitionSubType)
        {
            TransitionType = transitionType;
            TransitionSubType = transitionSubType;
        }
    }
}
