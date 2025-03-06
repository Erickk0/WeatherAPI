using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Controllers;
using WeatherAPI.CQRS.Query;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Neo4j.Driver;
using WeatherAPI.Models;

namespace WeatherAPI.Services.Repositories
{
    public class WeatherService : IWeatherService
    {
        private readonly IMediator _mediator;
        private readonly IDriver _driver;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IMediator mediator, IDriver driver, ILogger<WeatherService> logger)
        {
            _mediator = mediator;
            _driver = driver;
            _logger = logger;
        }

        public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command)
        {
            var query = "CREATE (w:Weather {id: randomUUID(), temperature: $temperature, humidity: $humidity, windSpeed: $windSpeed}) RETURN w.id";

            var parameters = new Dictionary<string, object>
            {
                { "temperature", command.Temperature },
                { "humidity", command.Humidity },
                { "windSpeed", command.WindSpeed }
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

                //var result = await _mediator.Send(command);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while interacting with the database.");
                return new BadRequestObjectResult($"Error creating weather data: {ex.Message}"); ;
            }
            
        }

        public async Task<IActionResult> GetWeatherById(string id)
        {
            var query = new GetWeatherByIdQuery(id);
            var result = await _mediator.Send(query);
            return result is not null ? new OkObjectResult(result) : new NotFoundResult();
        }

        public async Task<IEnumerable<WeatherItemDTO>> GetAllWeather()
        {
            var query = "MATCH (w:Weather) RETURN w.id AS id, w.temperature AS temperature, w.humidity AS humidity, w.windSpeed AS windSpeed";
            var weatherList = new List<WeatherItemDTO>();

            try
            {
                await using var session = _driver.AsyncSession();
                var cursor = await session.RunAsync(query);

                await cursor.ForEachAsync(record =>
                {
                    var weatherData = new WeatherItemDTO
                    {
                        Id = record["id"].As<string>(),
                        Temperature = record["temperature"].As<float>(),
                        Humidity = record["humidity"].As<int>(),
                        WindSpeed = record["windSpeed"].As<float>()
                    };
                    weatherList.Add(weatherData);
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while retrieving weather data", ex);
            }

            var result = new OkObjectResult(weatherList);
            //var result = await _mediator.Send(query);
            return weatherList;
        }

        public async Task<IActionResult> DeleteWeatherById(string id)
        {
            var query = new DeleteWeatherCommand(id);
            var result = await _mediator.Send(query);
            return new OkObjectResult(result);
        }
    }
}
