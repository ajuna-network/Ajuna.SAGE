using System.Text.Json.Serialization;
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


            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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