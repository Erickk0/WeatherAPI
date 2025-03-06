using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.CQRS.Commands;

public class CreateWeatherCommand : IRequest<IActionResult>
{
    public float Temperature { get; set; }

    public int Humidity { get; set; }

    public float WindSpeed { get; set; }
}