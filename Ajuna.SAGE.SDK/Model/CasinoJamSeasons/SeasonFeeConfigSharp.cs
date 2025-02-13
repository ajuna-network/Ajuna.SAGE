using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.ajuna_primitives.season_manager;
using System.Numerics;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSeasons
{
    /// <summary>
    /// SeasonFeeConfigSharp
    /// </summary>
    public class SeasonFeeConfigSharp
    {
        /// <summary>
        /// SeasonFeeConfigSharp constructor
        /// </summary>
        /// <param name="seasonFeeConfig"></param>
        public SeasonFeeConfigSharp(SeasonFeeConfig seasonFeeConfig)
        {
            TransferAsset = seasonFeeConfig.TransferAsset.Value;
            BuyAssetMin = seasonFeeConfig.BuyAssetMin.Value;
            BuyPercent = seasonFeeConfig.BuyPercent.Value;
            UpgradeAssetInventory = seasonFeeConfig.UpgradeAssetInventory.Value;
            UnlockTradeAsset = seasonFeeConfig.UnlockTradeAsset.Value;
            UnlockTransferAsset = seasonFeeConfig.UnlockTransferAsset.Value;
            StateTransitionBaseFee = seasonFeeConfig.StateTransitionBaseFee.Value;

        }

        // Parameterless constructor for deserialization
        public SeasonFeeConfigSharp() { }

        /// <summary>
        /// To substrate
        /// </summary>
        /// <returns></returns>
        public SeasonFeeConfig ToSubstrate()
        {
            var result = new SeasonFeeConfig();
            result.TransferAsset = new Substrate.NetApi.Model.Types.Primitive.U128(TransferAsset);
            result.BuyAssetMin = new Substrate.NetApi.Model.Types.Primitive.U128(BuyAssetMin);
            result.BuyPercent = new Substrate.NetApi.Model.Types.Primitive.U8(BuyPercent);
            result.UpgradeAssetInventory = new Substrate.NetApi.Model.Types.Primitive.U128(UpgradeAssetInventory);
            result.UnlockTradeAsset = new Substrate.NetApi.Model.Types.Primitive.U128(UnlockTradeAsset);
            result.UnlockTransferAsset = new Substrate.NetApi.Model.Types.Primitive.U128(UnlockTransferAsset);
            result.StateTransitionBaseFee = new Substrate.NetApi.Model.Types.Primitive.U128(StateTransitionBaseFee);
            return result;
        }

        /// <summary>
        /// Transfer Asset
        /// </summary>
        public BigInteger TransferAsset { get; set; }

        /// <summary>
        /// Buy Asset Min
        /// </summary>
        public BigInteger BuyAssetMin { get; set; }

        /// <summary>
        /// Buy Percent
        /// </summary>
        public byte BuyPercent { get; set; }

        /// <summary>
        /// Upgrade Asset Inventory
        /// </summary>
        public BigInteger UpgradeAssetInventory { get; set; }

        /// <summary>
        /// Unlock Trade Asset
        /// </summary>
        public BigInteger UnlockTradeAsset { get; set; }

        /// <summary>
        /// Unlock Transfer Asset
        /// </summary>
        public BigInteger UnlockTransferAsset { get; set; }

        /// <summary>
        /// State Transition Base Fee
        /// </summary>
        public BigInteger StateTransitionBaseFee { get; set; }
    }
}