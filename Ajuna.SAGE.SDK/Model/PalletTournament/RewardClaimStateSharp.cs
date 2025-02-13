using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto;

namespace Substrate.Integration.Model
{
    /// <summary>
    /// Wrapped RewardClaimState
    /// </summary>
    public class RewardClaimStateSharp
    {
        /// <summary>
        /// RewardClaimStateSharp constructor
        /// </summary>
        /// <param name="rewardClaimState"></param>
        public RewardClaimStateSharp(EnumRewardClaimState rewardClaimState)
        {
            RewardClaimState = rewardClaimState.Value;
            switch (RewardClaimState)
            {
                case RewardClaimState.Unclaimed:
                    break;

                case RewardClaimState.Claimed:
                    Account = (AccountId32)rewardClaimState.Value2;
                    break;
            }
        }

        /// <summary>
        /// RewardClaimState
        /// </summary>
        public RewardClaimState RewardClaimState { get; }

        /// <summary>
        /// BlockNumber
        /// </summary>
        public AccountId32? Account { get; }
    }
}