namespace Ajuna.SAGE.Game.CasinoJam
{
    public enum CasinoAction : byte
    {
        None = 0,
        Create = 1,
        Config = 2,
        Fund = 3,
        Gamble = 4,
        Loot = 5,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum CasinoSubAction : byte
    {
        None = 0,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum CasinoRuleType : byte
    {
        None = 0,
        AssetCount = 1,
        AssetTypeIs = 2,
        IsOwnerOf = 3,
        SameExist = 4,
        SameNotExist = 5,
        AssetTypesAt = 6,
        BalanceOf = 7,
        IsOwnerOfAll = 8,
        // *** DO NOT PASS 15 INDEX ***
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
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum AssetType
    {
        None = 0,
        Player = 1,
        Machine = 2,
        Tracker = 3,
        Seat = 4,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum AssetSubType
    {
        None = 0,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum PlayerSubType
    {
        None = 0,
        Human = 1,
        Tracker = 2
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum MachineSubType
    {
        None = 0,
        Bandit = 1
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum TokenType
    {
        T_1 = 0,
        T_10 = 1,
        T_100 = 2,
        T_1000 = 3,
        T_10000 = 4,
        T_100000 = 5,
        T_1000000 = 6,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum MultiplierType
    {
        V0 = 0,
        V1 = 1,
        V2 = 2,
        V3 = 3,
        V4 = 4,
        V5 = 5,
        V6 = 6,
        V7 = 7,
        V8 = 8,
        V9 = 9,
        // ...
        None = 15,
        // *** DO NOT PASS 15 INDEX ***
    }

}
