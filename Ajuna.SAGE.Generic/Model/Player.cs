using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Model;

namespace Ajuna.SAGE.Generic.Model
{

    /// <summary>
    /// Player class
    /// </summary>
    public class Player : IPlayer, IEquatable<Player>
    {
        /// <inheritdoc/>
        public ulong Id { get; set; }

        /// <inheritdoc/>
        public ICollection<IAsset>? Assets { get; set; }

        /// <inheritdoc/>
        public IBalance Balance { get; }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="id"></param>
        public Player(ulong id, uint balance = 0)
        {
            Id = id;
            Assets = [];
            Balance = new Balance(balance);
        }

        /// <inheritdoc/>
        public bool Equals(Player? other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id;
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

        /// <summary>
        /// Map to domain
        /// </summary>
        /// <param name="dbPlayer"></param>
        /// <returns></returns>
        public static Player MapToDomain(DbPlayer dbPlayer) => 
            new Player(dbPlayer.Id, dbPlayer.BalanceValue)
            {
                Assets = dbPlayer.Assets?
                    .Select(DbAsset => Asset.MapToDomain(DbAsset))
                    .ToList<IAsset>()
            };
    }
}