using Android.App;
using Android.Widget;
using Android.OS;
using System;
using WeatherApplication.Services;
using System.Text.RegularExpressions;

namespace WeatherApplication
{
    [Activity(Label = "WeatherApplication", MainLauncher = true)]
    public class MainActivity : Activity
    {
        CheckBox celsius, fahren;
        EditText userInput;
        TextView result;
        IRequestHandler requestHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Initializing class that will handle requests from user
            requestHandler = new RequestHandler();

            // Set the view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Initializing components
            userInput = FindViewById<EditText>(Resource.Id.cityInput);

            celsius = FindViewById<CheckBox>(Resource.Id.celsius);
            celsius.Checked = true;

            fahren = FindViewById<CheckBox>(Resource.Id.fahrenheit);
            fahren.Checked = false;

            result = FindViewById<TextView>(Resource.Id.resultTxt);

            // Eventhandler, invoked when user presses "Search" button
            FindViewById<Button>(Resource.Id.searchBtn).Click += (o, e) =>
            {
                // Reg expressions to check whether input is allowed or not
                string checkInteger = @"\d+";
                string checkSpecialCharacter = @"[@#$%&*+\-_(),+':;?.,![\]\s\\/]+$";

                // If input contains invalid characters, let the user know and retry
                if (userInput.Text == "" || Regex.Match(userInput.Text, checkInteger).Success || 
                    Regex.Match(userInput.Text, checkSpecialCharacter).Success) 
                {
                    result.Text = "Invalid input. Please try again";
                    return;
                }

                // 
                if (fahren.Checked)
                {
                    result.Text = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString(), true);
                }
                else
                {
                    result.Text = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString(), false);
                }
            };

            // Toggle between Fahrenheit and Celsius checkboxes
            FindViewById<CheckBox>(Resource.Id.celsius).Click += (e, o) =>
            {
                if (celsius.Checked)
                {
                    fahren.Checked = false;
                }
                else
                {
                    fahren.Checked = true;
                }
            };

            // Toggle between Fahrenheit and Celsius checkboxes
            FindViewById<CheckBox>(Resource.Id.fahrenheit).Click += (e, o) =>
            {
                if (fahren.Checked)
                {
                    celsius.Checked = false;
                }
                else
                {
                    celsius.Checked = true;
                }
            };
        }
    }
}

