using SecretsManager.Api.Models;
using SecretsManager.Api.Services;

var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;
builder.Configuration.AddSecretsManager(
    configurator: options =>
    {
        options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
        options.KeyGenerator = (_, s) => s
            .Replace($"{env}_{appName}_", string.Empty)
            .Replace("__", ":");

        // automatically pull new secret after it was rotated
        options.PollingInterval = TimeSpan.FromSeconds(10);
    }
);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<OpenWeatherApiSettings>(builder.Configuration.GetSection(OpenWeatherApiSettings.Key));
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWeatherService, WeatherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
        "/weather/{city}",
        async (string city, IWeatherService weatherService) =>
        {
            var weather = await weatherService.GetCurrentWeatherAsync(city);
            if (weather is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(weather);
        }
    )
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();
