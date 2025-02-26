namespace Ajuna.SAGE.Game.CasinoJam
{
    public enum CasinoAction : byte
    {
        None = 0,
        Create = 1,
        Config = 2,
        Deposit = 3,
        Gamble = 4,
        Withdraw = 5,
        Rent = 6,
        Reserve = 7,
        Release = 8,
        Kick = 9,
        Return = 10,
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
        Bandit = 1,
        Roulette = 2,
        BlackJack = 3,
        GuessNumber = 4
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum SeatSubType : byte
    {
        None = 0,
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

    public enum RentDuration
    {
        None = 0,
        Day1 = 1,
        Days2 = 2,
        Days3 = 3,
        Days5 = 4,
        Days7 = 5,
        Days14 = 6,
        Days28 = 7,
        Days56 = 8,
        Days112 = 9,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum ReservationDuration
    {
        None = 0,
        Mins5 = 1,
        Mins10 = 2,
        Mins15 = 3,
        Mins30 = 4,
        Mins45 = 5,
        Hour1 = 6,
        Hours2 = 7,
        Hours3 = 8,
        Hours4 = 9,
        Hours6 = 10,
        Hours8 = 11,
        Hours12 = 12,
        // *** DO NOT PASS 15 INDEX ***
    }
}