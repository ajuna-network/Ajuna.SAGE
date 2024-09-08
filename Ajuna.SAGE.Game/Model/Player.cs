namespace Ajuna.SAGE.Generic.Model
{
    /// <summary>
    /// Player interface
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Id of the player
        /// </summary>
        byte[] Id { get; set; }

        /// <summary>
        /// Assets of the player
        /// </summary>
        ICollection<IAsset>? Assets { get; set; }

        /// <summary>
        /// Is owner of asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        bool IsOwnerOf(IAsset asset);
    }

    /// <summary>
    /// Player class
    /// </summary>
    public class Player : IPlayer, IEquatable<Player>
    {
        /// <inheritdoc/>
        public byte[] Id { get; set; }

        /// <inheritdoc/>
        public ICollection<IAsset>? Assets { get; set; }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="id"></param>
        public Player(byte[] id)
        {
            Id = id;
            Assets = [];
        }

        /// <inheritdoc/>
        public bool Equals(Player? other)
        {
            if (other == null)
            {
                return false;
            }

            return Id.SequenceEqual(other.Id);
        }

        /// <inheritdoc/>
        public bool IsOwnerOf(IAsset asset)
        {
           return Assets != null && Assets.Any(a => a.Equals(asset));
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void Transition(IAsset[]? input, IAsset[]? output)
        {
            Assets ??= [];

            input ??= [];
            output ??= [];

            // Remove assets present in input but not in output
            foreach (var asset in input)
            {
                if (!output.Any(a => a.Equals(asset)))
                {
                    Assets.Remove(asset);
                }
            }

            // Add or update assets present in output but not in the current assets
            foreach (var asset in output)
            {
                var existingAsset = Assets.FirstOrDefault(a => a.Equals(asset));
                if (existingAsset == null)
                {
                    Assets.Add(asset);
                }
                else
                {
                    // TODO: check if we need to updated somethign here
                }
            }
        }
    }
}