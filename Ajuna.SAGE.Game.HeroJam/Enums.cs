namespace Ajuna.SAGE.Game.HeroJam
{
    public enum HeroAction : byte
    {
        None = 0,
        Create = 1,
        Sleep = 2,
        Work = 3,
        Use = 4,
        //Claim = 5,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ActionTime : byte
    {
        None = 0,
        Short = 1,
        Medium = 2,
        Long = 3
        // *** DO NOT PASS 3 INDEX (as there is logic, not working with 3+) ***
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WorkType : byte
    {
        None = 0,
        Hunt = 1,
        // *** DO NOT PASS 15 INDEX ***
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SleepType : byte
    {
        None = 0,
        Normal = 1,
        //Fatigue = 2,
        // *** DO NOT PASS 15 INDEX ***
    }

    /// <summary>
    /// 
    /// </summary>
    public enum UseType : byte
    {
        None = 0,
        Disassemble = 1,
        Assemble = 2,
        Consume = 3,
        // *** DO NOT PASS 15 INDEX ***
    }

    public enum ZoneType : byte
    {
        None = 0,
        //Homebase = 1,
        //Village = 2,
        //Forest = 3,
        //Plains = 4,
        //Mountain = 5,
        //Cave = 6,
        //Desert = 7,
        //Dungeon = 8,
    }

    public enum HeroRuleType : byte
    {
        None = 0,
        AssetCount = 1,
        AllAssetType = 2,
        AllStateType = 3,
        CanStateChange = 4,
        IsOwnerOf = 5,
        SameExist = 6,
        SameNotExist = 7,
        AssetTypeAt = 8,
        AssetFlagAt = 9
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
        MatchType = 8,
        Composite = 9,
    }

    public enum AssetType
    {
        None = 0,
        Hero = 1,
        Animal = 2,
        Item = 3,
    }

    public enum AssetSubType
    {
        None = 0,
    }

    public enum HeroSubType
    {
        None = 0
    }

    public enum AnimalSubType
    {
        None = 0,
        
        // Tier 1
        Insects = 1, // 🦗 – Can be caught without much effort.
        Mouse = 2, // 🐭 – Small, but easy to trap.
        Hedgehog = 3, // 🦔 – Slow-moving, but requires careful handling.

        // Tier 2
        Squirrel = 4, // 🐿 – Quick, but catchable with patience or a trap.
        Lizard = 5, // 🦎 – Can be caught by hand if spotted in time.
        Snake = 6, // 🐍 – Risky but possible without weapons (bare hands or a stick).
        Duck = 7, // 🦆 – Can be grabbed near water but may require trapping.

        // Tier 3
        Rabbit = 8, // 🐰 – Fast, but can be caught with simple traps.
        Turkey = 9, // 🦃 – Alert and fast on foot, easier with ranged weapons.
        Fox = 10, // 🦊 – Smart and fast
        Boar = 11, // 🐗 – Strong and aggressive, dangerous without proper tools.

        // Tier 4
        Deer = 12, // 🦌 – Extremely fast, requires ranged weapons or skilled tracking.
        Wolf = 13, // 🐺 – Hunts in packs and is dangerous when provoked.
        Bison = 14, // 🐃 – Massive, resilient, and can charge aggressively.
        Bear = 15, // 🐻 – The most dangerous; requires high-level gear or traps.
    }

    public enum ItemSubType
    {
        None = 0,
        Map = 1,
        Meat = 2
    }

    public enum StateType
    {
        None = 0,
        Sleep = 1,
        Work = 2,
    }

    public enum HeroStats
    {
        None = 0,
        Energy = 1,
        Fatigue = 2,
    }
}
