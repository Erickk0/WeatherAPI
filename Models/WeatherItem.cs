namespace WeatherAPI.Models
{
    public class WeatherItemDTO
    {
        public string Id { get; set; }

        public float Temperature { get; set; }

        public int Humidity { get; set; }

        public float WindSpeed { get; set; }
    }
}
