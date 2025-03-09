using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Models;
using WeatherAPI.CQRS.Query;
using WeatherAPI.Services.Repositories;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IWeatherService _weatherService;

        public WeatherController(IMediator mediator, IWeatherService weatherService)
        {
            _mediator = mediator;
            _weatherService = weatherService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command)
        {
            var result = await _weatherService.CreateWeather(command);
            return CreatedAtAction(nameof(GetWeatherById), new { id = result }, result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherById(string id)
        {
            return await _weatherService.GetWeatherById(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWeather()
        {
            var query = new GetAllWeatherQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteWeatherById(string id)
        {
            var result = await _weatherService.DeleteWeatherById(id);
            if (result)
            {
                return Ok("Weather record deleted");
            }
            else
            {
                return NotFound();
            }
        }
    }
}
