using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Appwidget;
using Xamarin.Essentials;
using Android.Content;

namespace CONNECTClient_Xamarin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Get the UI controls from the loaded layout
            EditText usernameText = FindViewById<EditText>(Resource.Id.UsernameText);
            EditText ipAddreddText = FindViewById<EditText>(Resource.Id.IpAddressText);
            EditText portText = FindViewById<EditText>(Resource.Id.PortText);
            Button connectButton = FindViewById<Button>(Resource.Id.ConnectButton);

            connectButton.Click += (sender, e) =>
            {
                try
                {
                    Core.Client.Connect(usernameText.Text, ipAddreddText.Text, int.Parse(portText.Text));
                    Intent nextActivity = new Intent(this, typeof(ConnectedActivity));
                    StartActivity(nextActivity);
                }
                catch
                {
                    if (usernameText.Text == "" || ipAddreddText.Text == "" || portText.Text == "")
                    {
                        Toast.MakeText(ApplicationContext, "You have to complete all the fields!", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(ApplicationContext, "Wrong IP address or port.", ToastLength.Long).Show();
                    }
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}