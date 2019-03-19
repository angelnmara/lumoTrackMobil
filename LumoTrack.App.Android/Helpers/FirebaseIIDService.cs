using sym = System;
using Android.App;
using Firebase.Iid;
using Android.Util;
using Firebase.Messaging;
using Android.Content;
using LumoTrack.Proxy;
using LumoTrack.DTO;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;
using Android.Icu.Util;
using System.Threading;
using System.Globalization;

namespace LumoTrack.App.Android.Helpers
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class FirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "FirebaseIIDService";

        private ISharedPreferences prefs;
        
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;
            
            Log.Debug(TAG, "Refreshed token: " + refreshedToken);

            SendRegistrationToServer(refreshedToken);

            Log.Debug(TAG, "Finish");

            FirebaseMessaging.Instance.SubscribeToTopic("allandroid");

            Log.Debug(TAG, "Subscribe to topic");
        }
        
        void SendRegistrationToServer(string token)
        {
            DeviceManager deviceManager = new DeviceManager();

            deviceManager.SetFirebaseToken(token);

            deviceManager.UpsertDeviceAsync(0.0, 0.0).Wait();

            Log.Debug(TAG, token);
        }

        public string GetToken()
        {
            var token = FirebaseInstanceId.Instance.GetToken("363782701847", FirebaseMessaging.InstanceIdScope);

            FirebaseMessaging.Instance.SubscribeToTopic("allandroid");

            return token;
        }
    }
}