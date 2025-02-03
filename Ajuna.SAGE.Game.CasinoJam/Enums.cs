namespace Ajuna.SAGE.Game.CasinoJam
{
    public enum CasinoAction : byte
    {
        None = 0,
        Create = 1,
        Gamble = 2,
        Change = 3,
        Loot = 4,
    }

    public enum CasinoRuleType : byte
    {
        None = 0,
        AssetCount = 1,
        AllAssetType = 2,
        IsOwnerOf = 5,
        SameExist = 6,
        SameNotExist = 7,
        AssetTypesAt = 8,
        ScoreOf0 = 9,
    }

    public enum CasinoRuleOp : byte
    {
        None = 0,
        EQ = 1,
        GT = 2,
        LT = 3,
        GE = 4,
        LE = 5,
        NE = 6,
        Index = 7,
        MatchType = 8,
        Composite = 9,
    }

    public enum AssetType
    {
        None = 0,
        Player = 1,
        Machine = 2,
    }

    public enum AssetSubType
    {
        None = 0,
    }

    public enum PlayerSubType
    {
        None = 0
    }

    public enum MachineSubType
    {
        None = 0,
        Bandit = 1
    }

    public enum TokenType
    {
        T1 = 0,
        T10 = 1,
        T100 = 2,
        T1000 = 3,
    }

    public enum AmountType
    {
        A0 = 0,
        A1 = 1,
        A2 = 2,
        A3 = 3,
        A4 = 4,
        A5 = 5,
        A6 = 6,
        A7 = 7,
        A8 = 8,
        A9 = 9,
    }

}
