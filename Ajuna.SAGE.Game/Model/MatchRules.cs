namespace Ajuna.SAGE.Generic.Model
{
    public struct MatchRules
    {
        public MatchRules(byte minAssets, byte maxAssets, int scaleFactPerc, int progProbPerc, byte rarityLevel, int progVariation)
        {
            MinAssets = minAssets;
            MaxAssets = maxAssets;
            ScaleFactPerc = scaleFactPerc;
            ProgProbPerc = progProbPerc;
            RarityLevel = rarityLevel;
            ProgVariation = progVariation;
        }

        public byte MinAssets { get; } = 1;
        public byte MaxAssets { get; } = 5;
        public int ScaleFactPerc { get; } = 100;
        public int ProgProbPerc { get; } = 20;
        public byte RarityLevel { get; } = 0;
        public int ProgVariation { get; } = 6;
    }
}