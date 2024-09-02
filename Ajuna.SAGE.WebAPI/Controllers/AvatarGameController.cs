using Ajuna.SAGE.Game.Algo;
using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Algo;
using Ajuna.SAGE.Generic.Model;
using Ajuna.SAGE.Model;
using Ajuna.SAGE.WebAPI.Data;
using Ajuna.SAGE.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajuna.SAGE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvatarGameController : ControllerBase
    {
        private const double BLOCKTIME_SEC = 6;

        private readonly ApiContext _context;

        private Engine<HeroJamIdentifier, HeroJamRule> _engine;

        public AvatarGameController(ApiContext context)
        {
            _context = context;
            _engine = new Engine<HeroJamIdentifier, HeroJamRule>(new BlockchainInfoProvider(1234), (p, r, a, b) => true);

            _context.Configs.Add(new DbConfig { Genesis = DateTime.Now });

            _context.Players.Add(new DbPlayer() { Name = "Cedric" });
            _context.Players.Add(new DbPlayer() { Name = "Theo" });
            _context.Players.Add(new DbPlayer() { Name = "Luu" });

            _context.SaveChanges();
        }

        // Check this if you need to add some stuff https://www.youtube.com/watch?v=Tj3qsKSNvMk

        [HttpGet("Seed")]
        public JsonResult Seed(int seed)
        {
            _engine = new Engine<HeroJamIdentifier, HeroJamRule>(new BlockchainInfoProvider(seed), (p, r, a, b) => true);

            _context.Configs.Add(new DbConfig { Genesis = DateTime.Now });

            _context.Players.Add(new DbPlayer() { Name = "Cedric" });
            _context.Players.Add(new DbPlayer() { Name = "Theo" });
            _context.Players.Add(new DbPlayer() { Name = "Test1" });
            _context.Players.Add(new DbPlayer() { Name = "Test2" });
            _context.Players.Add(new DbPlayer() { Name = "Test3" });
            _context.SaveChanges();

            return new JsonResult(Ok($"Successfully set new seed!"));
        }

        [HttpGet("Genesis")]
        public JsonResult Genesis()
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }   

            return new JsonResult(Ok(config.Genesis));
        }

        [HttpGet("BlockNumber")]
        public JsonResult BlockNumber()
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }

            return new JsonResult(Ok(CurrentBlockNumber(config.Genesis)));
        }

        [HttpGet("Match")]
        public JsonResult Match(MatchRules rules, [FromQuery] string[] datas)
        {
            var dataBytes = new List<byte[]>();
            foreach (var data in datas)
            {
                try
                {
                    dataBytes.Add(Utils.HexToBytes(data));
                }
                catch (Exception)
                {
                    return new JsonResult(BadRequest($"Invalid hex dna."));
                }
            }

            if (dataBytes.Count < 2 || dataBytes.Count > 5)
            {
                return new JsonResult(BadRequest($"Wrong amount of DNAs only leader and 1-4 sacrifices."));
            }

            var main = dataBytes.First();
            var secondaries = dataBytes.Skip(1);

            var result = new List<MatchResultJson>();

            foreach (var secondary in secondaries)
            {
                var flag = MatchingAlgorithm.IsMatching(rules, main, secondary, out MatchResult matchResult);

                result.Add(
                    new MatchResultJson(
                        leader: Convert.ToHexString(main),
                        sacrifice: Convert.ToHexString(secondary),
                        matchResult: matchResult));
            }

            return new JsonResult(Ok(result));
        }

        [HttpGet("Player")]
        public JsonResult Player(int playerId)
        {
            var inDbPlayer = _context.Players.Include(p => p.Cards).Where(p => p.Id == playerId).FirstOrDefault();

            if (inDbPlayer == null)
            {
                return new JsonResult(NotFound("No player found!"));
            }

            return new JsonResult(Ok(inDbPlayer));
        }

        [HttpGet("Reset")]
        public JsonResult Reset(int playerId)
        {
            var inDbPlayer = _context.Players
                .Include(p => p.Cards)
                .Where(p => p.Id == playerId).FirstOrDefault();

            if (inDbPlayer == null)
            {
                return new JsonResult(NotFound("No player found!"));
            }

            inDbPlayer.Cards.Clear();

            _context.SaveChanges();

            return new JsonResult(Ok($"Inventory of player with id={playerId} reseted."));
        }

        [HttpGet("Transition")]
        public JsonResult Transition(int playerId, HeroJamIdentifier identifier, [FromQuery] string[] assetIds)
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }

            var inDbPlayer = _context.Players.Include(p => p.Cards).Where(p => p.Id == playerId).FirstOrDefault();

            if (inDbPlayer == null)
            {
                return new JsonResult(NotFound($"Player with id={playerId} doesn't exist!"));
            }
            var inCards = new List<DbAsset>();
            var assets = new List<Asset>();

            foreach (var assetId in assetIds)
            {
                var card = inDbPlayer.Cards.FirstOrDefault(p => p.Id == assetId);
                if (card == null)
                {
                    return new JsonResult(NotFound($"Card with id={assetId} in player with id={playerId} inventory!"));
                }

                inCards.Add(card);

                assets.Add(new Asset(
                    Utils.HexToBytes(card.Id),
                    card.CollectionId,
                    card.Score,
                    card.Dna));
            }

            var player = new Player(BitConverter.GetBytes(inDbPlayer.Id));

            var flag = _engine.Transition(player, identifier, assets.ToArray(), out Asset[] result);

            inCards.ForEach(p => inDbPlayer.Cards.Remove(p));

            foreach (var avatar in result)
            {
                inDbPlayer.Cards.Add(
                    new DbAsset()
                    {
                        Id = Convert.ToHexString(avatar.Id),
                        CollectionId = avatar.CollectionId,
                        Score = avatar.Score,
                        Dna = avatar.Data.ToHexDna()
                    });
            }

            // add all result cards and save
            _context.SaveChanges();

            return new JsonResult(Ok(result));
        }

        [HttpGet("Asset")]
        public JsonResult Asset(string assetId)
        {
            var inDbCard = _context.Assets.Where(p => p.Id == assetId).FirstOrDefault();

            if (inDbCard == null)
            {
                return new JsonResult(NotFound($"Card with id={assetId} doesn't exist!"));
            }

            var asset = new Asset(Utils.HexToBytes(inDbCard.Id),
                                inDbCard.CollectionId,
                                inDbCard.Score,
                                inDbCard.Dna);

            var wrappedAsset = new WrappedAsset(asset);

            return new JsonResult(Ok(wrappedAsset));
        }

        private uint CurrentBlockNumber(DateTime genesis)
        {
            DateTime now = DateTime.Now;
            var currentBlockNumber = Math.Floor(now.Subtract(genesis).TotalSeconds / BLOCKTIME_SEC);
            return Convert.ToUInt32(currentBlockNumber);
        }
    }
}