using System;
using System.Collections.Generic;
using System.Text;

namespace LumoTrack.DTO
{
    public class TrackerVehicleDTO
    {
        public int trackerId { get; set; }

        public int vehicleId { get; set; }

        public string label { get; set; }

        public string regNumber { get; set; }

        public string model { get; set; }

        public string type { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }
    }
}
