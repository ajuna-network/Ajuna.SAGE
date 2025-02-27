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


namespace Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.season_manager
{
    
    
    /// <summary>
    /// >> 81 - Composite[ajuna_primitives.season_manager.SeasonFeeConfig]
    /// </summary>
    [SubstrateNodeType(TypeDefEnum.Composite)]
    public sealed class SeasonFeeConfig : BaseType
    {
        
        /// <summary>
        /// >> transfer_asset
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 TransferAsset { get; set; }
        /// <summary>
        /// >> buy_asset_min
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 BuyAssetMin { get; set; }
        /// <summary>
        /// >> buy_percent
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U8 BuyPercent { get; set; }
        /// <summary>
        /// >> upgrade_asset_inventory
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 UpgradeAssetInventory { get; set; }
        /// <summary>
        /// >> unlock_trade_asset
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 UnlockTradeAsset { get; set; }
        /// <summary>
        /// >> unlock_transfer_asset
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 UnlockTransferAsset { get; set; }
        /// <summary>
        /// >> state_transition_base_fee
        /// </summary>
        public Substrate.NetApi.Model.Types.Primitive.U128 StateTransitionBaseFee { get; set; }
        
        /// <inheritdoc/>
        public override string TypeName()
        {
            return "SeasonFeeConfig";
        }
        
        /// <inheritdoc/>
        public override byte[] Encode()
        {
            var result = new List<byte>();
            result.AddRange(TransferAsset.Encode());
            result.AddRange(BuyAssetMin.Encode());
            result.AddRange(BuyPercent.Encode());
            result.AddRange(UpgradeAssetInventory.Encode());
            result.AddRange(UnlockTradeAsset.Encode());
            result.AddRange(UnlockTransferAsset.Encode());
            result.AddRange(StateTransitionBaseFee.Encode());
            return result.ToArray();
        }
        
        /// <inheritdoc/>
        public override void Decode(byte[] byteArray, ref int p)
        {
            var start = p;
            TransferAsset = new Substrate.NetApi.Model.Types.Primitive.U128();
            TransferAsset.Decode(byteArray, ref p);
            BuyAssetMin = new Substrate.NetApi.Model.Types.Primitive.U128();
            BuyAssetMin.Decode(byteArray, ref p);
            BuyPercent = new Substrate.NetApi.Model.Types.Primitive.U8();
            BuyPercent.Decode(byteArray, ref p);
            UpgradeAssetInventory = new Substrate.NetApi.Model.Types.Primitive.U128();
            UpgradeAssetInventory.Decode(byteArray, ref p);
            UnlockTradeAsset = new Substrate.NetApi.Model.Types.Primitive.U128();
            UnlockTradeAsset.Decode(byteArray, ref p);
            UnlockTransferAsset = new Substrate.NetApi.Model.Types.Primitive.U128();
            UnlockTransferAsset.Decode(byteArray, ref p);
            StateTransitionBaseFee = new Substrate.NetApi.Model.Types.Primitive.U128();
            StateTransitionBaseFee.Decode(byteArray, ref p);
            var bytesLength = p - start;
            TypeSize = bytesLength;
            Bytes = new byte[bytesLength];
            global::System.Array.Copy(byteArray, start, Bytes, 0, bytesLength);
        }
    }
}
