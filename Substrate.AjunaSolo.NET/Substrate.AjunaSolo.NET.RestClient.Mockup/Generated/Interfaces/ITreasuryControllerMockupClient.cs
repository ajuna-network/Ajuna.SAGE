//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Substrate.AjunaSolo.NET.RestClient.Mockup.Generated.Interfaces
{
   using System;
   using System.Threading.Tasks;
   using Substrate.NetApi.Model.Types.Primitive;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_treasury;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec;
   
   public interface ITreasuryControllerMockupClient
   {
      Task<bool> SetProposalCount(U32 value);
      Task<bool> SetProposals(Proposal value, U32 key);
      Task<bool> SetDeactivated(U128 value);
      Task<bool> SetApprovals(BoundedVecT17 value);
      Task<bool> SetSpendCount(U32 value);
      Task<bool> SetSpends(SpendStatus value, U32 key);
   }
}
