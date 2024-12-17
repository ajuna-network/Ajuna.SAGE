namespace Ajuna.SAGE.Generic.Model
{
    public interface ITransitionIdentifier
    {
        byte TransitionType { get; }
        byte TransitionSubType { get; }
    }

    public struct TransitionIdentifier<T1, T2> : ITransitionIdentifier
        where T1 : Enum
        where T2 : Enum
    {
        public byte TransitionType { get; set; }
        public byte TransitionSubType { get; set; }

        public TransitionIdentifier(T1 idType1, T2 idType2)
        {
            TransitionType = Convert.ToByte(idType1);
            TransitionSubType = Convert.ToByte(idType2);
        }
    }
}