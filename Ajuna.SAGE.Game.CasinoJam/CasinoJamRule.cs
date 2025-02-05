using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.CasinoJam
{
    public struct CasinoJamRule : ITransitionRule
    {
        public byte RuleType { get; private set; }

        public byte RuleOp { get; private set; }

        public byte[] RuleValue { get; private set; }

        public CasinoRuleType CasinoRuleType => (CasinoRuleType)RuleType;
        public CasinoRuleOp CasinoRuleOp => (CasinoRuleOp)(RuleOp >> 4);
        public ValueType ValueType => (ValueType)(RuleOp & 0x0F);

        public CasinoJamRule(CasinoRuleType type, ValueType valueType, CasinoRuleOp operation, uint value)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)operation << 4 | (byte)valueType);
            RuleValue = BitConverter.GetBytes(value);
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, uint value)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)operation << 4 | (byte)ValueType.None);
            RuleValue = BitConverter.GetBytes(value);
        }

        public CasinoJamRule(CasinoRuleType type, ValueType valueType, CasinoRuleOp operation, byte[] value)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)operation << 4 | (byte)valueType);
            RuleValue = value;
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte[] value)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)operation << 4 | (byte)ValueType.None);
            RuleValue = value;
        }

        public CasinoJamRule(CasinoRuleType type, ValueType valueType, CasinoRuleOp operation)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)operation << 4 | (byte)valueType);
            RuleValue = [];
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte) operation << 4 | (byte)ValueType.None);
            RuleValue = [];
        }

        public CasinoJamRule(CasinoRuleType type)
        {
            RuleType = (byte)type;
            RuleOp = (byte)((byte)CasinoRuleOp.None << 4 | (byte)ValueType.None);
            RuleValue = [];
        }
    }
}
