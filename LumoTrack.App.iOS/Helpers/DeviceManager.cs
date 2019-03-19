using System;
using System.Threading.Tasks;
using Foundation;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using zorbek.essentials.utilities.Entities;

namespace LumoTrack.App.iOS.Helpers
{
    public class DeviceManager
    {
        private NSUserDefaults prefs;

        public DeviceManager()
        {
            prefs = NSUserDefaults.StandardUserDefaults;
        }

        public bool IsDeviceRegister()
        {
            bool isRegister;

            var deviceID = prefs.StringForKey("DeviceId");

            if (deviceID == string.Empty || deviceID == null)
                isRegister = false;
            else
                isRegister = true;

            return isRegister;

        }

        public string GetDeviceID()
        {
            string response;

            response = prefs.StringForKey("DeviceId");

            if (response == null)
                response = "";

            return response;
        }

        public string GetFirebaseToken()
        {
            string response;

            response = prefs.StringForKey("FirebaseToken");

            if (response == null)
                response = "";

            return response;
        }

        private void SetDeviceId(string deviceId)
        {
            prefs.SetString(deviceId, "DeviceId");
        }

        public void SetFirebaseToken(string firebaseToken)
        {
            prefs.SetString(firebaseToken, "FirebaseToken");
        }

        public async Task<ResponseEntity<DeviceDTO>> UpdateDevice(double latitude,double longitude)
        {
            try
            {
                var basicProxiesFactory = new BasicProxiesFactory();
                var deviceProxy = basicProxiesFactory.GetDeviceProxy();

                DeviceDTO deviceDTO = GetDeviceInfo(latitude, longitude);

                var response = await deviceProxy.Update(deviceDTO);

                SetDeviceId(response.Data.DeviceId);

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }

        }

        private DeviceDTO GetDeviceInfo(double latitude, double longitude)
        {
            var deviceDTO = new DeviceDTO();

            deviceDTO.FireBaseToken = GetFirebaseToken();

            deviceDTO.DeviceId = GetDeviceID();

            deviceDTO.Latitude = latitude;

            deviceDTO.Longitude = longitude;

            deviceDTO.devicesType = Common.DeviceTypesEnum.IOS;
             
            string timezone = NSTimeZone.SystemTimeZone.ToString();

            deviceDTO.TimeZone = TimeZoneHelper.OlsonTimeZoneToTimeZoneInfo(timezone);

            return deviceDTO;
        }
    }
}
