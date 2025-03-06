namespace WeatherAPI.CQRS.Commands;
using MediatR;

public class UpdateWeatherCommand : IRequest<bool>
{
    public string Id { get; set; }

    public float? Temperature { get; set; }

    public int? Humidity { get; set; }

    public float? WindSpeed { get; set; }
}
