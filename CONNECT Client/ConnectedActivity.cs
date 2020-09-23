using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace CONNECTClient_Xamarin
{
    [Activity(Label = "ConnectedActivity")]
    public class ConnectedActivity : Activity
    {
        int lightEffect = 5;
        string[] effectsList = new string[7] {"31", "32a", "32b", "32c", "32d", "33", "34"};

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            string lightState = "10";
            string red = "255";
            string green = "000";
            string blue = "000";
            

            // Create your application here
            SetContentView(Resource.Layout.activity_connected);

            // Get the UI controls from the loaded layout
            ToggleButton ledStateToggleButton = FindViewById<ToggleButton>(Resource.Id.LedStateToggleButton);
            TextView colorTextView = FindViewById<TextView>(Resource.Id.ColorTextView);
            SeekBar redSeekBar = FindViewById<SeekBar>(Resource.Id.RedSeekBar);
            SeekBar greenSeekBar = FindViewById<SeekBar>(Resource.Id.GreenSeekBar);
            SeekBar blueSeekBar = FindViewById<SeekBar>(Resource.Id.BlueSeekBar);
            Spinner effectsSpinner = FindViewById<Spinner>(Resource.Id.EffectsSpinner);
            Button disconnectButton = FindViewById<Button>(Resource.Id.DisconnectButton);

            effectsSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.effects_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            effectsSpinner.Adapter = adapter;

            ledStateToggleButton.Click += (sender, e) =>
            {
                if (ledStateToggleButton.Checked)
                {
                    lightState = "11";
                }
                else
                {
                    lightState = "10";
                }
                Core.Client.SendMessage(-1, lightState);
            };

            redSeekBar.ProgressChanged += (sender, e) =>
            {
                colorTextView.SetBackgroundColor(Color.Rgb(e.Progress, greenSeekBar.Progress, blueSeekBar.Progress));

                red = e.Progress.ToString();
                if (red.Length == 1)
                {
                    red = "00" + red;
                }
                else if (red.Length == 2)
                {
                    red = "0" + red;
                }
                
                Core.Client.SendMessage(-1, $"2{red}{green}{blue}");
            };
            greenSeekBar.ProgressChanged += (sender, e) =>
            {
                colorTextView.SetBackgroundColor(Color.Rgb(redSeekBar.Progress, e.Progress, blueSeekBar.Progress));

                green = e.Progress.ToString();
                if (green.Length == 1)
                {
                    green = "00" + green;
                }
                else if (green.Length == 2)
                {
                    green = "0" + green;
                }

                Core.Client.SendMessage(-1, $"2{red}{green}{blue}");
            };
            blueSeekBar.ProgressChanged += (sender, e) =>
            {
                colorTextView.SetBackgroundColor(Color.Rgb(redSeekBar.Progress, greenSeekBar.Progress, e.Progress));

                blue = e.Progress.ToString();
                if (blue.Length == 1)
                {
                    blue = "00" + blue;
                }
                else if (blue.Length == 2)
                {
                    blue = "0" + blue;
                }

                Core.Client.SendMessage(-1, $"2{red}{green}{blue}");
            };

            disconnectButton.Click += (sender, e) =>
            {
                Core.Client.Disconnect();
                Intent nextActivity = new Intent(this, typeof(MainActivity));
                StartActivity(nextActivity);
            };
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            string toast = string.Format("Selected effect is {0}", spinner.GetItemAtPosition(e.Position));
            Toast.MakeText(this, toast, ToastLength.Long).Show();

            if (e.Position != lightEffect)
            {
                lightEffect = e.Position;

                Core.Client.SendMessage(-1, effectsList[lightEffect]);
            }
        }
    }
}