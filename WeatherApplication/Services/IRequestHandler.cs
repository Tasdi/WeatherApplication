using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    public interface IRequestHandler
    {
        WeatherInformation FetchDataFromInput(string cityName);
    }
}