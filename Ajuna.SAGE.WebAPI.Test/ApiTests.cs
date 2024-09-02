using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Model;
using Ajuna.SAGE.WebAPI.Data;
using Ajuna.SAGE.WebAPI.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;

namespace Ajuna.SAGE.WebAPI.Test
{
    public class ApiTests
    {
        private JsonSerializerOptions _serializeOptions;

        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var webAppFactory = new WebApplicationFactory<Program>();
            _httpClient = webAppFactory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient?.Dispose();
        }

        [Test]
        public async Task SeedRoute_SeedTest()
        {
            var response = await _httpClient.GetAsync("/api/AvatarGame/Seed?seed=1234");
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Empty);
            Assert.That(stringResult, Is.EqualTo("{\"value\":\"Successfully set seed 1234!\",\"formatters\":[],\"contentTypes\":[],\"declaredType\":null,\"statusCode\":200}"));
        }

        [Test]
        public async Task GenesisRoute_GenesisTest()
        {
            var response = await _httpClient.GetAsync("/api/AvatarGame/Genesis");
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Empty);
            //Assert.That(stringResult, Is.EqualTo("{\"value\":\"2023-06-12T13:17:43.6805829+02:00\",\"formatters\":[],\"contentTypes\":[],\"declaredType\":null,\"statusCode\":200}"));
        }

        [Test]
        public async Task BlockNumberRoute_BlockNumberTest()
        {
            var response = await _httpClient.GetAsync("/api/AvatarGame/BlockNumber");
            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Empty);
            Assert.That(stringResult, Is.EqualTo("{\"value\":0,\"formatters\":[],\"contentTypes\":[],\"declaredType\":null,\"statusCode\":200}"));
        }

        [Test]
        public async Task PlayerRoute_PlayerIdTest()
        {
            var response = await _httpClient.GetAsync("/api/AvatarGame/Player?playerId=3");

            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Null);
            Assert.That(stringResult, Is.Not.Empty);

            var jsonResult = JsonSerializer.Deserialize<JsonResultPlayer>(stringResult, _serializeOptions);
            Assert.That(jsonResult, Is.Not.Null);
            Assert.That(jsonResult.StatusCode, Is.EqualTo(200));
            Assert.That(jsonResult.Value?.Id, Is.EqualTo(3));
            Assert.That(jsonResult.Value?.Name, Is.EqualTo("Luu"));
            Assert.That(jsonResult.Value?.Cards, Is.Empty);
        }

        [Test]
        public async Task MintAndForgeRoute_PlayerIdTest()
        {
            var response = await _httpClient.GetAsync("/api/AvatarGame/Mint?playerId=4&packType=PrimeMoon&seed=0x12345678901234561234567890123456");

            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Null);
            Assert.That(stringResult, Is.Not.Empty);

            var jsonResult = JsonSerializer.Deserialize<JsonResultCards>(stringResult, _serializeOptions);
            Assert.That(jsonResult, Is.Not.Null);
            Assert.That(jsonResult.StatusCode, Is.EqualTo(200));
            Assert.That(jsonResult.Value, Is.Not.Null);
            //Assert.That(jsonResult.Value.Count, Is.GreaterThanOrEqualTo(Constants.PrimeMoonPackMinSize));
            //Assert.That(jsonResult.Value.Count, Is.LessThanOrEqualTo(Constants.PrimeMoonPackMaxSize));
            Assert.That(jsonResult.Value.All(p => p.Dna.StartsWith("11")), Is.True);

            var forgeDnas = "";
            foreach(var card in jsonResult.Value)
            {
                forgeDnas += "&avatarIds=" + card.Id;
            }

            response = await _httpClient.GetAsync("/api/AvatarGame/Forge?playerId=4" + forgeDnas);

            stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Null);
            Assert.That(stringResult, Is.Not.Empty);

            jsonResult = JsonSerializer.Deserialize<JsonResultCards>(stringResult, _serializeOptions);
            Assert.That(jsonResult, Is.Not.Null);
            Assert.That(jsonResult.StatusCode, Is.EqualTo(200));
            Assert.That(jsonResult.Value, Is.Not.Null);
            Assert.That(jsonResult.Value.Count, Is.EqualTo(3));
            Assert.That(jsonResult.Value.First().Dna.StartsWith("12"), Is.True);

        }

        [Test]
        public async Task MatchRoute_DnasTest()
        {
            var dna1 = "0x" + "00 11 02 13 04 15 04 13 02 11 00".Replace(" ", "");
            // Evaluation      Ma Mi -- Mi -- -- -- -- Ma Mi --
            var dna2 = "0x" + "01 01 12 13 04 04 13 12 01 01 15".Replace(" ", "");

            var response = await _httpClient.GetAsync($"/api/AvatarGame/Match?rarityLevel=0&dnas={dna1}&dnas={dna2}");

            var stringResult = await response.Content.ReadAsStringAsync();
            Assert.That(stringResult, Is.Not.Null);
            Assert.That(stringResult, Is.Not.Empty);

            var jsonResult = JsonSerializer.Deserialize<JsonResultMatch>(stringResult, _serializeOptions);
            Assert.That(jsonResult, Is.Not.Null);
            Assert.That(jsonResult.Value, Is.Not.Null);
            Assert.That(jsonResult.Value.Count, Is.EqualTo(1));
            Assert.That(jsonResult.Value[0].Leader, Is.EqualTo("0011021304150413021100"));
            Assert.That(jsonResult.Value[0].Sacrifice, Is.EqualTo("0101121304041312010115"));
            Assert.That(jsonResult.Value[0].MatchResult.RarityLevel, Is.EqualTo(0));
            Assert.That(jsonResult.Value[0].MatchResult.IsMatching, Is.EqualTo(true));
            Assert.That(jsonResult.Value[0].MatchResult.MirrorIndex.Count, Is.EqualTo(3));
            Assert.That(jsonResult.Value[0].MatchResult.MatchIndex.Count, Is.EqualTo(2));
            Assert.Pass();
        }

        public class JsonResultBase
        {
            public List<object>? Formatters { get; set; }
            public List<object>? ContentTypes { get; set; }
            public object? DeclaredType { get; set; }
            public int StatusCode { get; set; }
        }

        public class JsonResultPlayer : JsonResultBase
        {
            public DbPlayer? Value { get; set; }
        }

        public class JsonResultCards : JsonResultBase
        {
            public DbAsset[]? Value { get; set; }
        }

        public class JsonResultMatch : JsonResultBase
        {
            public List<MatchResultJson>? Value { get; set; }
        }
    }
}