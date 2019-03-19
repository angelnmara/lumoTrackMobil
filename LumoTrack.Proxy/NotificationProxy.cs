using LumoTrack.DTO;
using LumoTrack.ProxyContract;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;
using Zorbek.Core.Api.Security.Client;
using Zorbek.Core.Api.Security.Client.Helper;

namespace LumoTrack.Proxy
{
    public class NotificationProxy : ProxyBase, INotificationProxy
    {
        private const string NotificationURL = "Notification";

        private ISecurityProxy _securityProxy;

        public NotificationProxy(string baseAddress, Credential credential) : base(baseAddress, credential)
        {
        }

        public async Task<ResponseEntity<List<NotificationDTO>>> GetNotification()
        {
            using (var httpClient = new TokenSafeHttpClient<List<NotificationDTO>>(_securityProxy, BaseAddress))
            {
                var URL = $"{NotificationURL}";

                var serviceResponse = await httpClient.Get(URL);

                return serviceResponse;
            }
        }
    }
}
