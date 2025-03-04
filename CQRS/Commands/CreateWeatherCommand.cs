using System;
using MediatR;

namespace WeatherAPI.CQRS.Commands;

public class CreateWeatherCommand : IRequest<string>
{
    public float Temperature { get; set; }

    public int Humidity { get; set; }

    public float WindSpeed { get; set; }
}