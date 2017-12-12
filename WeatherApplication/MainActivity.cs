using Android.App;
using Android.Widget;
using Android.OS;
using WeatherApplication.Services;
using WeatherApplication.Models;
using System.Text.RegularExpressions;
using System;

namespace WeatherApplication
{
    [Activity(Label = "WeatherApplication", MainLauncher = true)]
    public class MainActivity : Activity
    {
        // Declare objects of class that will be needed to request info and show it in application
        IRequestHandler requestHandler;
        WeatherInformation weatherInformation;
        // Declare components used in the application
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

            // Check whether user selected or unselected Celsius option
            celsius.Click += (e, o) =>
            {
                // If Celsius is choosen by user, set fahrenheit option to unselected
                if (celsius.Checked)
                {
                    fahren.Checked = false;
                }
                // If user unselects celsius option, the program selects fahrenheit option
                else
                {
                    fahren.Checked = true;
                }
                // If weather information is retrieved properly, show temp in correct format (depending on user's choice)
                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
            };

            // If Fahrenheit is choosen by user, set celsius option to unselected
            fahren.Click += (e, o) =>
            {
                // If fahrenheit is choosen by user, set celsius option to unselected
                if (fahren.Checked)
                {
                    celsius.Checked = false;
                }
                // If user unselects fahrenheit option, the program selects celsius option
                else
                {
                    celsius.Checked = true;
                }
                // If weather information is retrieved properly, show temp in correct format (depending on user's choice)
                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
            };

            // This eventhandler is invoked when user chooses to display min temp
            minTmp.Click += (e, o) =>
            {
                // Declare string which will store the correct value of temperature (celsius/fahren)
                string setCorrectMin = "";
                // Convert temperature in Kelvin to C/F depending on what user has chosen
                if (celsius.Checked)
                {
                    setCorrectMin = ConvertTemperature(weatherInformation.main.temp_min, "toCelsius");
                }
                else
                {
                    setCorrectMin = ConvertTemperature(weatherInformation.main.temp_min, "toFahrenheit");
                }
                // Update the corresponding components in GUI
                UpdateOptionalComponents(minTmp, resMinTmp, setCorrectMin);
            };

            // Same functionality as minTmp.Click (above). Difference: max temp instead of min
            maxTmp.Click += (e, o) =>
            {
                string setCorrectMax = "";

                if (celsius.Checked)
                {
                    setCorrectMax = ConvertTemperature(weatherInformation.main.temp_max, "toCelsius");
                }
                else
                {
                    setCorrectMax = ConvertTemperature(weatherInformation.main.temp_max, "toFahrenheit");
                }

                UpdateOptionalComponents(maxTmp, resMaxTmp, setCorrectMax);
            };

            // This eventhandler is invoked if a user wants to display humidity
            humidity.Click += (e, o) =>
            {
                // Update the corresponding components for humidity in GUI
                UpdateOptionalComponents(humidity, resHumidity, weatherInformation.main.humidity.ToString());
            };

            // This eventhandler is invoked if a user wants to display the pressure
            pressure.Click += (e, o) =>
            {
                // Update corresponding component in GUI
                UpdateOptionalComponents(pressure, resPressure, weatherInformation.main.pressure.ToString());
            };

            // This eventhandler is invoked if a user wants to display the coordinates (longitude and latitude)
            coordinates.Click += (e, o) =>
            {
                // Update corresponding components in GUI
                UpdateOptionalComponents(coordinates, resCoordinates,
                                        weatherInformation.coord.lon.ToString() + " " + weatherInformation.coord.lat.ToString());
            };
        }

        /* This function checks which format the user wants to see the temperature in and updates necessary components
         * in GUI accordingly
         */
        private void SetCorrectTemp()
        {
            string setMin = "";
            string setMax = "";

            if (celsius.Checked)
            {
                fahren.Checked = false;

                if (weatherInformation != null)
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

                if (weatherInformation != null)
                {
                    temperatureDefault.Text = ConvertTemperature(weatherInformation.main.temp, "toFahrenheit");
                    setMin = ConvertTemperature(weatherInformation.main.temp_min, "toFahrenheit");
                    setMax = ConvertTemperature(weatherInformation.main.temp_max, "toFahrenheit");

                    UpdateOptionalComponents(minTmp, resMinTmp, setMin);
                    UpdateOptionalComponents(maxTmp, resMaxTmp, setMax);
                }
            }
        }

