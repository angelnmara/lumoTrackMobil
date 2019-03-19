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
    public class VehicleProxy : ProxyBase, IVehicleProxy
    {
        private const string VehicleURL = "vehicles";

        private const string VehicleName = "vehicles/trucks";

        private ISecurityProxy _securityProxy;

        public VehicleProxy(string baseAddress, Credential credential) : base(baseAddress, credential)
        {
            //_securityProxy = securityProxy;
        }

        public async Task<ResponseEntity<List<VehicleDTO>>> GetVehicle()
        {
            using (var httpClient = new TokenSafeHttpClient<List<VehicleDTO>>(_securityProxy, BaseAddress))
            {
                var URL = $"{VehicleName}";

                var serviceResponse = await httpClient.Post(URL,"");

                return serviceResponse;
            }
        }

        public async Task<ResponseEntity<IEnumerable<TrackerVehicleDTO>>> GetVehicleInRange(LumoLocationDTO locationDTO)
        {
            string body = Newtonsoft.Json.JsonConvert.SerializeObject(locationDTO);

            using (var httpClient = new TokenSafeHttpClient<IEnumerable<TrackerVehicleDTO>>(_securityProxy, BaseAddress))
            {
                var URL = $"{VehicleURL}";

                var serviceResponse = await httpClient.Post(URL, body);

                return serviceResponse;
            }
        }

        public async Task<ResponseEntity<IEnumerable<TrackerVehicleDTO>>> GetVehicleTrackers(string hash)
        {
            var baseAddress = "http://api.kuktrackpro.com/Lumo/TrackerVehicle/";

            using (var httpClient = new TokenSafeHttpClient<IEnumerable<TrackerVehicleDTO>>(_securityProxy, baseAddress))
            {
                var URL = $"List?hash={hash}";

                var serviceResponse = await httpClient.Get(URL);

                return serviceResponse;
            }
        }
    }
}
