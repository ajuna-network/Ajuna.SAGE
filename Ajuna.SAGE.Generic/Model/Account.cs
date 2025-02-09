using Ajuna.SAGE.Game.Model;
using Ajuna.SAGE.Model;

namespace Ajuna.SAGE.Game.Model
{

    /// <summary>
    /// Player class
    /// </summary>
    public class Account : IAccount, IEquatable<Account>
    {
        /// <inheritdoc/>
        public uint Id { get; set; }

        /// <inheritdoc/>
        public ICollection<IAsset>? Assets { get; set; }

        /// <inheritdoc/>
        public IBalance Balance { get; }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="id"></param>
        public Account(uint id, uint balance = 0)
        {
            Id = id;
            Assets = [];
            Balance = new Balance(balance);
        }

        /// <inheritdoc/>
        public bool Equals(Account? other)
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
            //return Assets != null && Assets.Any(a => a.Equals(asset));
            return Id == asset.OwnerId;
        }

        /// <inheritdoc/>
        public IAsset[]? Query(byte[] filter)
        {
            if (Assets == null)
            {
                return null;
            }

            return Assets
                .Where(a => a.Data.Length >= filter.Length && a.Data.AsSpan(0, filter.Length).SequenceEqual(filter))
                .ToArray();
        }

        /// <summary>
        /// Transition
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="outputs"></param>
        public void Transition(IAsset[]? inputs, IAsset[]? outputs)
        {
            Assets ??= [];

            inputs ??= [];
            outputs ??= [];

            // Remove assets present in input but not in output
            foreach (var input in inputs)
            {
                if (!outputs.Any(a => a.Equals(input)))
                {
                    Assets.Remove(input);
                }
            }

            // Add or update assets present in output but not in the current assets
            foreach (var output in outputs)
            {
                var asset = Assets.FirstOrDefault(a => a.Equals(output));
                if (asset == null)
                {
                    Assets.Add(output);
                }
                else
                {
                    // update mutable data fields
                    asset.Score = output.Score;
                    asset.Data = output.Data;
                }
            }
        }

        /// <summary>
        /// Map to domain
        /// </summary>
        /// <param name="dbPlayer"></param>
        /// <returns></returns>
        public static Account MapToDomain(DbPlayer dbPlayer) => 
            new Account(dbPlayer.Id, dbPlayer.BalanceValue)
            {
                Assets = dbPlayer.Assets?
                    .Select(DbAsset => Asset.MapToDomain(DbAsset))
                    .ToList<IAsset>()
            };
    }
}