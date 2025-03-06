using Microsoft.AspNetCore.Mvc;
using WeatherAPI.CQRS.Commands;

namespace WeatherAPI.Services.Repositories
{
    public interface IWeatherService
    {
        Task<IActionResult> CreateWeather([FromBody] CreateWeatherCommand command);
        Task<IActionResult> GetWeatherById(string id);
        Task<IActionResult> GetAllWeather();
        Task<IActionResult> DeleteWeatherById(string id);
    }
}
