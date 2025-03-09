using MediatR;
using Neo4j.Driver;
using WeatherAPI.Models;
using System;
using System.Runtime.CompilerServices;
using WeatherAPI.Services.Repositories;

namespace WeatherAPI.CQRS.Query;

public class GetWeatherByIdHandler : IRequestHandler<GetWeatherByIdQuery, WeatherItemDTO>
{
    private readonly IWeatherService _weatherService;

    public GetWeatherByIdHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<WeatherItemDTO> Handle(GetWeatherByIdQuery request, CancellationToken cancellationToken)
    {
        return (WeatherItemDTO) await _weatherService.GetWeatherById(request.Id);

    }
}
