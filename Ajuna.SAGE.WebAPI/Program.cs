using System.Security.Cryptography;
using System.Text.Json.Serialization;
using Ajuna.SAGE.Game.HeroJam;
using Ajuna.SAGE.Generic;
using Ajuna.SAGE.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Yaml;
using Serilog;

namespace Ajuna.SAGE.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // configure serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo
                .Console()
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            // Set up configuration with YAML file
            builder.Configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddYamlFile("config.yml", optional: false, reloadOnChange: true);

            builder.Services.AddDbContext<ApiContext>
                (opt => opt.UseInMemoryDatabase("SageDB"));

            // Register the engine
            builder.Services.AddSingleton(sp =>
            {
                var randomSeed = RandomNumberGenerator.GetInt32(0, int.MaxValue);
                var blockchainProvider = new BlockchainInfoProvider(randomSeed);
                return HeroJameGame.Create(blockchainProvider);
            });

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Seed initial data
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

                // Add a default config if none exists
                if (!context.Configs.Any())
                {
                    context.Configs.Add(new DbConfig { Genesis = DateTime.Now });
                }

                // Add a default player if none exists
                if (!context.Players.Any())
                {
                    context.Players.Add(new DbPlayer
                    {
                        Id = 1,
                        BalanceValue = 1000,
                        Assets = new List<DbAsset>()
                    });
                }

                context.SaveChanges();
            }

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}