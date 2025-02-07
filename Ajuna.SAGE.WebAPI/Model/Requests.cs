using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Game.Model;
using Microsoft.AspNetCore.Mvc;

namespace Ajuna.SAGE.WebAPI.Model
{
    public class CreatePlayerRequest
    {
        public uint Id { get; set; }

        public uint Balance { get; set; }
    }

    public class TransitionRequest
    {
        public uint PlayerId { get; set; }

        public HeroJamIdentifier Identifier { get; set; }

        public uint[]? AssetIds { get; set; }
    }
}
