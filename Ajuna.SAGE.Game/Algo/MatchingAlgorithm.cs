using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Algo;
using Ajuna.SAGE.Generic.Model;

namespace Ajuna.SAGE.Game.Algo
{
    public class MatchingAlgorithm
    {
        /// <summary>
        /// Matches the avatars.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="all"></param>
        /// <param name="rarityLevel"></param>
        /// <param name="randomHash"></param>
        /// <returns></returns>
        public static WrappedAsset ProgressAsset(MatchRules rules, IEnumerable<WrappedAsset> all, byte[] randomHash, uint blockNumber)
        {
            WrappedAsset leader = all.First();
            IEnumerable<WrappedAsset> sacrifices = all.Skip(1);

            int match = 0;
            int nofit = 0;
            List<int> matchingScore = new();

            foreach (WrappedAsset? sacrifice in sacrifices)
            {
                if (IsMatching(rules, leader.ProgressArray, sacrifice.ProgressArray, out MatchResult matchResult))
                {
                    matchingScore.AddRange(matchResult.MatchIndex);
                    match++;
                }
                else
                {
                    nofit++;
                }
            }

            // only forge if there is at least one variation in the matchingscore
            if (matchingScore.Count > 0)
            {
                // max rolls available
                int rolls = match + nofit;

                // these need to be tweeked to match sim.
                int matchProb = (rules.ScaleFactPerc - rules.ProgProbPerc) / rules.MaxAssets;

                // TODO: add star clock here fo the periods ....
                int pMatch = rules.ProgProbPerc + match * matchProb;

                byte[] progressArray = leader.ProgressArray;
                // executes all rolls that we calculated at max the distinct amount of variations
                for (int i = 0; i < rolls; i++)
                {
                    // calculates aprobability with pMatch on a certain byte in the hash
                    if (randomHash[randomHash[i] % 32] * rules.ScaleFactPerc <= pMatch * byte.MaxValue) // TODO: setting it on 255 would fail always if roll is 255, maybee interesting
                    {
                        // takes a random variation to be upgraded
                        int pos = matchingScore[randomHash[i] % matchingScore.Count];

                        // TODO: verify adding one rarity, which is a high byte
                        progressArray[pos] += 16;

                        // removes duplicates of the already upgraded gene
                        matchingScore.RemoveAll(p => p == pos);

                        // break out if there is no more variation to upgrade in matchingScore
                        if (matchingScore.Count == 0)
                        {
                            break;
                        }
                    }
                }

                // write it back to the leader
                leader.ProgressArray = progressArray;
            }

            // always accumulate soulpoints into the result
            leader.Asset.Score = (uint)all.Sum(p => p.Asset.Score);

            return leader;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="progressBytesA"></param>
        /// <param name="progressBytesB"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        public static bool IsMatching(MatchRules rules, byte[] progressBytesA, byte[] progressBytesB, out MatchResult matchResult)
        {
            matchResult = MatchProgress(rules, progressBytesA, progressBytesB);
            matchResult.IsMatching = IsMatching(matchResult);

            return matchResult.IsMatching;
        }

        /// <summary>
        /// Interpret match result in true, or false.
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="mirror"></param>
        /// <returns></returns>
        public static bool IsMatching(MatchResult matchResult)
        {
            var matchs = matchResult.MatchIndex.Count;
            var mirror = matchResult.MirrorIndex.Count;
            // 0 matches = false, 1 match + 2 mirrors || 2 match + 1 mirror || 3+ matches = true
            return matchs > 0 && matchs * 2 + mirror >= 6;
        }

        /// <summary>
        /// Matches the progress.
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="progressBytesA"></param>
        /// <param name="progressBytesB"></param>
        /// <returns></returns>
        internal static MatchResult MatchProgress(MatchRules rules, byte[] progressBytesA, byte[] progressBytesB)
        {
            byte rarityLevel = rules.RarityLevel;
            var result = new MatchResult(rarityLevel);

            var codeStructA = progressBytesA;
            byte aLowest = codeStructA.Lowest(ByteType.High);

            var codeStructB = progressBytesB;
            byte bLowest = codeStructB.Lowest(ByteType.High);

            if (aLowest > bLowest)
            {
                return result;
            }

            for (int i = 0; i < codeStructA.Length; i++)
            {
                // gen at position i from avatar A
                byte rarA = codeStructA.Read(i, ByteType.High);
                byte varA = codeStructA.Read(i, ByteType.Low);

                // gen at position i from avatar B
                byte rarB = codeStructB.Read(i, ByteType.High);
                byte varB = codeStructB.Read(i, ByteType.Low);

                // check if they have same rarity
                bool sameRarity = rarA == rarB || rarB == 0x0B; // special items match always 0xBB...BB

                // check if current gene is lower then lowest gen rarity (Rarity) or if gene already has highest rarity (not obsolet)
                bool maxed = rarA > aLowest; // || aLowest == (byte)RarityType.Legendary;

                // if same rarity and on lowest rarity check if gene is a matching candidate, imp. matching is not equal mirror
                if (sameRarity && !maxed && (
                        rarA < rarityLevel              // easy progression forges
                     || varB == 0x0B                    // special items match always
                     || MatchProgressByte(rules, varA, varB))) // normal algorithm
                {
                    result.MatchIndex.Add(i);
                }
                // if the genes are same rarity not on lowest rarity and same components, then they count as mirrored
                else if (maxed && (varA == varB || varB == 0x0B))
                {
                    result.MirrorIndex.Add(i);
                }
            }

            return result;
        }

        /// <summary>
        /// Matches the progress byte.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static bool MatchProgressByte(MatchRules rules, int v1, int v2)
        {
            int diff = Math.Abs(v1 - v2);
            // neighboor indexes are counted as matching example index 0 matches with index 1 and index max
            return diff == 1 || diff == rules.ProgVariation - 1;
        }
    }
}