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
    public class DeviceProxy : ProxyBase,IDeviceProxy
    {
        private const string deviceRegisterURL = "device/register";
        private const string deviceUpdateURL = "device";


        private ISecurityProxy _securityProxy;

        public DeviceProxy(string baseAddress, Credential credential) : base(baseAddress, credential)
        {
        }

        public async Task<ResponseEntity<DeviceDTO>> Register(DeviceDTO deviceDTO)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(deviceDTO);

            using (var httpClient = new TokenSafeHttpClient<DeviceDTO>(_securityProxy, BaseAddress))
            {
                var URL = $"{deviceRegisterURL}";

                var serviceResponse = await httpClient.Post(URL, body);

                return serviceResponse;
            }
        }

        public async Task<ResponseEntity<DeviceDTO>> Update(DeviceDTO deviceDTO)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(deviceDTO);

            using (var httpClient = new TokenSafeHttpClient<DeviceDTO>(_securityProxy, BaseAddress))
            {
                var URL = $"{deviceUpdateURL}";

                var serviceResponse = await httpClient.Post(URL, body);

                return serviceResponse;
            }
        }
    }
}
