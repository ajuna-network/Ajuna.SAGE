using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Generic.Model;
using Ajuna.SAGE.Model;
using Ajuna.SAGE.WebAPI.Model;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Json;

namespace Ajuna.SAGE.WebAPI.Test
{
    [TestFixture]
    public class ApiIntegrationTests
    {
        private CustomWebApplicationFactory _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        [Order(1)]
        public async Task Get_BlockNumber_ShouldReturnOk()
        {
            // Arrange & Act
            var response = await _client.GetAsync("api/Api/BlockNumber");

            // Assert using Assert.That
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be OK");

            var result = await response.Content.ReadAsStringAsync();
            Assert.That(result, Is.Not.Null.And.Not.Empty, "Result should not be null or empty");
            Assert.That(int.TryParse(result, out _), Is.True, "Result should be an integer");
        }

        [Test]
        [Order(2)]
        public async Task CreatePlayer_ShouldReturnCreated()
        {
            // Arrange
            var playerRequest = new
            {
                Id = 2UL,
                Balance = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/Api/CreatePlayer", playerRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code should be Created");

            var content = await response.Content.ReadAsStringAsync();
            var createdPlayer = JObject.Parse(content);

            Assert.That(createdPlayer, Is.Not.Null);
            Assert.That((ulong)createdPlayer["id"], Is.EqualTo(2UL));
            Assert.That((int)createdPlayer["balanceValue"], Is.EqualTo(100));
        }

        [Test]
        [Order(3)]
        public async Task Get_NonExistentPlayer_ShouldContainNotFoundMessage()
        {
            // Arrange & Act
            var response = await _client.GetAsync("api/Api/Player/9999");
            var content = await response.Content.ReadAsStringAsync();

            // Since the current endpoint returns a JsonResult(NotFound("No player found!")) but not a 404 status code,
            // we check for the presence of the message:
            Assert.That(content, Contains.Substring("No player found!"), "Response should contain 'No player found!' message");
        }

        [Test]
        [Order(4)]
        public async Task Get_ExistentPlayer_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var response = await _client.GetAsync("api/Api/Player/2");
            var dbPlayer = await response.Content.ReadFromJsonAsync<DbPlayer>();

            Assert.That(dbPlayer, Is.Not.Null, "DbPlayer should not be null");
            Assert.That(dbPlayer.Assets, Is.Not.Null, "DbPlayer assets should not be null");
            Assert.That(dbPlayer.Assets.Count, Is.EqualTo(0), "Should have exactly 1 asset");
            Assert.That(dbPlayer.BalanceValue, Is.EqualTo(100), "Balance should be 100");
        }

        [Test]
        [Order(5)]
        public async Task Transition_CreateHero_ShouldReturnOk()
        {
            var transitionRequest = new TransitionRequest
            {
                PlayerId = 2UL,
                Identifier = HeroJamIdentifier.Create(AssetType.Hero, AssetSubType.None),
                AssetIds = Array.Empty<ulong>()
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/Api/Transition", transitionRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Status code should be OK");

            var content = await response.Content.ReadAsStringAsync();
            // The controller returns Ok(flag), where flag is boolean
            Assert.That(content, Is.EqualTo("true"), "Transition should return 'true' on success.");

            // Retrieve the player again and check assets
            var playerResponse = await _client.GetAsync("api/Api/Player/2");
            Assert.That(playerResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Fetching player after transition should be OK");

            var dbPlayer = await playerResponse.Content.ReadFromJsonAsync<DbPlayer>();

            // Expect exactly one asset
            Assert.That(dbPlayer, Is.Not.Null, "DbPlayer should not be null");
            Assert.That(dbPlayer.Assets, Is.Not.Null, "DbPlayer assets should not be null");
            Assert.That(dbPlayer.Assets.Count, Is.EqualTo(1), "Should have exactly 1 asset");

            Assert.That(dbPlayer.BalanceValue, Is.EqualTo(90), "Balance should be reduced by 10");

            // Check that the single asset has AssetType = Hero
            var dbAsset = dbPlayer.Assets.First();
            Assert.That(dbAsset.BalanceValue, Is.EqualTo(10), "Balance of db asset should be 10");

            var asset = Asset.MapToDomain(dbAsset);
            Assert.That(asset, Is.Not.Null, "Asset should not be null");

            var heroAsset = new HeroAsset(asset);
            Assert.That(heroAsset, Is.Not.Null, "HeroJamAsset should not be null");
            Assert.That(heroAsset.AssetType, Is.EqualTo(AssetType.Hero), "The newly created asset should be of type Hero");

            Assert.That(heroAsset.Balance.Value, Is.EqualTo(10), "Hero asset should have a balance of 10");
        }

        [Test]
        [Order(6)]
        public async Task Transition_CreateHero_ShouldFailWithAssets()
        {
            var transitionRequest = new TransitionRequest
            {
                PlayerId = 2UL,
                Identifier = HeroJamIdentifier.Create(AssetType.Hero, AssetSubType.None),
                AssetIds = new ulong[] { 1 }
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/Api/Transition", transitionRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict), "Status code should be Conflict");
            Assert.That(content, Contains.Substring("Transition failed!"), "Response should contain 'Transition failed!' message");
        }

        [Test]
        [Order(7)]
        public async Task Transition_CreateHero_ShouldFailWithHero()
        {
            var transitionRequest = new TransitionRequest
            {
                PlayerId = 2UL,
                Identifier = HeroJamIdentifier.Create(AssetType.Hero, AssetSubType.None),
                AssetIds = Array.Empty<ulong>()
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/Api/Transition", transitionRequest);
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict), "Status code should be Conflict");
            Assert.That(content, Contains.Substring("Transition failed!"), "Response should contain 'Transition failed!' message");
        }

    }

}