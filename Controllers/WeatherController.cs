using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly Neo4jService _neo4jService;

        public WeatherController(Neo4jService neo4jService)
        {
            _neo4jService = neo4jService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateWeather([FromBody] WeatherItem item)
        {
            await _neo4jService.CreateWeatherItem(item);
            return CreatedAtAction(nameof(GetWeather), new { id = item.Id }, item);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWeather(string id)
        {
            var item = await _neo4jService.GetWeatherItem(id);
            if (item == null) return NotFound("Weather item not found");
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeather(string id, [FromBody] WeatherItem item)
        {
            item.Id = id;
            await _neo4jService.UpdateWeatherItem(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeather(string id)
        {
            await _neo4jService.DeleteWeatherItem(id);
            return NoContent();
        }
    }
}
