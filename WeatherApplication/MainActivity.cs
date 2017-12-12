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
        // Declare instances of class that will be needed to request info
        IRequestHandler requestHandler;
        WeatherInformation weatherInformation;
        // Declare components used in the application
        Button searchBtn;
        CheckBox celsiusCBox, fahrenCBox, minTmpCBox, maxTmpCBox, humidityCBox, pressureCBox, coordinatesCBox;
        EditText userInput;
        GridLayout defaultGrid, optionalGrid;
        TextView temperatureDefaultView, weatherDescriptionView, countryCodeView, windSpeedView;
        TextView resMinTmpView, resMaxTmpView, resHumidityView, resPressureView, resCoordinatesView;

        RadioButton radioButton;
        
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
            celsiusCBox.Click += (e, o) =>
            { 
                // If Celsius is choosen by user, set fahrenheit option to unselected. Opposite way otherwise
                fahrenCBox.Checked = !celsiusCBox.Checked;

                // If weather information is retrieved properly, show temp in correct format (depending on user's choice)
                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
            };

            // If Fahrenheit is choosen by user, set celsius option to unselected
            fahrenCBox.Click += (e, o) =>
            {
                celsiusCBox.Checked = !fahrenCBox.Checked;
                
                // If weather information is retrieved properly, show temp in correct format (depending on user's choice)
                if (weatherInformation != null)
                {
                    SetCorrectTemp();
                }
            };

            // This eventhandler is invoked when user chooses to display min temp
            minTmpCBox.Click += (e, o) =>
            {
                SetOptionalTemp(minTmpCBox, resMinTmpView, weatherInformation.main.temp_min);
            };

            // Same functionality as minTmp.Click (above). Difference: max temp instead of min
            maxTmpCBox.Click += (e, o) =>
            {
                SetOptionalTemp(maxTmpCBox, resMaxTmpView, weatherInformation.main.temp_max);
            };

            // This eventhandler is invoked if a user wants to display humidity
            humidityCBox.Click += (e, o) =>
            {
                // Update the corresponding components for humidity in GUI
                UpdateOptionalComponents(humidityCBox, resHumidityView, weatherInformation.main.humidity.ToString());
            };

            // This eventhandler is invoked if a user wants to display the pressure
            pressureCBox.Click += (e, o) =>
            {
                // Update corresponding component in GUI
                UpdateOptionalComponents(pressureCBox, resPressureView, weatherInformation.main.pressure.ToString());
            };

            // This eventhandler is invoked if a user wants to display the coordinates (longitude and latitude)
            coordinatesCBox.Click += (e, o) =>
            {
                // Update corresponding components in GUI
                UpdateOptionalComponents(coordinatesCBox, resCoordinatesView,
                                        "(" + weatherInformation.coord.lon.ToString() + ":" + weatherInformation.coord.lat.ToString() + ")");
            };
        }

        /*
         * Takes a temperature (min or max), and the related components
         * (for min or max temp) in GUI as inparameter when user checks
         * corresponding CheckBox. Sets the correct value in GUI.
         */
         private void SetOptionalTemp (CheckBox checkBox, TextView textView, double temperature)
        {
            // Declare string which will store the correct value of temperature (celsius/fahren)
            string correctTemp = "";

            // Convert temperature in Kelvin to C/F depending on what user has chosen
            if (celsiusCBox.Checked)
            {
                correctTemp = ConvertTemperature(temperature, true);
            }
            else
            {
                correctTemp = ConvertTemperature(temperature, false);
            }

            // Update the corresponding components in GUI
            UpdateOptionalComponents(checkBox, textView, correctTemp);
        }


        /* This function checks which format the user wants to see the temperature in and updates necessary components
         * in GUI accordingly
         */
        private void SetCorrectTemp()
        {
            string setMin = "";
            string setMax = "";
            bool checkChoosenTemp = false;

            if (celsiusCBox.Checked)
            {
                fahrenCBox.Checked = false;
                checkChoosenTemp = true;
            }

            if (fahrenCBox.Checked)
            {
                celsiusCBox.Checked = false;
                checkChoosenTemp = false;
            }
            
            if (weatherInformation != null)
            {
                temperatureDefaultView.Text = ConvertTemperature(weatherInformation.main.temp, checkChoosenTemp);
                setMin = ConvertTemperature(weatherInformation.main.temp_min, checkChoosenTemp);
                setMax = ConvertTemperature(weatherInformation.main.temp_max, checkChoosenTemp);

                UpdateOptionalComponents(minTmpCBox, resMinTmpView, setMin);
                UpdateOptionalComponents(maxTmpCBox, resMaxTmpView, setMax);
            }
        }

        /* This function takes a double and boolean as inparameters.
         * The value stored in double is the original temperature that
         * is returned by response in API. The boolean indicates which
         * type Kelvin is to be converted to. The function converts
         * the temperatur in kelvin and returns the value in string format.
         */
        private string ConvertTemperature(double tempInKelvin, bool convertTo)
        {
            double convertedTemp = 0;

            // If boolean is false, convert from Kelvin to Fahrenheit
            if (!convertTo)
            {
                convertedTemp = (9 / 5 * (tempInKelvin - 273)) + 32;
                
            }
            // Convert to celsius otherwise
            else
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
            celsiusCBox = FindViewById<CheckBox>(Resource.Id.celsius);
            fahrenCBox = FindViewById<CheckBox>(Resource.Id.fahrenheit);

            minTmpCBox = FindViewById<CheckBox>(Resource.Id.optMinTemp);
            maxTmpCBox = FindViewById<CheckBox>(Resource.Id.optMaxTemp);
            humidityCBox = FindViewById<CheckBox>(Resource.Id.optHumidity);
            pressureCBox = FindViewById<CheckBox>(Resource.Id.optPressure);
            coordinatesCBox = FindViewById<CheckBox>(Resource.Id.optCoordinates);

            // Initialize textfields
            userInput = FindViewById<EditText>(Resource.Id.cityInput);
            temperatureDefaultView = FindViewById<TextView>(Resource.Id.temperatureRes);
            weatherDescriptionView = FindViewById<TextView>(Resource.Id.descriptionRes);
            countryCodeView = FindViewById<TextView>(Resource.Id.countryCodeRes);
            windSpeedView = FindViewById<TextView>(Resource.Id.windRes);
            
            resMinTmpView = FindViewById<TextView>(Resource.Id.resMinTemp);
            resMaxTmpView = FindViewById<TextView>(Resource.Id.resMaxTemp);
            resHumidityView = FindViewById<TextView>(Resource.Id.resHumidity);
            resPressureView = FindViewById<TextView>(Resource.Id.resPressure);
            resCoordinatesView = FindViewById<TextView>(Resource.Id.resCoordinates);

            // Initialize gridlayouts
            defaultGrid = FindViewById<GridLayout>(Resource.Id.defaultValuesGrid);
            optionalGrid = FindViewById<GridLayout>(Resource.Id.optionalGrid);

            // Initialize searchbutton
            searchBtn = FindViewById<Button>(Resource.Id.searchBtn);

            // Set default properties for some of the componenets
            celsiusCBox.Checked = true;
            fahrenCBox.Checked = false;

            minTmpCBox.Enabled = false;
            maxTmpCBox.Enabled = false;
            humidityCBox.Enabled = false;
            pressureCBox.Enabled = false;
            coordinatesCBox.Enabled = false;
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

            // Remove new lines
            string input = userInput.Text;
            input = Regex.Replace(input, @"\t|\n|\r", "");
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
                weatherInformation = requestHandler.FetchDataFromInput(userInput.Text.ToString());

                if (weatherInformation == null)
                {
                    invalidSearch = true;
                }
                else
                {
                    if (fahrenCBox.Checked)
                    {
                        temperatureDefaultView.Text = ConvertTemperature(weatherInformation.main.temp, false);
                    }
                    else
                    {
                        temperatureDefaultView.Text = ConvertTemperature(weatherInformation.main.temp, true);
                    }

                    // Sets the default values and displays it on screen
                    windSpeedView.Text = weatherInformation.wind.speed.ToString();
                    weatherDescriptionView.Text = weatherInformation.weather[0].description.ToString();
                    countryCodeView.Text = weatherInformation.sys.country.ToString();

                    // A user can now choose to display optional values
                    minTmpCBox.Enabled = true;
                    maxTmpCBox.Enabled = true;
                    humidityCBox.Enabled = true;
                    pressureCBox.Enabled = true;
                    coordinatesCBox.Enabled = true;
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
            for (int i = 0; i < optionalGrid.ChildCount; i+=2)
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
                    child.Text = "Value not found";
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
                            UpdateOptionalComponents(checkBoxChild, txtViewChild, "(" + weatherInformation.coord.lon.ToString() + ":" + 
                                weatherInformation.coord.lat.ToString() + ")");
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
            if (weatherInformation != null && checkBox.Checked)
            {
                textView.Text = weatherInfo;
            }
            else
            {
                textView.Text = "";
            }
        }
    }
}
