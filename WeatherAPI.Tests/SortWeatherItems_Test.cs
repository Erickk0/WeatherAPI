using WeatherAPI.CQRS.Query;
using WeatherAPI.Services.Repositories;
using WeatherAPI.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;


public class GetAllWeatherHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnWeatherDataSortedByTemperature()
    {
        //Arrange
        var mockWeatherService = Substitute.For<IWeatherService>();
        var mockLogger = Substitute.For<ILogger<GetAllWeatherHandler>>();

        var unsortedWeatherData = new List<WeatherItemDTO>
        {
            new WeatherItemDTO { Id = "1", Temperature = 25.0f, Humidity = 60, WindSpeed = 10.0f },
            new WeatherItemDTO { Id = "2", Temperature = 20.0f, Humidity = 70, WindSpeed = 15.0f },
            new WeatherItemDTO { Id = "3", Temperature = 30.0f, Humidity = 50, WindSpeed = 5.0f }
        };

        mockWeatherService.GetAllWeather().Returns(unsortedWeatherData);

        var handler = new GetAllWeatherHandler(mockWeatherService, mockLogger);

        var query = new GetAllWeatherQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);
        var sortedData = result.ToList();

        // Assert
        Assert.NotNull(sortedData);
        sortedData[0].Temperature.Should().Be(20.0f);
        sortedData[1].Temperature.Should().Be(25.0f);
        sortedData[2].Temperature.Should().Be(30.0f);
    }
}