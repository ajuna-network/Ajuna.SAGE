using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition;
using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.transition.enums;
using Substrate.NetApi.Model.Types.Base;
using System;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// AssetTypeSharp
    /// </summary>
    public class AssetTypeSharp
    {
        public AssetTypeSharp(EnumAssetType enumAssetType)
        {
            AssetType = enumAssetType.Value;
            switch (enumAssetType.Value)
            {
                case AssetType.Player:
                    break;
                case AssetType.Machine:
                    MachineType = ((EnumMachineType)enumAssetType.Value2).Value;
                    break;
            }
        }

        // Parameterless constructor for deserialization
        public AssetTypeSharp() { }

        /// <summary>
        /// To substrate
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public EnumAssetType ToSubstrate()
        {
            EnumAssetType result = new EnumAssetType
            {
                Value = AssetType,
            };
            switch (AssetType)
            {
                case AssetType.Player:
                    result.Value2 = new BaseVoid();
                    break;
                case AssetType.Machine:
                    if (MachineType == null)
                    {
                        throw new InvalidOperationException("MachineType is required for AssetType.Machine");
                    }
                    var machineType = new EnumMachineType();
                    machineType.Create(MachineType.Value);
                    result.Value2 = machineType;
                    break;
            }
            return result;
        }

        /// <summary>
        /// AssetType
        /// </summary>
        public AssetType AssetType { get; set; }

        /// <summary>
        /// MachineType
        /// </summary>
        public MachineType? MachineType { get; private set; }
    }
}