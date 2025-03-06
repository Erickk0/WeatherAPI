using System;
using MediatR;
using Neo4j.Driver;
using WeatherAPI.CQRS.Commands;

namespace WeatherAPI.CQRS.Commands;

public class UpdateWeatherHandler : IRequestHandler<UpdateWeatherCommand, bool>
{
    private readonly IDriver _driver;

    public UpdateWeatherHandler(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<bool> Handle(UpdateWeatherCommand request, CancellationToken cancellationToken)
    {
        await using var session = _driver.AsyncSession();

        return await session.ExecuteWriteAsync(async tx =>
        {
            var fetchQuery = "MATCH (w:Weather {id: $id}) RETURN w";
            var fetchCursor = await tx.RunAsync(fetchQuery, new { id = request.Id });
            
            if (!await fetchCursor.FetchAsync())
                return false;

            var record = fetchCursor.Current;
            var node = record["w"].As<INode>();

            return true;
        });
    }
}

