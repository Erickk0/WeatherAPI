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
            var query = "MATCH (w:Weather {id: $id}) RETURN w";
            var parameters = new Dictionary<string, object> { { "id", id } };

            try
            {
                await using var session = _driver.AsyncSession();
                var result = await session.ExecuteReadAsync(async tx =>
                {
                    var cursor = await tx.RunAsync(query, parameters);

                    if (!await cursor.FetchAsync())
                        return null; // Return null if no record is found

                    var record = cursor.Current;
                    var node = record["w"].As<INode>();

                    return new WeatherItemDTO
                    {
                        Id = id,
                        Temperature = node.Properties["temperature"].As<float>(),
                        Humidity = node.Properties["humidity"].As<int>(),
                        WindSpeed = node.Properties["windSpeed"].As<float>()
                    };
                });

                if (result == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(result); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving weather data for ID {WeatherId}.", id);
                return new BadRequestObjectResult($"Error retrieving weather data: {ex.Message}");
            }
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

            return weatherList;
        }

        public async Task<bool> DeleteWeatherById(string id)
        {
            var query = "MATCH (w:Weather {id: $id}) DELETE w";

            try
            {
                await using var session = _driver.AsyncSession();
                var result = await session.ExecuteWriteAsync(async tx =>
                {
                    var cursor = tx.RunAsync(query, new { id });
                    return cursor;
                });
                return result != null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occured while deleting the weather record", ex);
            }
        }
    }
}
