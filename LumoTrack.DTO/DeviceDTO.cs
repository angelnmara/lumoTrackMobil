using LumoTrack.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace LumoTrack.DTO
{
    public class DeviceDTO
    {
        public string DeviceId { get; set; }

        public string FireBaseToken { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public DateTime LastNotification { get; set; }

        public string TimeZone { get; set; }

        public DeviceTypesEnum devicesType { get; set; }
    }
}
