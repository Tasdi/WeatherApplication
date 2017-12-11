using System;
using NUnit;
using Moq;
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
    }
}
