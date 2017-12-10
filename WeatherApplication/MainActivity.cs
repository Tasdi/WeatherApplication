using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace WeatherApplication
{
    [Activity(Label = "WeatherApplication", MainLauncher = true)]
    public class MainActivity : Activity
    {
        CheckBox celsius, fahren;
        EditText userInput;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            userInput = FindViewById<EditText>(Resource.Id.cityInput);

            celsius = FindViewById<CheckBox>(Resource.Id.celsius);
            celsius.Checked = true;

            fahren = FindViewById<CheckBox>(Resource.Id.fahrenheit);
            fahren.Checked = false;

            FindViewById<Button>(Resource.Id.searchBtn).Click += (o, e) =>
            {
                if (fahren.Checked)
                {
                    userInput.Text = "Fahrenheit";
                }
                else
                {
                    userInput.Text = "Celsius";
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

