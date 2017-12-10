using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    public class RequestHandler : IRequestHandler
    {
        public string APIReq = "api.openweathermap.org/data/2.5/weather?q=";
        public static string KEY= "2c807f909d802a67435deb6a813afe1c";

        public RequestHandler()
        {
        }

        public string FetchDataFromInputAsync(string cityName, bool checkDegree)
        {
            string apiUrl = GetApiUrl(cityName);
            string jsonData = getJsonFromApi(apiUrl);

            if (jsonData == "")
            {
                return "The city does not exist in the database";
            }
                

            var res = JsonConvert.DeserializeObject<WeatherInformation>(jsonData);
            if (checkDegree)
            {
                double tempF = (9 / 5 * (res.main.temp - 273)) + 32;
                return $"country name is {res.sys.country} and the degree is {tempF}";
            }
            else
            {
                double tempC = res.main.temp - 273.15;
                return $"city name is {cityName} and the degree is {tempC}";
            }
        }

        private string getJsonFromApi(string apiUrl)
        {
            string dataStream = null;

            dataStream = RequestWithUrl(apiUrl, dataStream);
            if(dataStream == null)
            {
                return "";
            }
            return dataStream;
        }

        private string RequestWithUrl(string apiUrl, string dataStream)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{apiUrl}");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StringBuilder sb = new StringBuilder();
                String line;

                using (StreamReader sr = new StreamReader(receiveStream))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }
                receiveStream.Close();
                response.Close();
                dataStream = sb.ToString();
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            return dataStream;
        }

        private string GetApiUrl(string cityName)
        {
            StringBuilder sb = new StringBuilder(APIReq);
            sb.Append($"{cityName}&APPID={KEY}");
            return sb.ToString();
        }
    }
}