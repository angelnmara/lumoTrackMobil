using Kuktrack.Integrations.Proxy;
using LumoTrack.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;

namespace LumoTrack.ProxyContract
{
   public  interface IVehicleProxy
    {
        Task<ResponseEntity<IEnumerable<TrackerVehicleDTO>>> GetVehicleInRange(LumoLocationDTO locationDTO);
        Task<ResponseEntity<List<VehicleDTO>>> GetVehicle();
        Task<ResponseEntity<IEnumerable<TrackerVehicleDTO>>> GetVehicleTrackers(string hash);
    }
}
