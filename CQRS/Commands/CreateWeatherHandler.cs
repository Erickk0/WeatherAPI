using MediatR;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Services.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WeatherAPI.CQRS.Commands
{
    public class CreateWeatherHandler : IRequestHandler<CreateWeatherCommand, IActionResult>
    {
        private readonly IWeatherService _weatherService;

        public CreateWeatherHandler(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Handle(CreateWeatherCommand request, CancellationToken cancellationToken)
        {
            return await _weatherService.CreateWeather(request);
        }
    }
}
