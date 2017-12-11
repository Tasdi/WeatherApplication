using Android.App;
using Android.Widget;
using Android.OS;
using WeatherApplication.Services;
using WeatherApplication.Models;
using System.Text.RegularExpressions;

namespace WeatherApplication
{
    [Activity(Label = "WeatherApplication", MainLauncher = true)]
    public class MainActivity : Activity
    {
        IRequestHandler requestHandler;
        WeatherInformation weatherInformation;

        Button searchBtn;
        CheckBox celsius, fahren;
        EditText userInput;
        GridLayout defaultGrid;
        TextView temperatureDefault, weatherDescription, countryCode, windSpeed, test;
        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Initializing class that will handle requests from user
            requestHandler = new RequestHandler();

            // Set the view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Initializing components
            InitializeGuiComponents();
            
            // Eventhandler, invoked when user presses "Search" button
            searchBtn.Click += (o, e) =>
            {
                GenerateDefaultInformation();
            };

            // Toggle between Fahrenheit and Celsius checkboxes FindViewById<CheckBox>(Resource.Id.celsius).Click
            celsius.Click += (e, o) =>
            {
                if (celsius.Checked)
                {
                    fahren.Checked = false;
                }
                else
                {
                    fahren.Checked = true;
                }

                SetCorrectTemp();
            };

            // Toggle between Fahrenheit and Celsius checkboxes
            fahren.Click += (e, o) =>
            {
                if (fahren.Checked)
                {
                    celsius.Checked = false;
                }
                else
                {
                    celsius.Checked = true;
                }

                SetCorrectTemp();
            };
        }

        private void SetCorrectTemp()
        {
            if (celsius.Checked)
            {
                fahren.Checked = false;

                if (!userInput.Text.Equals(""))
                {
                    ConvertTemperature(weatherInformation.main.temp, "toCelsius");
                }
            }
            if (fahren.Checked)
            {
                celsius.Checked = false;

                if (!userInput.Text.Equals(""))
                {
                    ConvertTemperature(weatherInformation.main.temp, "toFahrenheit");
                }
            }
        }

        private void ConvertTemperature(double tempInKelvin, string convertTo)
        {
            double convertedTemp = 0;

            if (convertTo.Equals("toFahrenheit"))
            {
                convertedTemp = (9 / 5 * (tempInKelvin - 273)) + 32;
                
            }
            else if (convertTo.Equals("toCelsius"))
            {
                convertedTemp = tempInKelvin - 273.15;
            }

            temperatureDefault.Text = convertedTemp.ToString();
        }

        private void InitializeGuiComponents()
        {
            // Initialize checkboxes
            celsius = FindViewById<CheckBox>(Resource.Id.celsius);
            fahren = FindViewById<CheckBox>(Resource.Id.fahrenheit);

            // Initialize textfields
            userInput = FindViewById<EditText>(Resource.Id.cityInput);
            temperatureDefault = FindViewById<TextView>(Resource.Id.temperatureRes);
            weatherDescription = FindViewById<TextView>(Resource.Id.descriptionRes);
            countryCode = FindViewById<TextView>(Resource.Id.countryCodeRes);
            windSpeed = FindViewById<TextView>(Resource.Id.windRes);

            // Initialize gridlayouts
            defaultGrid = FindViewById<GridLayout>(Resource.Id.defaultValuesGrid);

            // Initialize searchbutton
            searchBtn = FindViewById<Button>(Resource.Id.searchBtn);

            test = FindViewById<TextView>(Resource.Id.resPressure);

            celsius.Checked = true;
            fahren.Checked = false;
        }

        private void GenerateDefaultInformation()
        {
            // Reg expressions to check whether input is allowed or not
            string checkInteger = @"\d+";
            string checkSpecialCharacter = @"[@#$%&*+\-_(),+':;?.,![\]\s\\/]+$";

            // If input contains invalid characters, let the user know and retry
            if (userInput.Text == "" || Regex.Match(userInput.Text, checkInteger).Success ||
                Regex.Match(userInput.Text, checkSpecialCharacter).Success)
            {
                // Print out debug message
                Toast.MakeText(this, "Invalid input. Please try again", ToastLength.Long).Show();
                // Will clear the values displayed from response
                UpdateDefaultComponents();
            }
            else
            {
                weatherInformation = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString());

                if (weatherInformation == null)
                {
                    Toast.MakeText(this, $"Could not find any data for '{userInput.Text.ToString()}'", ToastLength.Long).Show();
                    // Will clear the values displayed from response
                    UpdateDefaultComponents();
                }
                else
                {
                    if (fahren.Checked)
                    {
                        ConvertTemperature(weatherInformation.main.temp, "toFahrenheit");
                    }
                    else
                    {
                        ConvertTemperature(weatherInformation.main.temp, "toCelsius");
                    }

                    windSpeed.Text = weatherInformation.wind.speed.ToString();
                    weatherDescription.Text = weatherInformation.weather[0].description.ToString();
                    countryCode.Text = weatherInformation.sys.country.ToString();
                }
            }
        }

        private void UpdateDefaultComponents()
        {
            for (int i = 1; i < defaultGrid.ChildCount; i+=2)
            {
                TextView child = (TextView)defaultGrid.GetChildAt(i);

                if (child.Tag != null)
                {
                    child.Text = "Value ot found";
                }
            }
        }
    }
}

