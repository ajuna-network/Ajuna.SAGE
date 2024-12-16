using Ajuna.SAGE.Game.Model;

namespace Ajuna.SAGE.WebAPI.Model
{
    public class CreatePlayerRequest
    {
        public ulong Id { get; set; }

        public uint Balance { get; set; }
    }
}
