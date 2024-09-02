namespace Ajuna.SAGE.Generic.Algo
{

    public class MatchResult
    {
        public bool IsMatching { get; set; }
        public List<int> MatchIndex { get; set; }
        public List<int> MirrorIndex { get; set; }
        public byte RarityLevel { get; set; }

        public MatchResult(byte rarityLevel)
        {
            IsMatching = false;
            MatchIndex = new List<int>();
            MirrorIndex = new List<int>();
            RarityLevel = rarityLevel;
        }
    }
}