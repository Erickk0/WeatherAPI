using MediatR;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Query
{
    public class GetAllWeatherQuery : IRequest<IEnumerable<WeatherItemDTO>>{ }
}
