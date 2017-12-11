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
        CheckBox celsius, fahren, minTmp, maxTmp, humidity, pressure, coordinates;
        EditText userInput;
        GridLayout defaultGrid, optionalGrid;
        TextView temperatureDefault, weatherDescription, countryCode, windSpeed;
        TextView resMinTmp, resMaxTmp, resHumidity, resPressure, resCoordinates;
        
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
                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
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

                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
            };

            minTmp.Click += (e, o) =>
            {
                string min = "";
                if (celsius.Checked)
                {
                    min = ConvertTemperature(weatherInformation.main.temp_min, "toCelsius");
                }
                else
                {
                    min = ConvertTemperature(weatherInformation.main.temp_min, "toFahrenheit");
                }

                UpdateOptionalComponents(minTmp, resMinTmp, min);
            };

            maxTmp.Click += (e, o) =>
            {
                string max = "";

                if (celsius.Checked)
                {
                    max = ConvertTemperature(weatherInformation.main.temp_max, "toCelsius");
                }
                else
                {
                    max = ConvertTemperature(weatherInformation.main.temp_max, "toFahrenheit");
                }

                UpdateOptionalComponents(maxTmp, resMaxTmp, max);
            };

            humidity.Click += (e, o) =>
            {
                UpdateOptionalComponents(humidity, resHumidity, weatherInformation.main.humidity.ToString());
            };

            pressure.Click += (e, o) =>
            {
                UpdateOptionalComponents(pressure, resPressure, weatherInformation.main.pressure.ToString());
            };

            coordinates.Click += (e, o) =>
            {
                UpdateOptionalComponents(coordinates, resCoordinates,
                                        weatherInformation.coord.lon.ToString() + " " + weatherInformation.coord.lat.ToString());
            };
        }

        private void SetCorrectTemp()
        {
            string setMin = "";
            string setMax = "";
            if (celsius.Checked)
            {
                fahren.Checked = false;

                if (!userInput.Text.Equals(""))
                {
                    temperatureDefault.Text = ConvertTemperature(weatherInformation.main.temp, "toCelsius");
                    setMin = ConvertTemperature(weatherInformation.main.temp_min, "toCelsius");
                    setMax = ConvertTemperature(weatherInformation.main.temp_max, "toCelsius");
                    UpdateOptionalComponents(minTmp, resMinTmp, setMin);
                    UpdateOptionalComponents(maxTmp, resMaxTmp, setMax);
                }
            }
            if (fahren.Checked)
            {
                celsius.Checked = false;

                if (!userInput.Text.Equals(""))
                {
                    temperatureDefault.Text = ConvertTemperature(weatherInformation.main.temp, "toFahrenheit");
                    setMin = ConvertTemperature(weatherInformation.main.temp_min, "toFahrenheit");
                    setMax = ConvertTemperature(weatherInformation.main.temp_max, "toFahrenheit");
                    UpdateOptionalComponents(minTmp, resMinTmp, setMin);
                    UpdateOptionalComponents(maxTmp, resMaxTmp, setMax);
                }
            }
        }

        private string ConvertTemperature(double tempInKelvin, string convertTo)
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

            return convertedTemp.ToString();
        }

        private void InitializeGuiComponents()
        {
            // Initialize checkboxes
            celsius = FindViewById<CheckBox>(Resource.Id.celsius);
            fahren = FindViewById<CheckBox>(Resource.Id.fahrenheit);

            minTmp = FindViewById<CheckBox>(Resource.Id.optMinTemp);
            maxTmp = FindViewById<CheckBox>(Resource.Id.optMaxTemp);
            humidity = FindViewById<CheckBox>(Resource.Id.optHumidity);
            pressure = FindViewById<CheckBox>(Resource.Id.optPressure);
            coordinates = FindViewById<CheckBox>(Resource.Id.optCoordinates);

            // Initialize textfields
            userInput = FindViewById<EditText>(Resource.Id.cityInput);
            temperatureDefault = FindViewById<TextView>(Resource.Id.temperatureRes);
            weatherDescription = FindViewById<TextView>(Resource.Id.descriptionRes);
            countryCode = FindViewById<TextView>(Resource.Id.countryCodeRes);
            windSpeed = FindViewById<TextView>(Resource.Id.windRes);
            
            resMinTmp = FindViewById<TextView>(Resource.Id.resMinTemp);
            resMaxTmp = FindViewById<TextView>(Resource.Id.resMaxTemp);
            resHumidity = FindViewById<TextView>(Resource.Id.resHumidity);
            resPressure = FindViewById<TextView>(Resource.Id.resPressure);
            resCoordinates = FindViewById<TextView>(Resource.Id.resCoordinates);

            // Initialize gridlayouts
            defaultGrid = FindViewById<GridLayout>(Resource.Id.defaultValuesGrid);
            optionalGrid = FindViewById<GridLayout>(Resource.Id.optionalGrid);

            // Initialize searchbutton
            searchBtn = FindViewById<Button>(Resource.Id.searchBtn);

            celsius.Checked = true;
            fahren.Checked = false;

            minTmp.Enabled = false;
            maxTmp.Enabled = false;
            humidity.Enabled = false;
            pressure.Enabled = false;
            coordinates.Enabled = false;
        }

        private void GenerateDefaultInformation()
        {
            // Reg expressions to check whether input is allowed or not
            string checkInteger = @"\d+";
            string checkSpecialCharacter = @"[@#$%&*+\-_(),+':;?.,![\]\s\\/]+$";

            bool invalidSearch = false;

            // If input contains invalid characters, let the user know and retry
            if (userInput.Text == "" || Regex.Match(userInput.Text, checkInteger).Success ||
                Regex.Match(userInput.Text, checkSpecialCharacter).Success)
            {
                // Take care of cases if input is invalid
                invalidSearch = true;
            }
            else
            {
                // Get weather information
                weatherInformation = null;
                weatherInformation = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString());

                if (weatherInformation == null)
                {
                    invalidSearch = true;
                }
                else
                {
                    if (fahren.Checked)
                    {
                        temperatureDefault.Text = ConvertTemperature(weatherInformation.main.temp, "toFahrenheit");
                    }
                    else
                    {
                        temperatureDefault.Text = ConvertTemperature(weatherInformation.main.temp, "toCelsius");
                    }

                    windSpeed.Text = weatherInformation.wind.speed.ToString();
                    weatherDescription.Text = weatherInformation.weather[0].description.ToString();
                    countryCode.Text = weatherInformation.sys.country.ToString();

                    minTmp.Enabled = true;
                    maxTmp.Enabled = true;
                    humidity.Enabled = true;
                    pressure.Enabled = true;
                    coordinates.Enabled = true;
                }
            }

            if (invalidSearch)
            {
                // Print debug info
                Toast.MakeText(this, $"Could not find any data for '{userInput.Text.ToString()}'", ToastLength.Long).Show();
                // Will clear the values displayed from response
                ResetDefaultComponents();
                ResetOptionalValues();
            }
            else
            {
                OverrideOptionalValues();
            }
        }

        private void ResetOptionalValues()
        {
            for (int i = 0; i < optionalGrid.ChildCount; i += 2)
            {
                CheckBox checkBoxChild = (CheckBox)optionalGrid.GetChildAt(i);
                checkBoxChild.Enabled = false;
                checkBoxChild.Checked = false;

                TextView txtViewChild = (TextView)optionalGrid.GetChildAt(i + 1);

                UpdateOptionalComponents(checkBoxChild, txtViewChild, "");
            }
        }

        private void OverrideOptionalValues()
        {
            for (int i = 0; i < optionalGrid.ChildCount; i += 2)
            {
                CheckBox checkBoxChild = (CheckBox)optionalGrid.GetChildAt(i);

                if (checkBoxChild.Checked)
                {
                    TextView txtViewChild = (TextView)optionalGrid.GetChildAt(i + 1);

                    string childTag = txtViewChild.Tag.ToString();

                    switch (childTag)
                    {
                        case "1":   
                        case "3":
                            SetCorrectTemp();
                        break;
                        case "5":
                            UpdateOptionalComponents(checkBoxChild, txtViewChild, weatherInformation.main.humidity.ToString());
                            break;
                        case "7":
                            UpdateOptionalComponents(checkBoxChild, txtViewChild, weatherInformation.main.pressure.ToString());
                            break;
                        case "9":
                            UpdateOptionalComponents(checkBoxChild, txtViewChild, weatherInformation.coord.lon.ToString() + "" + 
                                weatherInformation.coord.lat.ToString());
                            break;
                    }
                }
            }
        }

        private void ResetDefaultComponents()
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

        private void UpdateOptionalComponents(CheckBox checkBox, TextView textView, string weatherInfo)
        {
            if (weatherInformation != null)
            {
                if (checkBox.Checked)
                {
                    textView.Text = weatherInfo;
                }
                else
                {
                    textView.Text = "";
                }
            }
            else
            {
                textView.Text = "";
            }
        }
    }
}
