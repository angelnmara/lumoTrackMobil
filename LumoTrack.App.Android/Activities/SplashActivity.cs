using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Iid;
using LumoTrack.App.Android.Helpers;

namespace LumoTrack.App.Android.Activities
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;

        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
            SetContentView(Resource.Layout.MessageSentLayout);
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        public override void OnBackPressed() { }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            var deviceManager = new DeviceManager();
            if(!deviceManager.IsFirstTime)
            {
                if (!deviceManager.IsDeviceRegister())
                {
                    if (!deviceManager.IsFirebaseToken())
                    {
                        deviceManager.RefreshFirebaseToken();
                    }
                    await deviceManager.UpsertDeviceAsync(0.0, 0.0);
                }
            }

            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}