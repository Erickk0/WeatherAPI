using MediatR;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Services.Repositories;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;

namespace WeatherAPI.CQRS.Query
{
    public class GetAllWeatherHandler : IRequestHandler<GetAllWeatherQuery, IEnumerable<WeatherItemDTO>>
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<GetAllWeatherHandler> _logger;

        public GetAllWeatherHandler(IWeatherService weatherService, ILogger<GetAllWeatherHandler> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherItemDTO>> Handle(GetAllWeatherQuery request, CancellationToken cancellationToken)
        {
            var weatherItems = (IEnumerable<WeatherItemDTO>) await _weatherService.GetAllWeather();

            var result = weatherItems.OrderBy(w => w.Temperature).ToList();

            return result;
        }
    }
}