using MediatR;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Query;
public class GetWeatherByIdQuery : IRequest<WeatherItemDTO>
{
    public string Id { get; set; }
    public GetWeatherByIdQuery(string id) => Id = id;
}
