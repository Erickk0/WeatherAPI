using System;
using MediatR;

namespace WeatherAPI.CQRS.Commands;

public class DeleteWeatherCommand : IRequest<bool>
{
    public string Id { get; set; }

    public DeleteWeatherCommand(string id)
    {
        Id = id;
    }
}