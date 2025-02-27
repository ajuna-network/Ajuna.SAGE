//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Substrate.NetApi.Model.Types.Base;
using System.Collections.Generic;


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_collective.pallet
{
    
    
    /// <summary>
    /// >> Call
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public enum Call
    {
        
        /// <summary>
        /// >> set_members
        /// Set the collective's membership.
        /// 
        /// - `new_members`: The new member list. Be nice to the chain and provide it sorted.
        /// - `prime`: The prime member whose vote sets the default.
        /// - `old_count`: The upper bound for the previous number of members in storage. Used for
        ///   weight estimation.
        /// 
        /// The dispatch of this call must be `SetMembersOrigin`.
        /// 
        /// NOTE: Does not enforce the expected `MaxMembers` limit on the amount of members, but
        ///       the weight estimations rely on it to estimate dispatchable weight.
        /// 
        /// # WARNING:
        /// 
        /// The `pallet-collective` can also be managed by logic outside of the pallet through the
        /// implementation of the trait [`ChangeMembers`].
        /// Any call to `set_members` must be careful that the member set doesn't get out of sync
        /// with other logic managing the member set.
        /// 
        /// ## Complexity:
        /// - `O(MP + N)` where:
        ///   - `M` old-members-count (code- and governance-bounded)
        ///   - `N` new-members-count (code- and governance-bounded)
        ///   - `P` proposals-count (code-bounded)
        /// </summary>
        set_members = 0,
        
        /// <summary>
        /// >> execute
        /// Dispatch a proposal from a member using the `Member` origin.
        /// 
        /// Origin must be a member of the collective.
        /// 
        /// ## Complexity:
        /// - `O(B + M + P)` where:
        /// - `B` is `proposal` size in bytes (length-fee-bounded)
        /// - `M` members-count (code-bounded)
        /// - `P` complexity of dispatching `proposal`
        /// </summary>
        execute = 1,
        
        /// <summary>
        /// >> propose
        /// Add a new proposal to either be voted on or executed directly.
        /// 
        /// Requires the sender to be member.
        /// 
        /// `threshold` determines whether `proposal` is executed directly (`threshold < 2`)
        /// or put up for voting.
        /// 
        /// ## Complexity
        /// - `O(B + M + P1)` or `O(B + M + P2)` where:
        ///   - `B` is `proposal` size in bytes (length-fee-bounded)
        ///   - `M` is members-count (code- and governance-bounded)
        ///   - branching is influenced by `threshold` where:
        ///     - `P1` is proposal execution complexity (`threshold < 2`)
        ///     - `P2` is proposals-count (code-bounded) (`threshold >= 2`)
        /// </summary>
        propose = 2,
        
        /// <summary>
        /// >> vote
        /// Add an aye or nay vote for the sender to the given proposal.
        /// 
        /// Requires the sender to be a member.
        /// 
        /// Transaction fees will be waived if the member is voting on any particular proposal
        /// for the first time and the call is successful. Subsequent vote changes will charge a
        /// fee.
        /// ## Complexity
        /// - `O(M)` where `M` is members-count (code- and governance-bounded)
        /// </summary>
        vote = 3,
        
        /// <summary>
        /// >> disapprove_proposal
        /// Disapprove a proposal, close, and remove it from the system, regardless of its current
        /// state.
        /// 
        /// Must be called by the Root origin.
        /// 
        /// Parameters:
        /// * `proposal_hash`: The hash of the proposal that should be disapproved.
        /// 
        /// ## Complexity
        /// O(P) where P is the number of max proposals
        /// </summary>
        disapprove_proposal = 5,
        
        /// <summary>
        /// >> close
        /// Close a vote that is either approved, disapproved or whose voting period has ended.
        /// 
        /// May be called by any signed account in order to finish voting and close the proposal.
        /// 
        /// If called before the end of the voting period it will only close the vote if it is
        /// has enough votes to be approved or disapproved.
        /// 
        /// If called after the end of the voting period abstentions are counted as rejections
        /// unless there is a prime member set and the prime member cast an approval.
        /// 
        /// If the close operation completes successfully with disapproval, the transaction fee will
        /// be waived. Otherwise execution of the approved operation will be charged to the caller.
        /// 
        /// + `proposal_weight_bound`: The maximum amount of weight consumed by executing the closed
        /// proposal.
        /// + `length_bound`: The upper bound for the length of the proposal in storage. Checked via
        /// `storage::read` so it is `size_of::<u32>() == 4` larger than the pure length.
        /// 
        /// ## Complexity
        /// - `O(B + M + P1 + P2)` where:
        ///   - `B` is `proposal` size in bytes (length-fee-bounded)
        ///   - `M` is members-count (code- and governance-bounded)
        ///   - `P1` is the complexity of `proposal` preimage.
        ///   - `P2` is proposal-count (code-bounded)
        /// </summary>
        close = 6,
    }
    
    /// <summary>
    /// >> 184 - Variant[pallet_collective.pallet.Call]
    /// Contains a variant per dispatchable extrinsic that this pallet has.
    /// </summary>
    public sealed class EnumCall : BaseEnumRust<Call>
    {
        
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public EnumCall()
        {
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, Substrate.NetApi.Model.Types.Base.BaseOpt<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>, Substrate.NetApi.Model.Types.Primitive.U32>>(Call.set_members);
				AddTypeDecoder<BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.EnumRuntimeCall, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>>(Call.execute);
				AddTypeDecoder<BaseTuple<Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_solo_runtime.EnumRuntimeCall, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>>(Call.propose);
				AddTypeDecoder<BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types.H256, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.NetApi.Model.Types.Primitive.Bool>>(Call.vote);
				AddTypeDecoder<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types.H256>(Call.disapprove_proposal);
				AddTypeDecoder<BaseTuple<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.primitive_types.H256, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_weights.weight_v2.Weight, Substrate.NetApi.Model.Types.Base.BaseCom<Substrate.NetApi.Model.Types.Primitive.U32>>>(Call.close);
        }
    }
}
