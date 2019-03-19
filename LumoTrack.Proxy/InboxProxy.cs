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
    public class InboxProxy : ProxyBase, IInboxProxy
    {
        private const string InboxURL = "Inbox";

        private const string InboxCreateURL = "Inbox/create";

        private ISecurityProxy _securityProxy;

        public InboxProxy(string baseAddress, Credential credential) : base(baseAddress, credential)
        {
        }

        public async Task<ResponseEntity<InboxDTO>> Create(InboxDTO inboxDTO)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(inboxDTO);

            using (var httpClient = new TokenSafeHttpClient<InboxDTO>(_securityProxy, BaseAddress))
            {
                var URL = $"{InboxCreateURL}";

                var serviceResponse = await httpClient.Post(URL, body);

                return serviceResponse;
            }
        }

        public async Task<ResponseEntity<IEnumerable<InboxDTO>>> GetNotification(string userId)
        {
            List<Tuple<string, string>> headers = new List<Tuple<string, string>>();

            Tuple<string, string> userheader = new Tuple<string, string>("UserID",userId);

            headers.Add(userheader);

            using (var httpClient = new TokenSafeHttpClient<IEnumerable<InboxDTO>>(_securityProxy, BaseAddress, headers))
            {
                var URL = $"{InboxURL}";

                var serviceResponse = await httpClient.Post(URL, userId);

                return serviceResponse;
            }
        }

        public async Task<ResponseEntity<string>> GetNotification1(string userId)
        {
            using (var httpClient = new TokenSafeHttpClient<string>(_securityProxy, BaseAddress))
            {
                var URL = $"{InboxURL}";

                var serviceResponse = await httpClient.Post(URL, userId);

                return serviceResponse;
            }
        }
    }
}
