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
            string result1 = requestHandler.getJsonFromApi(apiUrl1);
            weatherInformation = JsonConvert.DeserializeObject<Mock<IWeatherInformation>>(result1);
            Assert.AreEqual("London", weatherInformation.Name);

            string apiUrl2 = requestHandler.GetApiUrl("stockholm");
            string result2 = requestHandler.getJsonFromApi(apiUrl2);
            weatherInformation = JsonConvert.DeserializeObject<Mock<IWeatherInformation>>(result2);
            Assert.AreEqual("Stockholm", weatherInformation.Name);
        }

    }
}
