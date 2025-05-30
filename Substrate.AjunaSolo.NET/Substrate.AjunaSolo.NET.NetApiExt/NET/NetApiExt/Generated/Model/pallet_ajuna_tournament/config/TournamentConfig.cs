//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Attributes;
using Substrate.NetApi.Model.Types.Base;
using Substrate.NetApi.Model.Types.Metadata.Base;
using System.Collections.Generic;


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config
{
    
    
    /// <summary>
    /// >> 259 - Composite[pallet_ajuna_tournament.config.TournamentConfig]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class TournamentConfig : BaseType
    {
        
        /// <summary>
        /// >> start
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 Start { get; set; }
        /// <summary>
        /// >> active_end
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ActiveEnd { get; set; }
        /// <summary>
        /// >> claim_end
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 ClaimEnd { get; set; }
        /// <summary>
        /// >> initial_reward
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> InitialReward { get; set; }
        /// <summary>
        /// >> max_reward
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128> MaxReward { get; set; }
        /// <summary>
        /// >> take_fee_percentage
        /// </summary>
        public Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U8> TakeFeePercentage { get; set; }
        /// <summary>
        /// >> reward_distribution
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT16 RewardDistribution { get; set; }
        /// <summary>
        /// >> golden_duck_config
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config.EnumGoldenDuckConfig GoldenDuckConfig { get; set; }
        /// <summary>
        /// >> max_players
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U32 MaxPlayers { get; set; }
        /// <summary>
        /// >> ranker
        /// </summary>
        public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.sage.casino_jam.CasinoJamEntityRanker Ranker { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "TournamentConfig";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(Start.Encode());
            result.AddRange(ActiveEnd.Encode());
            result.AddRange(ClaimEnd.Encode());
            result.AddRange(InitialReward.Encode());
            result.AddRange(MaxReward.Encode());
            result.AddRange(TakeFeePercentage.Encode());
            result.AddRange(RewardDistribution.Encode());
            result.AddRange(GoldenDuckConfig.Encode());
            result.AddRange(MaxPlayers.Encode());
            result.AddRange(Ranker.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            Start = new Substrate.NetApi.Model.Types.Primitive.U32();
            Start.Decode(byteArray, ref p);
            ActiveEnd = new Substrate.NetApi.Model.Types.Primitive.U32();
            ActiveEnd.Decode(byteArray, ref p);
            ClaimEnd = new Substrate.NetApi.Model.Types.Primitive.U32();
            ClaimEnd.Decode(byteArray, ref p);
            InitialReward = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128>();
            InitialReward.Decode(byteArray, ref p);
            MaxReward = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U128>();
            MaxReward.Decode(byteArray, ref p);
            TakeFeePercentage = new Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.NetApi.Model.Types.Primitive.U8>();
            TakeFeePercentage.Decode(byteArray, ref p);
            RewardDistribution = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT16();
            RewardDistribution.Decode(byteArray, ref p);
            GoldenDuckConfig = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_ajuna_tournament.config.EnumGoldenDuckConfig();
            GoldenDuckConfig.Decode(byteArray, ref p);
            MaxPlayers = new Substrate.NetApi.Model.Types.Primitive.U32();
            MaxPlayers.Decode(byteArray, ref p);
            Ranker = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.sage.casino_jam.CasinoJamEntityRanker();
            Ranker.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
