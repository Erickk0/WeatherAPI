using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Models;
using WeatherAPI.CQRS.Query;
using WeatherAPI.Services.Repositories;

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
            /*try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetWeatherById), new { id = result }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating weather data: {ex.Message}");
            }*/
            var result = await _weatherService.CreateWeather(command);
            return CreatedAtAction(nameof(GetWeatherById), new { id = result }, result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherById(string id)
        {
            /*var query = new GetWeatherByIdQuery(id);
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();*/
            return await _weatherService.GetWeatherById(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWeather()
        {
            /*var query = new GetAllWeatherQuery();
            var result = await _mediator.Send(query);
            return Ok(result);*/
            var result = await _weatherService.GetAllWeather();
            return Ok(result);
        }
        
        [HttpDelete]
        public async Task<IActionResult> DeleteWeatherById(string id)
        {
            /*var query = new DeleteWeatherCommand(id);
            var result = await _mediator.Send(query);
            return Ok(result);*/
            return await _weatherService.DeleteWeatherById(id);
        }
    }
}
