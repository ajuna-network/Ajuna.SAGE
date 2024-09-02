namespace Ajuna.SAGE.Generic.Tests
{
    public enum ActionType : byte
    {
        TypeA = 0,
        TypeB = 1
    }

    public enum ActionSubType : byte
    {
        TypeX = 0,
        TypeY = 1
    }

    public enum ActionRuleType : byte
    {
        MinAsset = 0,
        MaxAsset = 1
    }

    public enum ActionRuleOp : byte
    {
        Equal = 0,
        Greater = 1,
        Lesser = 2,
        GreaterEqual = 3,
        LesserEqual = 4,
        NotEqual = 5
    }
}
