using Neo4j.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public class Neo4jService
    {
        private readonly IDriver _driver;

        public Neo4jService(string uri, string user, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
        }

        public async Task CreateWeatherItem(WeatherItem item)
        {
            var query = "CREATE (w:Weather {id: $id, temperature: $temperature, humidity: $humidity, windSpeed: $windSpeed})";
            var parameters = new Dictionary<string, object>
            {
                { "id", item.Id },
                { "temperature", item.Temperature },
                { "humidity", item.Humidity },
                { "windSpeed", item.WindSpeed }
            };

            await ExecuteWriteTransaction(query, parameters);
        }

        public async Task<WeatherItem> GetWeatherItem(string id)
        {
            var query = "MATCH (w:Weather {id: $id}) RETURN w";
            var parameters = new Dictionary<string, object> { { "id", id } };

            return await ExecuteReadTransaction(query, parameters);
        }

        public async Task UpdateWeatherItem(WeatherItem item)
        {
            var query = "MATCH (w:Weather {id: $id}) SET w.temperature = $temperature, w.humidity = $humidity, w.windSpeed = $windSpeed";
            var parameters = new Dictionary<string, object>
            {
                { "id", item.Id },
                { "temperature", item.Temperature },
                { "humidity", item.Humidity },
                { "windSpeed", item.WindSpeed }
            };

            await ExecuteWriteTransaction(query, parameters);
        }

        public async Task DeleteWeatherItem(string id)
        {
            var query = "MATCH (w:Weather {id: $id}) DELETE w";
            var parameters = new Dictionary<string, object> { { "id", id } };

            await ExecuteWriteTransaction(query, parameters);
        }

        private async Task ExecuteWriteTransaction(string query, Dictionary<string, object> parameters)
        {
            await using var session = _driver.AsyncSession();
            await session.WriteTransactionAsync(async tx => await tx.RunAsync(query, parameters));
        }

        private async Task<WeatherItem> ExecuteReadTransaction(string query, Dictionary<string, object> parameters)
        {
            await using var session = _driver.AsyncSession();
            return await session.ReadTransactionAsync(async tx =>
            {
                var cursor = await tx.RunAsync(query, parameters);
                var record = await cursor.SingleAsync();
                var node = record["w"].As<INode>();

                return new WeatherItem
                {
                    Id = node.Properties["id"].As<string>(),
                    Temperature = node.Properties["temperature"].As<float>(),
                    Humidity = node.Properties["humidity"].As<float>(),
                    WindSpeed = node.Properties["windSpeed"].As<float>()
                };
            });
        }
    }
}
