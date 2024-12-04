namespace Ajuna.SAGE.Game.HeroJam
{
    public enum HeroAction : byte
    {
        None = 0,
        Create = 1,
        Sleep = 2,
        Work = 3,
        Travel = 4,
        Claim = 5,
    }

    public enum ActionTime : byte
    {
        None = 0,
        Short = 1,
        Medium = 2,
        Long = 3
    }

    public enum WorkType : byte
    {
        None = 0,
        Hunt = 1,
        Gather = 2,
        Build = 3,
        Travel = 4,
        Train = 5,
        Fight = 6,
        Rest = 7,
    }

    public enum ZoneType : byte
    {
        None = 0,
        Homebase = 1,
        Village = 2,
        Forest = 3,
        Plains = 4,
        Mountain = 5,
        Cave = 6,
        Desert = 7,
        Dungeon = 8,
    }

    public enum HeroRuleType : byte
    {
        None = 0,
        AssetCount = 1,
        AllAssetType = 2,
        AllStateType = 3,
        CanStateChange = 4,
        IsOwnerOf = 5,
        SameExist = 6
    }

    public enum HeroRuleOp : byte
    {
        None = 0,
        EQ = 1,
        GT = 2,
        LT = 3,
        GE = 4,
        LE = 5,
        NE = 6,
        Index = 7,
        MatchType = 8
    }

    public enum AssetType
    {
        None = 0,
        Hero = 1,
    }

    public enum AssetSubType
    {
        None = 0,
    }

    public enum StateType
    {
        None = 0,
        Sleep = 1,
        Idle = 2,
        Work = 3,
    }
}
