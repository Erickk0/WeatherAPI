using MediatR;
using Neo4j.Driver;
using WeatherAPI.Models;
using System;

namespace WeatherAPI.CQRS.Query;

public class GetWeatherByIdHandler : IRequestHandler<GetWeatherByIdQuery, WeatherItemDTO>
{
    private readonly IDriver _driver;

    public GetWeatherByIdHandler(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<WeatherItemDTO> Handle(GetWeatherByIdQuery request, CancellationToken cancellationToken)
    {
        var query = "MATCH (w:Weather {id: $id}) RETURN w";
        var parameters = new Dictionary<string, object> { { "id", request.Id } };

        await using var session = _driver.AsyncSession();
        return await session.ExecuteReadAsync(async tx =>
        {
            IResultCursor cursor = await tx.RunAsync(query, parameters);

            if (!await cursor.FetchAsync())
                return null;

            var record = cursor.Current;
            var node = record["w"].As<INode>();

            return new WeatherItemDTO
            {
                Id = request.Id,
                Temperature = node.Properties["temperature"].As<float>(),
                Humidity = node.Properties["humidity"].As<int>(),
                WindSpeed = node.Properties["windSpeed"].As<float>()

            };
        });
    }
}
