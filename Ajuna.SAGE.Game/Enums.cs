namespace Ajuna.SAGE.Generic
{
    public enum ByteType
    {
        Full,
        High,
        Low,
    }

    public enum PackType
    {
        PrimeMoon,
    }

    public enum RarityType
    {
        Common = 1,
        Uncommon = 2,
        Rare = 3,
        Epic = 4,
        Legendary = 5,
        Mythical = 6

    }

    public enum ItemType
    {
        ItemTypeA = 1,
        ItemTypeB = 2,
        ItemTypeC = 3,
        ItemTypeD = 4,
    }

    public enum HexType
    {
        X0 = 0,
        X1 = 1,
        X2 = 2,
        X3 = 3,
        X4 = 4,
        X5 = 5,
        X6 = 6,
        X7 = 7,
        X8 = 8,
        X9 = 9,
        XA = 10,
        XB = 11,
        XC = 12,
        XD = 13,
        XE = 14,
        XF = 15,
    }

    public enum NibbleType
    {
        X0 = 0, // 0000
        X1 = 1, // 0001
        X2 = 2, // 0010
        X3 = 3, // 0011
        X4 = 4, // 0100
        X5 = 5, // 0101
        X6 = 6, // 0110
        X7 = 7, // 0111
    }

    public enum ItemSubTypeA
    {
        ItemSubTypeA1 = 1,
        ItemSubTypeA2 = 2, 
        ItemSubTypeA3 = 3,
        ItemSubTypeA4 = 4,
    }

    public enum TransitionType
    {
        TransitionTypeA = 1,
        TransitionTypeB = 1,
    }

    public enum CelestialtemState
    {
        Unprospected = 1,
        Prospecting = 2,
        Prospected = 3,
    }

    public enum ConstructionItemType
    {
        Ship = 1,
        Shuttle = 2,
        Engine = 5,
        Radar = 3,
        Cargo = 4,
        Weapon = 6,
        Shield = 7,
        Armor = 8
     }

    public enum ConstructionItemState
    {
        Free = 1,
        Traveling = 2,
        Harvesting = 3,
    }

    public enum LifeformItemType
    {
        Colony = 1,
        Captain = 2,
        Crew = 3,
        Passenger = 4,
        Animal = 5,
        Plant = 6,
        Alien = 7,
    }

    public enum RessourceItemType
    {
        Stardust = 1,
        Scrap = 2,
        Fuel = 3,
        Food = 4,
        Water = 5,
        Oxygen = 6,
        Hydrogen = 7,
        Carbon = 8,
        Iron = 9,
        Copper = 10,
        Silver = 11,
        Gold = 12,
        Platinum = 13,
        Uranium = 14,
        Titanium = 15
    }

    public enum ShipState {
        Free = 1,
        Traveling = 2,
        Harvesting = 3,
    }

    public enum ForgeType
    {
        None = 0,
        Stack = 1,
        PrimeMoon = 2,
        ExtractStardust = 3,
        InfuseStardust = 4,
        EngageInEvent = 5,
        HarvestMoonOrNebula = 6,
        HarvestMoonOrNebulaFinish = 7,
        ForgeReceipt = 8,
        DetectNebula = 9,
        MintNebula = 10,
        ProspectingStart = 11,
        ProspectingEnd = 12,
        MintCoordinates = 13,
        FlyToMoon = 14,
        Upgrade = 15,
    }


}