using Ajuna.SAGE.Game.Model;

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

        public HeroJamRule(HeroRuleType type, HeroRuleOp operation, byte[] value)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = Convert.ToByte(operation);
            RuleValue = value;
        }
    }
}
