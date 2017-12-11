using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using WeatherApplication.Models;

namespace WeatherApplication.Services
{
    public class RequestHandler : IRequestHandler
    {
        public string APIReq; //= "api.openweathermap.org/data/2.5/weather?q=";
        public static string KEY; // KEY= "2c807f909d802a67435deb6a813afe1c";

        public RequestHandler()
        {
            // Initialize variables needed to query API
            APIReq = "api.openweathermap.org/data/2.5/weather?q=";
            KEY = "2c807f909d802a67435deb6a813afe1c";
        }

        public WeatherInformation FetchDataFromInputAsync(string cityName)
        {
            // Get correct url of API in string format
            string apiUrl = GetApiUrl(cityName);
            // Use the url string to get json data
            string jsonData = getJsonFromApi(apiUrl);
            // Deserialize json data to models (WeatherInformation)
            WeatherInformation weatherInformation = JsonConvert.DeserializeObject<WeatherInformation>(jsonData);

            // If returned json data is null, let the user know
            if (jsonData == "")
            {
                return null;
            }
            else
            {
                return weatherInformation;
            }
        }

        // Take url as input, return the json objects contained in url
        private string getJsonFromApi(string apiUrl)
        {
            string dataStream = null;

            // Get data from stream in string format
            dataStream = RequestWithUrl(apiUrl, dataStream);

            // If a null value is returned, something went wrong. Return empty string
            if(dataStream == null)
            {
                return "";
            }

            // If nothing went wrong, return the stream in string format
            return dataStream;
        }

        private string RequestWithUrl(string apiUrl, string dataStream)
        {
            try
            {
                // Specify protocol along with url in order to request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"http://{apiUrl}");
                // Store response obtained from request
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Store the response stream
                Stream receiveStream = response.GetResponseStream();
                // Stringbuilder to store what is being read from stream
                StringBuilder sb = new StringBuilder();
                // Store text in "line" that will be append to the string builder
                String line;

                // Read the entire stream
                using (StreamReader sr = new StreamReader(receiveStream))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }

                // Done reading, close the streams
                receiveStream.Close();
                response.Close();
                // Convert to string
                dataStream = sb.ToString();
            }

            // Catch every possible exception and print that for debugging purposes
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }

            // Return what has been read from the stream in string format
            return dataStream;
        }

        private string GetApiUrl(string cityName)
        {
            // Creating stringbuilder to manipulate string
            StringBuilder sb = new StringBuilder(APIReq);
            // Append city name and key to url
            sb.Append($"{cityName}&APPID={KEY}");
            // Return the url in string format
            return sb.ToString();
        }
    }
}