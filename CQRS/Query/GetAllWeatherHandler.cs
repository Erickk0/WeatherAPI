using MediatR;
using Neo4j.Driver;
using WeatherAPI.CQRS.Query;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Query;

public class GetAllWeatherQueryHandler : IRequestHandler<GetAllWeatherQuery, IEnumerable<WeatherItemDTO>>
{
    private readonly IDriver _driver;

    public GetAllWeatherQueryHandler(IDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
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
}

