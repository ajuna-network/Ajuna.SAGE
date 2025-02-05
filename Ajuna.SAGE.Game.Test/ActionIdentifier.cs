using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Test
{
    public struct ActionIdentifier : ITransitionIdentifier
    {
        public byte TransitionType { get; private set; }
        public byte TransitionSubType { get; private set; }

        public ActionIdentifier(ActionType idType1, ActionSubType idType2)
        {
            TransitionType = Convert.ToByte(idType1);
            TransitionSubType = Convert.ToByte(idType2);
        }
    }
}
