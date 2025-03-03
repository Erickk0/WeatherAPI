namespace WeatherAPI.Models
{
    public class WeatherItem
    {
        public required string Id { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float WindSpeed { get; set; }
    }
}
