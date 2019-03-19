using LumoTrack.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using zorbek.essentials.utilities.Entities;

namespace LumoTrack.ProxyContract
{
    public interface IDeviceProxy
    {
        Task<ResponseEntity<DeviceDTO>> Register(DeviceDTO deviceDTO);
        Task<ResponseEntity<DeviceDTO>> Update(DeviceDTO deviceDTO);
    }
}
