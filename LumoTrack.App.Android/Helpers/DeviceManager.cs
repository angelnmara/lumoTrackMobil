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
using Firebase.Iid;
using Firebase.Messaging;
using LumoTrack.DTO;
using LumoTrack.Proxy;

namespace LumoTrack.App.Android.Helpers
{
    public class DeviceManager
    {
        private ISharedPreferences prefs;

        public bool IsFirstTime { get; internal set; }

        public DeviceManager()
        {
            prefs = Application.Context.GetSharedPreferences("LumoTrack", FileCreationMode.Private);
            IsFirstTime = prefs.GetBoolean("FirstTime", true);
        }

        public bool IsDeviceRegister()
        {
            lock (prefs)
            {
                bool isregister;
                var deviceID = prefs.GetString("DeviceId", string.Empty);
                if (deviceID == string.Empty)
                    isregister = false;
                else
                    isregister = true;

                return isregister;
            }
        }

        public bool IsFirebaseToken()
        {
            bool isregister;
            //var firebaseID = prefs.GetString("FireBaseToken", string.Empty);
            var token = FirebaseInstanceId.Instance.Token;

            if (token == string.Empty)
                isregister = false;
            else
                isregister = true;

            return isregister;
        }

        public string GetDeviceID()
        {
            return prefs.GetString("DeviceId", string.Empty);
        }

        private void SetDeviceId(string deviceId)
        {
            ISharedPreferencesEditor editor = prefs.Edit();

            editor.PutString("DeviceId", deviceId);

            editor.PutBoolean("FirstTime", false);

            editor.Apply();
        }

        public string GetFirebaseToken()
        {
            return prefs.GetString("FireBaseToken", string.Empty);
        }

        internal void RefreshFirebaseToken()
        {

            Console.WriteLine("Refresh FirebaseToken");
            var firebaseService = new FirebaseIIDService();

            string token = firebaseService.GetToken();
            
            SetFirebaseToken(token);

        }

        public void SetFirebaseToken(string deviceId)
        {
            lock (prefs)
            {
                ISharedPreferencesEditor editor = prefs.Edit();

                editor.PutString("FireBaseToken", deviceId);

                editor.Apply();
            }

        }

        public async System.Threading.Tasks.Task UpsertDeviceAsync(double latitude, double longitude)
        {

            var basicProxiesFactory = new BasicProxiesFactory();
            var deviceProxy = basicProxiesFactory.GetDeviceProxy();

            DeviceDTO deviceDTO = GetDeviceInfo(latitude, longitude);

            var response = await deviceProxy.Update(deviceDTO);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                SetDeviceId(response.Data.DeviceId);
        }

        private DeviceDTO GetDeviceInfo(double latitude, double longitude)
        {
            var deviceDTO = new DeviceDTO();

            deviceDTO.FireBaseToken = GetFirebaseToken();

            deviceDTO.DeviceId = GetDeviceID();

            deviceDTO.Latitude = latitude;

            deviceDTO.Longitude = longitude;

            deviceDTO.TimeZone = "Central Standard Time";

            deviceDTO.devicesType = Common.DeviceTypesEnum.Android;

            return deviceDTO;
        }

    }
}