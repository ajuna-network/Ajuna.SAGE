//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Substrate.AjunaSolo.NET.RestClient.Test.Generated
{
   using System;
   using NUnit.Framework;
   using System.Threading.Tasks;
   using System.Net.Http;
   using Substrate.AjunaSolo.NET.RestClient.Mockup.Generated.Clients;
   using Substrate.AjunaSolo.NET.RestClient.Generated.Clients;
   using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types;
   using Substrate.NetApi.Model.Types.Primitive;
   
   public class AssetsControllerClientTest : ClientTestBase
   {
      private System.Net.Http.HttpClient _httpClient;
      [SetUp()]
      public void Setup()
      {
         _httpClient = CreateHttpClient();
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetDetails GetTestValue2()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetDetails result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetDetails();
         result.Owner = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Owner = this.GetTestValue3();
         result.Issuer = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Issuer = this.GetTestValue4();
         result.Admin = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Admin = this.GetTestValue5();
         result.Freezer = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Freezer = this.GetTestValue6();
         result.Supply = this.GetTestValueU128();
         result.Deposit = this.GetTestValueU128();
         result.MinBalance = this.GetTestValueU128();
         result.IsSufficient = this.GetTestValueBool();
         result.Accounts = this.GetTestValueU32();
         result.Sufficients = this.GetTestValueU32();
         result.Approvals = this.GetTestValueU32();
         result.Status = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumAssetStatus();
         result.Status.Create(this.GetTestValueEnum<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetStatus>());
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue3()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue4()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue5()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue6()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      [Test()]
      public async System.Threading.Tasks.Task TestAsset()
      {
         // Construct new Mockup client to test with.
         AssetsControllerMockupClient mockupClient = new AssetsControllerMockupClient(_httpClient);

         // Construct new subscription client to test with.
         var subscriptionClient = CreateSubscriptionClient();

         // Construct new RPC client to test with.
         AssetsControllerClient rpcClient = new AssetsControllerClient(_httpClient, subscriptionClient);
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetDetails mockupValue = this.GetTestValue2();
         Substrate.NetApi.Model.Types.Primitive.U32 mockupKey = this.GetTestValueU32();

         Assert.IsTrue(await rpcClient.SubscribeAsset(mockupKey));

         // Save the previously generated mockup value in RPC service storage.
         bool mockupSetResult = await mockupClient.SetAsset(mockupValue, mockupKey);

         // Test that the expected mockup value was handled successfully from RPC service.
         Assert.IsTrue(mockupSetResult);
         var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(1));
         Assert.IsTrue(await subscriptionClient.ReceiveNextAsync(cts.Token));

         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetDetails rpcResult = await rpcClient.GetAsset(mockupKey);

         // Test that the expected mockup value matches the actual result from RPC service.
         Assert.AreEqual(mockupValue.Encode(), rpcResult.Encode());
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetAccount GetTestValue8()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetAccount result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetAccount();
         result.Balance = this.GetTestValueU128();
         result.Status = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumAccountStatus();
         result.Status.Create(this.GetTestValueEnum<Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AccountStatus>());
         result.Reason = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumExistenceReason();
         result.Reason = this.GetTestValue9();
         result.Extra = new Substrate.NetApi.Model.Types.Base.BaseTuple();
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumExistenceReason GetTestValue9()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumExistenceReason result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumExistenceReason();
         // NOT IMPLEMENTED >> Initialize Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.EnumExistenceReason
         return result;
      }
      public Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> GetTestValue10()
      {
         Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> result;
         result = new Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>();
         result.Create(this.GetTestValueU32(), this.GetTestValue11());
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue11()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      [Test()]
      public async System.Threading.Tasks.Task TestAccount()
      {
         // Construct new Mockup client to test with.
         AssetsControllerMockupClient mockupClient = new AssetsControllerMockupClient(_httpClient);

         // Construct new subscription client to test with.
         var subscriptionClient = CreateSubscriptionClient();

         // Construct new RPC client to test with.
         AssetsControllerClient rpcClient = new AssetsControllerClient(_httpClient, subscriptionClient);
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetAccount mockupValue = this.GetTestValue8();
         Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> mockupKey = this.GetTestValue10();

         Assert.IsTrue(await rpcClient.SubscribeAccount(mockupKey));

         // Save the previously generated mockup value in RPC service storage.
         bool mockupSetResult = await mockupClient.SetAccount(mockupValue, mockupKey);

         // Test that the expected mockup value was handled successfully from RPC service.
         Assert.IsTrue(mockupSetResult);
         var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(1));
         Assert.IsTrue(await subscriptionClient.ReceiveNextAsync(cts.Token));

         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetAccount rpcResult = await rpcClient.GetAccount(mockupKey);

         // Test that the expected mockup value matches the actual result from RPC service.
         Assert.AreEqual(mockupValue.Encode(), rpcResult.Encode());
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.Approval GetTestValue13()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.Approval result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.Approval();
         result.Amount = this.GetTestValueU128();
         result.Deposit = this.GetTestValueU128();
         return result;
      }
      public Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> GetTestValue14()
      {
         Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> result;
         result = new Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32>();
         result.Create(this.GetTestValueU32(), this.GetTestValue15(), this.GetTestValue16());
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue15()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 GetTestValue16()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32();
         result.Value = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Types.Base.Arr32U8();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8(),
                  this.GetTestValueU8()});
         return result;
      }
      [Test()]
      public async System.Threading.Tasks.Task TestApprovals()
      {
         // Construct new Mockup client to test with.
         AssetsControllerMockupClient mockupClient = new AssetsControllerMockupClient(_httpClient);

         // Construct new subscription client to test with.
         var subscriptionClient = CreateSubscriptionClient();

         // Construct new RPC client to test with.
         AssetsControllerClient rpcClient = new AssetsControllerClient(_httpClient, subscriptionClient);
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.Approval mockupValue = this.GetTestValue13();
         Substrate.NetApi.Model.Types.Base.BaseTuple<Substrate.NetApi.Model.Types.Primitive.U32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32, Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.sp_core.crypto.AccountId32> mockupKey = this.GetTestValue14();

         Assert.IsTrue(await rpcClient.SubscribeApprovals(mockupKey));

         // Save the previously generated mockup value in RPC service storage.
         bool mockupSetResult = await mockupClient.SetApprovals(mockupValue, mockupKey);

         // Test that the expected mockup value was handled successfully from RPC service.
         Assert.IsTrue(mockupSetResult);
         var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(1));
         Assert.IsTrue(await subscriptionClient.ReceiveNextAsync(cts.Token));

         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.Approval rpcResult = await rpcClient.GetApprovals(mockupKey);

         // Test that the expected mockup value matches the actual result from RPC service.
         Assert.AreEqual(mockupValue.Encode(), rpcResult.Encode());
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetMetadata GetTestValue18()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetMetadata result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetMetadata();
         result.Deposit = this.GetTestValueU128();
         result.Name = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
         result.Name = this.GetTestValue19();
         result.Symbol = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
         result.Symbol = this.GetTestValue20();
         result.Decimals = this.GetTestValueU8();
         result.IsFrozen = this.GetTestValueBool();
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 GetTestValue19()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
         result.Value = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8()});
         return result;
      }
      public Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 GetTestValue20()
      {
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8 result;
         result = new Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.bounded_collections.bounded_vec.BoundedVecT8();
         result.Value = new Substrate.NetApi.Model.Types.Base.BaseVec<Substrate.NetApi.Model.Types.Primitive.U8>();
         result.Value.Create(new Substrate.NetApi.Model.Types.Primitive.U8[] {
                  this.GetTestValueU8()});
         return result;
      }
      [Test()]
      public async System.Threading.Tasks.Task TestMetadata()
      {
         // Construct new Mockup client to test with.
         AssetsControllerMockupClient mockupClient = new AssetsControllerMockupClient(_httpClient);

         // Construct new subscription client to test with.
         var subscriptionClient = CreateSubscriptionClient();

         // Construct new RPC client to test with.
         AssetsControllerClient rpcClient = new AssetsControllerClient(_httpClient, subscriptionClient);
         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetMetadata mockupValue = this.GetTestValue18();
         Substrate.NetApi.Model.Types.Primitive.U32 mockupKey = this.GetTestValueU32();

         Assert.IsTrue(await rpcClient.SubscribeMetadata(mockupKey));

         // Save the previously generated mockup value in RPC service storage.
         bool mockupSetResult = await mockupClient.SetMetadata(mockupValue, mockupKey);

         // Test that the expected mockup value was handled successfully from RPC service.
         Assert.IsTrue(mockupSetResult);
         var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(1));
         Assert.IsTrue(await subscriptionClient.ReceiveNextAsync(cts.Token));

         Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.pallet_assets.types.AssetMetadata rpcResult = await rpcClient.GetMetadata(mockupKey);

         // Test that the expected mockup value matches the actual result from RPC service.
         Assert.AreEqual(mockupValue.Encode(), rpcResult.Encode());
      }
      [Test()]
      public async System.Threading.Tasks.Task TestNextAssetId()
      {
         // Construct new Mockup client to test with.
         AssetsControllerMockupClient mockupClient = new AssetsControllerMockupClient(_httpClient);

         // Construct new subscription client to test with.
         var subscriptionClient = CreateSubscriptionClient();

         // Construct new RPC client to test with.
         AssetsControllerClient rpcClient = new AssetsControllerClient(_httpClient, subscriptionClient);
         Substrate.NetApi.Model.Types.Primitive.U32 mockupValue = this.GetTestValueU32();


         Assert.IsTrue(await rpcClient.SubscribeNextAssetId());

         // Save the previously generated mockup value in RPC service storage.
         bool mockupSetResult = await mockupClient.SetNextAssetId(mockupValue);

         // Test that the expected mockup value was handled successfully from RPC service.
         Assert.IsTrue(mockupSetResult);
         var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(1));
         Assert.IsTrue(await subscriptionClient.ReceiveNextAsync(cts.Token));

         Substrate.NetApi.Model.Types.Primitive.U32 rpcResult = await rpcClient.GetNextAssetId();

         // Test that the expected mockup value matches the actual result from RPC service.
         Assert.AreEqual(mockupValue.Encode(), rpcResult.Encode());
      }
   }
}
