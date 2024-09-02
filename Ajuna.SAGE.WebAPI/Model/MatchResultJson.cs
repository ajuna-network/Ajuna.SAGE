using Ajuna.SAGE.Generic.Algo;

namespace Ajuna.SAGE.WebAPI.Model
{
    public class MatchResultJson
    {
        public string Leader { get; set; }
        public string Sacrifice { get; set; }

        public MatchResult MatchResult { get; set; }

        public MatchResultJson(string leader, string sacrifice, MatchResult matchResult)
        {
            Leader = leader;
            Sacrifice = sacrifice;
            MatchResult = matchResult;

        }
    }
}