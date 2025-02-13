using Substrate.AjunaSolo.NET.NetApiExt.Generated.Model.example_transition.asset;

namespace Ajuna.SAGE.SDK.Model.CasinoJamSage
{
    /// <summary>
    /// AssetSharp
    /// </summary>
    public class AssetSharp
    {
        /// <summary>
        /// Asset
        /// </summary>
        /// <param name="asset"></param>
        public AssetSharp(Asset asset)
        {
            Id = asset.Id.Value;
            CollectionId = asset.CollectionId.Value;
            Genesis = asset.Genesis.Value;
            Variant = new AssetVariantSharp(asset.Variant);

        }

        // Parameterless constructor for deserialization
        public AssetSharp() { }

        /// <summary>
        /// Id
        /// </summary>
        public uint Id { get; private set; }

        /// <summary>
        /// CollectionId
        /// </summary>
        public byte CollectionId { get; private set; }

        /// <summary>
        /// Genesis
        /// </summary>
        public uint Genesis { get; private set; }

        /// <summary>
        /// Variant
        /// </summary>
        public AssetVariantSharp Variant { get; private set; }
    }
}