        /* This function takes a double and string as inparameters.
         * The value stored in double is the original temperature that
         * is returned by response in API. The string contains which
         * type Kelvin is to be converted to. The function converts
         * the temperatur in kelvin and returns the value in string format.
         */
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

            // Rounds up the temperature to 2 decimal places
            convertedTemp = Math.Round(convertedTemp, 2);
            return convertedTemp.ToString();
        }

        /* This function initializes references to all the components that
         * exist in the GUI so that the components can be manipulated.
         */
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

            // Set default properties for some of the componenets
            celsius.Checked = true;
            fahren.Checked = false;

            minTmp.Enabled = false;
            maxTmp.Enabled = false;
            humidity.Enabled = false;
            pressure.Enabled = false;
            coordinates.Enabled = false;
        }

        /* This function is called by the eventhandler that is invoked
         * once a user searches for a city. The function checks whether
         * the user input is valid or invalid and sets up the GUI accordingly.
         */
        private void GenerateDefaultInformation()
        {
            // Reg expressions to check whether input is allowed or not
            string checkInteger = @"\d+";
            string checkSpecialCharacter = @"[@#$%&*+\-_(),+':;?.,![\]\s\\/]+$";

            // Remove spaces from input
            string input = Regex.Replace(userInput.Text.ToString(), @"\s+", "");
            // Put the string specified without spaces (if there exist any from beginning)
            userInput.Text = input;

            bool invalidSearch = false;

            // If input contains invalid characters, let the user know and retry
            if (userInput.Text == "" || Regex.Match(userInput.Text, checkInteger).Success ||
                Regex.Match(userInput.Text, checkSpecialCharacter).Success)
            {
                // Take care of cases if input is invalid
                invalidSearch = true;
                // Ensures old values will not appear
                weatherInformation = null;
            }
            else
            {
                // Get weather information
                weatherInformation = requestHandler.FetchDataFromInputAsync(input);

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

                    // Sets the default values and displays it on screen
                    windSpeed.Text = weatherInformation.wind.speed.ToString();
                    weatherDescription.Text = weatherInformation.weather[0].description.ToString();
                    countryCode.Text = weatherInformation.sys.country.ToString();

                    // A user can now choose to display optional values
                    minTmp.Enabled = true;
                    maxTmp.Enabled = true;
                    humidity.Enabled = true;
                    pressure.Enabled = true;
                    coordinates.Enabled = true;
                }
            }

            if (invalidSearch)
            {
                // Print debug info on display
                Toast.MakeText(this, $"Could not find any data for '{userInput.Text.ToString()}'", ToastLength.Long).Show();
                // Will reset values
                ResetDefaultComponents();
                ResetOptionalValues();
            }
            else
            {
                // If some options are left as chosen when user queries information
                // about a new city, the corresponding values will be override by new information
                OverrideOptionalValues();
            }
        }

        /* This method is invoked if user input was invalid, i.e.
         * returned data from API is null. The method will
         * iterate over the DEFAULT components in GUI and reset its field.
         */
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

        /* This method is invoked if user input was invalid, i.e.
         * returned data from API is null. The method will
         * iterate over the OPTIONAL components in GUI and reset its field.
         */
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

        /* This method is invoked when a user performs a new query
         * but has left options of a old query selected. The method
         * will iterate over the selected options and update its value
         * with the new values that is retreived from the API.
         */
        private void OverrideOptionalValues()
        {
            // Only need to iterate over necessary components,
            // hence incremented by 2 in each iteration.
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

        /* This method is invoked when a optional component has
         * been changed, i.e. when a user requests a new query.
         * The function takes two views and a string as inparameter.
         * The checkbox indicates whether information should be displayed
         * immidiately or not. The information that should be displayed
         * is specified by the string "weatherInfo" and the text is placed
         * on corresponding textview.
         */
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
