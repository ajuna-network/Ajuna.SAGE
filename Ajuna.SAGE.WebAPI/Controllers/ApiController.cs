using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Generic.Model;
using Ajuna.SAGE.Model;
using Ajuna.SAGE.WebAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Ajuna.SAGE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private const double BLOCKTIME_SEC = 6;

        private readonly ApiContext _context;

        private Engine<HeroJamIdentifier, HeroJamRule>? _engine;

        public ApiController(ApiContext context)
        {
            // create engine
            var randomSeed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
            var blockchainProvider = new BlockchainInfoProvider(randomSeed);
            _engine = HeroJameGame.Create(blockchainProvider);

            _context = context;
            _context.Configs.Add(new DbConfig { Genesis = DateTime.Now });
            _context.SaveChanges();
        }

        [HttpGet("HeroJam")]
        public IActionResult HeroJam(int? seed = null)
        {
            seed ??= RandomNumberGenerator.GetInt32(0, int.MaxValue);

            _engine = HeroJameGame.Create(new BlockchainInfoProvider(seed.Value));

            return Ok($"Successfully set new engine!");
        }

        [HttpGet("BlockNumber")]
        public IActionResult BlockNumber()
        {
            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return new JsonResult(NotFound("No genesis block found!"));
            }

            return Ok(CurrentBlockNumber(config.Genesis));
        }

        [HttpPost("CreatePlayer")]
        public IActionResult CreatePlayer([FromBody] CreatePlayerRequest request)
        {
            var existingPlayer = _context.Players.FirstOrDefault(p => p.Id == request.Id);
            if (existingPlayer != null)
            {
                return Conflict($"A player with Id={request.Id} already exists.");
            }

            var newDbPlayer = new DbPlayer
            {
                Id = request.Id,
                BalanceValue = request.Balance,
                Assets = []
            };

            _context.Players.Add(newDbPlayer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(DbPlayer), new { playerId = newDbPlayer.Id }, newDbPlayer);
        }

        [HttpGet("Player")]
        public IActionResult DbPlayer(ulong playerId)
        {
            var indDbPlayer = _context.Players
                .Include(p => p.Assets)
                .FirstOrDefault(p => p.Id == playerId);

            if (indDbPlayer == null)
            {
                return new JsonResult(NotFound("No player found!"));
            }

            return Ok(indDbPlayer);
        }

        [HttpGet("Transition")]
        public IActionResult Transition(ulong inDbPlayerId, HeroJamIdentifier identifier, [FromQuery] ulong[] inDbAssetIds)
        {
            if (_engine == null)
            {
                return NotFound("Game engine is not setup!");
            }

            var config = _context.Configs.FirstOrDefault();
            if (config == null)
            {
                return NotFound("DbConfig doesn't exist!");
            }

            var inDbPlayer = _context.Players
                .Include(p => p.Assets)
                .Where(p => p.Id == inDbPlayerId)
                .FirstOrDefault();

            if (inDbPlayer == null)
            {
                return NotFound($"Player with id={inDbPlayerId} doesn't exist!");
            }

            var inDbAssets = new List<DbAsset>();
            var assets = new List<IAsset>();

            foreach (var inDbAssetId in inDbAssetIds)
            {
                var inDbAsset = _context.Assets.FirstOrDefault(p => p.Id == inDbAssetId);
                if (inDbAsset == null)
                {
                    return NotFound($"Asset with id={inDbAssetId} doesn't exist!");
                }

                inDbAssets.Add(inDbAsset);

                assets.Add(Asset.MapToDomain(inDbAsset));
            }

            var player = Player.MapToDomain(inDbPlayer);

            // TODO: make sure no trade assets
            // TODO: make sure no locked assets

            var flag = _engine.Transition(player, identifier, [.. assets], out IAsset[] result);

            if (!flag)
            {
                return Conflict("Transition failed!");
            }

            // remove all assets from the player, that went into the transition
            inDbAssets.ForEach(p => inDbPlayer.Assets.Remove(p));

            // Add the resulting assets to the player
            foreach (var avatar in result)
            {
                inDbPlayer.Assets.Add(SAGE.Model.DbAsset.MapToDb(avatar));
            }

            // add all result cards and save
            _context.SaveChanges();

            return Ok(flag);
        }

        [HttpGet("Asset")]
        public IActionResult DbAsset(ulong assetId)
        {
            var inDbAsset = _context.Assets.FirstOrDefault(p => p.Id == assetId);

            if (inDbAsset == null)
            {
                return new JsonResult(NotFound($"Asset with id={assetId} doesn't exist!"));
            }

            return Ok(inDbAsset);
        }

        private uint CurrentBlockNumber(DateTime genesis)
        {
            DateTime now = DateTime.Now;
            var currentBlockNumber = Math.Floor(now.Subtract(genesis).TotalSeconds / BLOCKTIME_SEC);
            return Convert.ToUInt32(currentBlockNumber);
        }
    }
}