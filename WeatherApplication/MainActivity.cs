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

            //  Creating our services and models

            requestHandler = new RequestHandler();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            userInput = FindViewById<EditText>(Resource.Id.cityInput);

            celsius = FindViewById<CheckBox>(Resource.Id.celsius);
            celsius.Checked = true;

            fahren = FindViewById<CheckBox>(Resource.Id.fahrenheit);
            fahren.Checked = false;

            result = FindViewById<TextView>(Resource.Id.resultTxt);

            // Change to searchBtn.Click
            FindViewById<Button>(Resource.Id.searchBtn).Click += (o, e) =>
            {
                string checkInteger = @"\d+";
                string checkSpecialCharacter = @"[@#$%&*+\-_(),+':;?.,![\]\s\\/]+$";

                if (userInput.Text == "" || Regex.Match(userInput.Text, checkInteger).Success || 
                    Regex.Match(userInput.Text, checkSpecialCharacter).Success) 
                {
                    result.Text = "Invalid input. Please try again";
                    return;
                }

                if (fahren.Checked)
                {
                    result.Text = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString(), true);
                }
                else
                {
                    result.Text = requestHandler.FetchDataFromInputAsync(userInput.Text.ToString(), false);
                }
            };

            FindViewById<CheckBox>(Resource.Id.celsius).Click += (e, o) =>
            {
                if (celsius.Checked)
                {
                    Console.WriteLine(celsius.Checked);
                    fahren.Checked = false;
                }
                else
                {
                    Console.WriteLine(celsius.Checked);
                    fahren.Checked = true;
                }
            };

            FindViewById<CheckBox>(Resource.Id.fahrenheit).Click += (e, o) =>
            {
                if (fahren.Checked)
                {
                    Console.WriteLine(fahren.Checked);
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

