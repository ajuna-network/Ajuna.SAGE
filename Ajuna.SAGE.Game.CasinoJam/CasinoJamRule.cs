using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.CasinoJam
{
    public struct CasinoJamRule : ITransitionRule
    {
        public byte RuleType { get; private set; }

        public byte RuleOp { get; private set; }

        public byte[] RuleValue { get; private set; }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, uint value)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = Convert.ToByte(operation);
            RuleValue = BitConverter.GetBytes(value);
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte[] value)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = Convert.ToByte(operation);
            RuleValue = value;
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = Convert.ToByte(operation);
            RuleValue = [];
        }

        public CasinoJamRule(CasinoRuleType type)
        {
            RuleType = Convert.ToByte(type);
            RuleOp = 0x00;
            RuleValue = [];
        }
    }
}
