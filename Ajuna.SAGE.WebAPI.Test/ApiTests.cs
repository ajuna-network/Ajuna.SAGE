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
        public async Task CreatePlayer_ShouldReturnCreated()
        {
            // Arrange
            var playerRequest = new
            {
                Id = 1UL,
                Balance = 100
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/Api/CreatePlayer", playerRequest);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Status code should be Created");

            var createdPlayer = await response.Content.ReadFromJsonAsync<YourPlayerDtoOrDbPlayerClass>();
            Assert.That(createdPlayer, Is.Not.Null, "Created player should not be null");
            Assert.That(createdPlayer.Id, Is.EqualTo(1UL), "Player Id should match the requested Id");
            Assert.That(createdPlayer.BalanceValue, Is.EqualTo(100), "Player balance should match the requested balance");
        }

        [Test]
        public async Task Get_NonExistentPlayer_ShouldContainNotFoundMessage()
        {
            // Arrange & Act
            var response = await _client.GetAsync("api/Api/Player?playerId=9999");
            var content = await response.Content.ReadAsStringAsync();

            // Since the current endpoint returns a JsonResult(NotFound("No player found!")) but not a 404 status code,
            // we check for the presence of the message:
            Assert.That(content, Contains.Substring("No player found!"), "Response should contain 'No player found!' message");
        }
    }

    // Replace with your actual player model
    public class YourPlayerDtoOrDbPlayerClass
    {
        public ulong Id { get; set; }
        public int BalanceValue { get; set; }
        // Add other properties as needed
    }
}