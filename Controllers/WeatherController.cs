using MediatR;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Models;
using WeatherAPI.CQRS.Query;

namespace Weather_API.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WeatherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetWeatherById), new { id = result }, result);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating weather data: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeatherById(string id)
        {
            var query = new GetWeatherByIdQuery(id);
            var result = await _mediator.Send(query);
            return result is not null ? Ok(result) : NotFound();
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
            var query = new DeleteWeatherCommand(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
