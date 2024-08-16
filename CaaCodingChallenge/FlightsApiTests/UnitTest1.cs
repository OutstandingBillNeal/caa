using FlightsApi;
using FlightsApi.Controllers;
using Microsoft.Extensions.Logging;
using Moq;

namespace FlightsApiTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();
            var sut = new WeatherForecastController(mockLogger.Object);
            var result = sut.Get();
            Assert.NotNull(result);
            Assert.IsType<WeatherForecast[]>(result);
        }
    }
}