namespace Ajuna.SAGE.Game.HeroJam
{
    public enum HeroAction : byte
    {
        Sleep = 0,
        Work = 1,
        Create = 2,
    }

    public enum ActionTime : byte
    {
        None = 0,
        Short = 1,
        Medium = 2,
        Long = 3
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
