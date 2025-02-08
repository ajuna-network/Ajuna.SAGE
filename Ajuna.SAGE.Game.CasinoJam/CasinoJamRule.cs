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
        public MultiplierType ValueType => (MultiplierType)(RuleOp & 0x0F);

        private static byte CreateRuleOp(CasinoRuleOp operation, MultiplierType valueType) =>
            (byte)(((byte)operation << 4) | (byte)valueType);

        // Existing constructors using byte[] or uint as value:
        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, byte[] value)
        {
            RuleType = (byte)type;
            RuleOp = CreateRuleOp(operation, valueType);
            RuleValue = value;
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte[] value)
            : this(type, MultiplierType.None, operation, value)
        {
        }

        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, uint value)
            : this(type, valueType, operation, BitConverter.GetBytes(value))
        {
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, uint value)
            : this(type, MultiplierType.None, operation, BitConverter.GetBytes(value))
        {
        }

        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation)
            : this(type, valueType, operation, Array.Empty<byte>())
        {
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation)
            : this(type, MultiplierType.None, operation, Array.Empty<byte>())
        {
        }

        public CasinoJamRule(CasinoRuleType type)
            : this(type, MultiplierType.None, CasinoRuleOp.None, Array.Empty<byte>())
        {
        }

        // New constructors for single bytes (they get encoded into a uint)
        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, byte b1)
            : this(type, valueType, operation, [b1, 0x00, 0x00, 0x00])
        {
        }

        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, byte b1, byte b2)
            : this(type, valueType, operation, [b1, b2, 0x00, 0x00])
        {
        }

        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, byte b1, byte b2, byte b3)
            : this(type, valueType, operation, [b1, b2, b3, 0x00])
        {
        }

        public CasinoJamRule(CasinoRuleType type, MultiplierType valueType, CasinoRuleOp operation, byte b1, byte b2, byte b3, byte b4)
            : this(type, valueType, operation, [b1, b2, b3, b4])
        {
        }

        // Overloads with default MultiplierType (None)
        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte b1)
            : this(type, MultiplierType.None, operation, b1)
        {
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte b1, byte b2)
            : this(type, MultiplierType.None, operation, b1, b2)
        {
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte b1, byte b2, byte b3)
            : this(type, MultiplierType.None, operation, b1, b2, b3)
        {
        }

        public CasinoJamRule(CasinoRuleType type, CasinoRuleOp operation, byte b1, byte b2, byte b3, byte b4)
            : this(type, MultiplierType.None, operation, b1, b2, b3, b4)
        {
        }
    }
}