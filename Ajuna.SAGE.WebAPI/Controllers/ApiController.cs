using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Game;
using Ajuna.SAGE.Game.Model;
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

        private Engine<HeroJamIdentifier, HeroJamRule> _engine;

        public ApiController(ApiContext context, Engine<HeroJamIdentifier, HeroJamRule> engine)
        {
            _context = context;
            _engine = engine;
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
                return NotFound("No genesis block found!");
            }

            _engine.BlockchainInfoProvider.CurrentBlockNumber = CurrentBlockNumber(config.Genesis);

            return Ok(_engine.BlockchainInfoProvider.CurrentBlockNumber);
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
                Assets = new List<DbAsset>()
            };

            _context.Players.Add(newDbPlayer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(DbPlayer), new { playerId = newDbPlayer.Id }, newDbPlayer);
        }

        [HttpGet("Player/{playerId}")]
        public IActionResult DbPlayer(ulong playerId)
        {
            var indDbPlayer = _context.Players
                .Include(p => p.Assets)
                .FirstOrDefault(p => p.Id == playerId);

            if (indDbPlayer == null)
            {
                return NotFound("No player found!");
            }

            return Ok(indDbPlayer);
        }

        [HttpPost("Transition")]
        public IActionResult Transition([FromBody] TransitionRequest request)
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

            // set the current block number
            _engine.BlockchainInfoProvider.CurrentBlockNumber = CurrentBlockNumber(config.Genesis);

            var inDbPlayer = _context.Players
                .Include(p => p.Assets)
                .Where(p => p.Id == request.PlayerId)
                .FirstOrDefault();

            if (inDbPlayer == null)
            {
                return NotFound($"Player with id={request.PlayerId} doesn't exist!");
            }

            var inDbAssets = new List<DbAsset>();
            var assets = new List<IAsset>();

            // get all assets from the player, if asset ids have been provided
            if (request.AssetIds != null && request.AssetIds.Length > 0)
            {
                foreach (var inDbAssetId in request.AssetIds)
                {
                    var inDbAsset = _context.Assets.FirstOrDefault(p => p.Id == inDbAssetId);
                    if (inDbAsset == null)
                    {
                        return NotFound($"Asset with id={inDbAssetId} doesn't exist!");
                    }

                    inDbAssets.Add(inDbAsset);

                    assets.Add(Asset.MapToDomain(inDbAsset));
                }
            }

            var player = Player.MapToDomain(inDbPlayer);

            // TODO: make sure no trade assets
            // TODO: make sure no locked assets

            var flag = _engine.Transition(player, request.Identifier, [.. assets], out IAsset[] result);

            if (!flag)
            {
                return Conflict("Transition failed!");
            }

            // make sure that balance is taken over
            inDbPlayer.BalanceValue = player.Balance.Value;

            var assetIds = assets.Select(a => a.Id).ToHashSet();
            var resultIds = result.Select(r => r.Id).ToHashSet();

            var toBeRemoved = assets.Where(a => !resultIds.Contains(a.Id)).ToList();
            var toBeCreated = result.Where(r => !assetIds.Contains(r.Id)).ToList();
            var toBeUpdated = result.Where(r => assetIds.Contains(r.Id)).ToList();

            // Remove the assets no longer needed
            var removedAssetIds = toBeRemoved.Select(a => a.Id).ToHashSet();
            var removedDbAssets = _context.Assets.Where(p => removedAssetIds.Contains(p.Id)).ToList();
            if (removedDbAssets.Count > 0)
            {
                _context.Assets.RemoveRange(removedDbAssets);
            }

            // Create new assets
            var newDbAssets = toBeCreated.Select(a => SAGE.Model.DbAsset.MapToDb(a)).ToList();
            foreach (var newDbAsset in newDbAssets)
            {
                inDbPlayer.Assets.Add(newDbAsset);
            }

            // Update all assets that have been tagged to be updated
            foreach (var updatedAsset in toBeUpdated)
            {
                var inDbAsset = _context.Assets.FirstOrDefault(p => p.Id == updatedAsset.Id);
                if (inDbAsset == null)
                {
                    return NotFound($"Asset with id={updatedAsset.Id} doesn't exist!");
                }

                // Update properties
                inDbAsset.Score = updatedAsset.Score;
                inDbAsset.CollectionId = updatedAsset.CollectionId;
                inDbAsset.Genesis = updatedAsset.Genesis;
                inDbAsset.Data = updatedAsset.Data;
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
                return NotFound($"Asset with id={assetId} doesn't exist!");
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