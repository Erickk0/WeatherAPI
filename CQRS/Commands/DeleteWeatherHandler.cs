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
using WeatherAPI.Services.Repositories;

namespace WeatherAPI.CQRS.Commands;

public class DeleteWeatherHandler : IRequestHandler<DeleteWeatherCommand, bool>
{
    private readonly IWeatherService _weatherService;

    public DeleteWeatherHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<bool> Handle(DeleteWeatherCommand request, CancellationToken cancellationToken)
    {
        return await _weatherService.DeleteWeatherById(request.Id);
    }
}