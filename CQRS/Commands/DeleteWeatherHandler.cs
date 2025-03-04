using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Neo4j.Driver;
using WeatherAPI.CQRS.Commands;
using Microsoft.Extensions.Logging;
using WeatherAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace WeatherAPI.CQRS.Commands;

public class DeleteWeatherHandler : IRequestHandler<DeleteWeatherCommand, bool>
{
    private readonly IDriver _driver;

    public DeleteWeatherHandler(IDriver driver)
    {
        _driver = driver ?? throw new ArgumentNullException(nameof(driver));
    }

    public async Task<bool> Handle(DeleteWeatherCommand request, CancellationToken cancellationToken)
    {
        var query = "MATCH (w:Weather {id: $id}) DELETE w";

        try
        {
            await using var session = _driver.AsyncSession();
            var result = await session.ExecuteWriteAsync(tx =>
            {
                var cursor = tx.RunAsync(query, new { id = request.Id });
                return cursor;
            });
            return true;
        }
        catch(Exception ex)
        {
            throw new InvalidOperationException("Error occured while deleting the weather record", ex);
        }
        
    }

}