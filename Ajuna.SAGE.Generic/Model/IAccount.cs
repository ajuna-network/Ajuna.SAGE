using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.Game.Model
{
    /// <summary>
    /// Player interface
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Id of the player
        /// </summary>
        uint Id { get; set; }

        /// <summary>
        /// Is owner of asset
        /// </summary>
        /// <param name="asset"></param>
        /// <returns></returns>
        bool IsOwnerOf(IAsset asset);

        /// <summary>
        /// Balance of the player
        /// </summary>
        IBalance Balance { get; }
    }
}