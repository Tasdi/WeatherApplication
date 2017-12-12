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
        private Mock<IWeatherInformation> weatherInformation;

        [SetUp]
        public void TestSetup()
        {
            requestHandler = new RequestHandler();
            weatherInformation = new Mock<IWeatherInformation>();
        }

        [Test]
        public void ShouldGetCorrectApiUrl()
        {
            string result = requestHandler.GetApiUrl("hawai");
            Assert.AreEqual(result,
                "api.openweathermap.org/data/2.5/weather?q=hawai&APPID=2c807f909d802a67435deb6a813afe1c");

        }

        [Test]
        public void ShouldGetCorrectJsonFromApi()
        {
            string apiUrl1 = requestHandler.GetApiUrl("london");
            string result1 = requestHandler.GetJsonFromApi(apiUrl1);
            weatherInformation = JsonConvert.DeserializeObject<Mock<IWeatherInformation>>(result1);
            Assert.AreEqual("London", weatherInformation.Name);

            string apiUrl2 = requestHandler.GetApiUrl("stockholm");
            string result2 = requestHandler.GetJsonFromApi(apiUrl2);
            weatherInformation = JsonConvert.DeserializeObject<Mock<IWeatherInformation>>(result2);
            Assert.AreEqual("Stockholm", weatherInformation.Name);
        }

        [Test]
        public void ShouldGetNullIfInputIsNotInDatabase()
        {
            string apiUrl1 = requestHandler.GetApiUrl("londo");
            string result1 = requestHandler.GetJsonFromApi(apiUrl1);
            weatherInformation = JsonConvert.DeserializeObject<Mock<IWeatherInformation>>(result1);
            Assert.AreEqual(null, weatherInformation);
        }

        [Test]
        public void testFetchFromInput()
        {
           WeatherInformation info = new WeatherInformation();
            info = requestHandler.FetchDataFromInput("dhaka");
            Assert.AreEqual("Dhaka",info.name);

        }

        [Test]
        public void testFetchFromInputWhenWrongInput()
        {
            WeatherInformation info = new WeatherInformation();
            info = requestHandler.FetchDataFromInput("daka");
            Assert.AreEqual(null, info);

        }

    }
}
