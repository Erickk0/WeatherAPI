/*using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Neo4j.Driver;
using WeatherAPI.CQRS.Commands;
using Microsoft.Extensions.Logging;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Commands
{
    public class CreateWeatherHandler : IRequestHandler<CreateWeatherCommand, string>
    {
        private readonly IDriver _driver;
        private readonly ILogger<CreateWeatherHandler> _logger;

        public CreateWeatherHandler(IDriver driver, ILogger<CreateWeatherHandler> logger)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> Handle(CreateWeatherCommand request, CancellationToken cancellationToken)
        {
            var weather = new WeatherItemDTO();
            weather.Id = new Guid().ToString();
            var query = "CREATE (w:Weather {id: randomUUID(), temperature: $temperature, humidity: $humidity, windSpeed: $windSpeed}) RETURN w.id";
            var parameters = new Dictionary<string, object>
            {
                { "temperature", request.Temperature },
                { "humidity", request.Humidity },
                { "windSpeed", request.WindSpeed }
            };

            try
            {
                await using var session = _driver.AsyncSession();
                var result = await session.ExecuteWriteAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, parameters);
                    var record = await cursor.SingleAsync();

                    return record["w.id"].As<string>();
                });

                _logger.LogInformation("Weather record created successfully with ID: {WeatherId}", result);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while interacting with Neo4j database during weather record creation.");
                throw new InvalidOperationException("Error occurred while interacting with the database", ex);
            }
        }
    }
}*/


using MediatR;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.CQRS.Commands
{
    public class CreateWeatherHandler : IRequestHandler<CreateWeatherCommand, IActionResult>
    {
        private readonly IWeatherService _weatherService;

        public CreateWeatherHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Handle(CreateWeatherCommand request, CancellationToken cancellationToken)
        {
            return await _weatherService.CreateWeather(request);
        }
    }
}
