namespace Ajuna.SAGE.Game.Model
{
    public interface ITransitionRule
    {
        byte RuleType { get; }
        byte RuleOp { get; }
        public byte[] RuleValue { get; }
    }

    public struct TransitionRules<T1, T2> : ITransitionRule
        where T1 : Enum
        where T2 : Enum
    {
        public byte RuleType { get; set; }
        public byte RuleOp { get; set; }
        public byte[] RuleValue { get; set; }

        public TransitionRules(T1 ruleType, T2 ruleOp, int ruleValue)
        {
            RuleType = Convert.ToByte(ruleType);
            RuleOp = Convert.ToByte(ruleOp);
            RuleValue = BitConverter.GetBytes(ruleValue);
        }
    }
}