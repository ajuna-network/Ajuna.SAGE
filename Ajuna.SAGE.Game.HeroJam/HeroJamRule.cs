using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.HeroJam
{
    public struct HeroJamRule : ITransitionRule
    {
        public byte RuleType { get; private set; }

        public byte RuleOp { get; private set; }

        public byte[] RuleValue { get; private set; }

        public HeroJamRule(HeroRuleType type, HeroRuleOp operation, uint value)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = Convert.ToByte(operation);
            RuleValue = BitConverter.GetBytes(value);
        }
    }
}
