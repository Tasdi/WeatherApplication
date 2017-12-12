using System.Collections.Generic;


namespace WeatherApplication.Models
{
    // The following classes is used to store appropriate json objects to corresponding classes.
    // It is strcutured according to json format the response is received in.

    // Will store the coordinates from response
    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    // This class holds information such as weather description
    public class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    // Holds information about temperature, pressure, etc.
    public class Main
    {
        public double temp { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
    }

    // Will store information about wind speed and degree
    public class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public class Clouds
    {
        public int all { get; set; }
    }

    // This class holds information such as country code,
    // time for sunrise, sunset, etc.
    public class Sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public double message { get; set; }
        public string country { get; set; }
        public double sunrise { get; set; }
        public double sunset { get; set; }
    }

    // This class will be invoked to render weather information
    // in the GUI
    public class WeatherInformation : IWeatherInformation
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}