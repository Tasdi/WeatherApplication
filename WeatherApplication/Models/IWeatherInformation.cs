using System.Collections.Generic;

namespace WeatherApplication.Models
{
    public interface IWeatherInformation
    {
        Coord coord { get; set; }
        List<Weather> weather { get; set; }
        string @base { get; set; }
        Main main { get; set; }
        int visibility { get; set; }
        Wind wind { get; set; }
        Clouds clouds { get; set; }
        int dt { get; set; }
        Sys sys { get; set; }
        int id { get; set; }
        string name { get; set; }
        int cod { get; set; }
    }
}