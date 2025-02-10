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
        public IBalance Balance { get; }

        /// <summary>
        /// Player constructor
        /// </summary>
        /// <param name="id"></param>
        public Account(uint id, uint balance = 0)
        {
            Id = id;
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

        /// <summary>
        /// Map to domain
        /// </summary>
        /// <param name="dbPlayer"></param>
        /// <returns></returns>
        public static Account MapToDomain(DbPlayer dbPlayer) => 
            new Account(dbPlayer.Id, dbPlayer.BalanceValue) {};
    }
}