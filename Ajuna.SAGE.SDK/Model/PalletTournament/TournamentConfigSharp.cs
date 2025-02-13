using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.AjunaSolo.NET.NetApiExt.Helper;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Primitive;
using System.Linq;
using System.Numerics;

namespace Substrate.Integration.Model.PalletTournament
{
    /// <summary>
    /// TournamentConfigSharp
    /// </summary>
    public class TournamentConfigSharp
    {
        /// <summary>
        /// TournamentConfigSharp constructor
        /// </summary>
        /// <param name="tournamentConfig"></param>
        public TournamentConfigSharp(TournamentConfig tournamentConfig)
        {
            Start = tournamentConfig.Start;
            ActiveEnd = tournamentConfig.ActiveEnd;
            ClaimEnd = tournamentConfig.ClaimEnd;
            if (tournamentConfig.InitialReward.OptionFlag)
            {
                InitialReward = tournamentConfig.InitialReward.Value;
            }
            if (tournamentConfig.MaxReward.OptionFlag)
            {
                MaxReward = tournamentConfig.MaxReward.Value;
            }
            if (tournamentConfig.TakeFeePercentage.OptionFlag)
            {
                TakeFeePercentage = tournamentConfig.TakeFeePercentage.Value;
            }
            RewardDistribution = tournamentConfig.RewardDistribution.Value.ToBytes();
            GoldenDuckConfig = new GoldenDuckConfigSharp(tournamentConfig.GoldenDuckConfig);
            MaxPlayers = tournamentConfig.MaxPlayers;
        }

        /// <summary>
        /// TournamentConfigSharp constructor
        /// </summary>
        /// <param name="start"></param>
        /// <param name="activeEnd"></param>
        /// <param name="claimEnd"></param>
        /// <param name="initialReward"></param>
        /// <param name="maxReward"></param>
        /// <param name="takeFeePercentage"></param>
        /// <param name="rewardDistribution"></param>
        /// <param name="goldenDuckConfig"></param>
        /// <param name="maxPlayers"></param>
        public TournamentConfigSharp(uint start, uint activeEnd, uint claimEnd, BigInteger? initialReward, BigInteger? maxReward, byte? takeFeePercentage, byte[] rewardDistribution, GoldenDuckConfigSharp goldenDuckConfig, uint maxPlayers)
        {
            Start = start;
            ActiveEnd = activeEnd;
            ClaimEnd = claimEnd;
            InitialReward = initialReward;
            MaxReward = maxReward;
            TakeFeePercentage = takeFeePercentage;
            RewardDistribution = rewardDistribution;
            GoldenDuckConfig = goldenDuckConfig;
            MaxPlayers = maxPlayers;
        }

        /// <summary>
        /// ToSubstrate
        /// </summary>
        /// <returns></returns>
        public TournamentConfig ToSubstrate()
        {
            var tournamentConfig = new TournamentConfig
            {
                Start = new U32(Start),
                ActiveEnd = new U32(ActiveEnd),
                ClaimEnd = new U32(ClaimEnd),
                InitialReward = new BaseOpt<U128>(),
                MaxReward = new BaseOpt<U128>(),
                TakeFeePercentage = new BaseOpt<U8>(),
                RewardDistribution = new BoundedVecT16(),
                GoldenDuckConfig = new EnumGoldenDuckConfig(),
                MaxPlayers = new U32(MaxPlayers)
            };
            if (InitialReward.HasValue)
            {
                tournamentConfig.InitialReward.Create(new U128(InitialReward.Value));
            }
            if (MaxReward.HasValue)
            {
                tournamentConfig.MaxReward.Create(new U128(MaxReward.Value));
            }
            if (TakeFeePercentage.HasValue)
            {
                tournamentConfig.TakeFeePercentage.Create(new U8(TakeFeePercentage.Value));
            }
            tournamentConfig.RewardDistribution.Value = new BaseVec<U8>(RewardDistribution.Select(p => new U8(p)).ToArray());
            tournamentConfig.GoldenDuckConfig.Create(GoldenDuckConfig.GoldenDuckConfig, GoldenDuckConfig.RewardPercentage != null ? new U8(GoldenDuckConfig.RewardPercentage.Value) : null);

            return tournamentConfig;
        }

        /// <summary>
        /// Start
        /// </summary>
        public uint Start { get; }

        /// <summary>
        /// ActiveEnd
        /// </summary>
        public uint ActiveEnd { get; }

        /// <summary>
        /// ClaimEnd
        /// </summary>
        public uint ClaimEnd { get; }

        /// <summary>
        /// InitialReward
        /// </summary>
        public BigInteger? InitialReward { get; }

        /// <summary>
        /// MaxReward
        /// </summary>
        public BigInteger? MaxReward { get; }

        /// <summary>
        /// TakeFeePercentage
        /// </summary>
        public byte? TakeFeePercentage { get; }

        /// <summary>
        /// RewardDistribution
        /// </summary>
        public byte[] RewardDistribution { get; }

        /// <summary>
        /// GoldenDuckConfig
        /// </summary>
        public GoldenDuckConfigSharp GoldenDuckConfig { get; }

        /// <summary>
        /// MaxPlayers
        /// </summary>
        public uint MaxPlayers { get; }
    }
}