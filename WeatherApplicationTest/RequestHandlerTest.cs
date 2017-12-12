using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WeatherApplication.Services;
using WeatherApplication.Models;

namespace WeatherApplicationTest
{
    [TestFixture]
    public class RequestHandlerTest
    {
        private RequestHandler requestHandler;
        private WeatherInformation weatherInformation;

        // Initializes set-up for the tests
        [SetUp]
        public void TestSetup()
        {
            requestHandler = new RequestHandler();
            weatherInformation = new WeatherInformation();
        }

        // Tests that API url is formatted correctly
        [Test]
        public void ShouldGetCorrectApiUrl()
        {
            string result = requestHandler.GetApiUrl("hawaii");
            Assert.AreEqual(result,
                "api.openweathermap.org/data/2.5/weather?q=hawaii&APPID=2c807f909d802a67435deb6a813afe1c");

        }

        // If user input (city name) exist in API, correct data should be returned
        [Test]
        public void ShouldGetCorrectJsonFromApi()
        {
            string apiUrl1 = requestHandler.GetApiUrl("london");
            string result1 = requestHandler.GetJsonFromApi(apiUrl1);
            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(result1);
            Assert.AreEqual("London", weatherInformation.name);

            string apiUrl2 = requestHandler.GetApiUrl("stockholm");
            string result2 = requestHandler.GetJsonFromApi(apiUrl2);
            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(result2);
            Assert.AreEqual("Stockholm", weatherInformation.name);
        }

        // Tests that weather information is actually null if incorrect value is passed in URL
        [Test]
        public void ShouldGetNullIfInputIsNotInDatabase()
        {
            string apiUrl1 = requestHandler.GetApiUrl("londo");
            string result1 = requestHandler.GetJsonFromApi(apiUrl1);
            weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(result1);
            Assert.AreEqual(null, weatherInformation);
        }

        // Tests that all methods in RequestHandler works properly when input is correct
        [Test]
        public void TestFetchFromInput()
        {
            WeatherInformation info = new WeatherInformation();
            info = requestHandler.FetchDataFromInput("dhaka");
            Assert.AreEqual("Dhaka", info.name);

        }

        // If incorrect input, weather information is null
        [Test]
        public void TestFetchFromInputWhenWrongInput()
        {
            WeatherInformation info = new WeatherInformation();
            info = requestHandler.FetchDataFromInput("daka");
            Assert.AreEqual(null, info);
        }
    }
}
