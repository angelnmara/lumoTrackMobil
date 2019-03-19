using LumoTrack.ProxyContract;
using System;
using System.Collections.Generic;
using System.Text;
using Zorbek.Core.Api.Security.Client;
using Zorbek.Core.Api.Security.Client.Helper;

namespace LumoTrack.Proxy
{
    public class BasicProxiesFactory: IBasicProxiesFactory
    {
        //private const string SERVICE_URL = "http://192.168.1.65/Lumo/";

        //private const string SERVICE_URL = "http://192.168.0.13/Lumo/";

        private const string SERVICE_URL = "http://104.209.33.188/lumotrack/api/";

        public IVehicleProxy GetVehicleProxy()
        {
            var credential = CredentialHelper.GetInstance();

            var vehicleProxy = new VehicleProxy(SERVICE_URL, credential);

            return vehicleProxy;
        }

        public IInboxProxy GetInboxProxy()
        {
            var credential = CredentialHelper.GetInstance();

            var inboxProxy = new InboxProxy(SERVICE_URL, credential);

            return inboxProxy;
        }

        public INotificationProxy GetNotificationProxy()
        {
            var credential = CredentialHelper.GetInstance();

            var notificationProxy = new NotificationProxy(SERVICE_URL, credential);

            return notificationProxy;
        }

        public IDeviceProxy GetDeviceProxy()
        {
            var credential = CredentialHelper.GetInstance();

            var deviceProxy = new DeviceProxy(SERVICE_URL, credential);

            return deviceProxy;
        }
    }
}
