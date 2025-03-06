/*using MediatR;
using Neo4j.Driver;
using WeatherAPI.CQRS.Query;
using WeatherAPI.Models;
using WeatherAPI.Services.Repositories;

namespace WeatherAPI.CQRS.Query;

public class GetAllWeatherQueryHandler : IRequestHandler<GetAllWeatherQuery, IEnumerable<WeatherItemDTO>>
{
    private readonly IDriver _driver;
    private readonly IWeatherService _weatherService;

    public GetAllWeatherQueryHandler(IDriver driver, IWeatherService weatherService)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        _weatherService = weatherService;
    }

    public async Task<IEnumerable<WeatherItemDTO>> Handle(GetAllWeatherQuery request, CancellationToken cancellationToken)
    {
        var query = "MATCH (w:Weather) RETURN w.id AS id, w.temperature AS temperature, w.humidity AS humidity, w.windSpeed AS windSpeed";

        var weatherList = new List<WeatherItemDTO>();

        try
        {
            await using var session = _driver.AsyncSession();
            var result = await session.RunAsync(query);

            await result.ForEachAsync(record =>
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
}*/


using MediatR;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Query
{
    public class GetAllWeatherHandler : IRequestHandler<GetAllWeatherQuery, IEnumerable<WeatherItemDTO>>
    {
        private readonly IWeatherService _weatherService;

        public GetAllWeatherHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IEnumerable<WeatherItemDTO>> Handle(GetAllWeatherQuery request, CancellationToken cancellationToken)
        {
            return (IEnumerable<WeatherItemDTO>) await _weatherService.GetAllWeather();
        }
    }
}