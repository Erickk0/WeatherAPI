using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;
using WeatherAPI.Models;

namespace WeatherAPI.Services.Repositories
{
    public interface IWeatherService
    {
        Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command);
        Task<IActionResult> GetWeatherById(string id);
        Task<IEnumerable<WeatherItemDTO>> GetAllWeather();
        Task<IActionResult> DeleteWeatherById(string id);
    }
}
