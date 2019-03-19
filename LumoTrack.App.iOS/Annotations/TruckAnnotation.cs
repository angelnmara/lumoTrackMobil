using System;
using CoreLocation;
using LumoTrack.DTO;
using MapKit;

namespace LumoTrack.App.iOS.Annotations
{
    public class TruckAnnotation : MKAnnotation
    {
        CLLocationCoordinate2D coord;

        public TrackerVehicleDTO Tracker { get; set; }

        public TruckAnnotation(TrackerVehicleDTO trackerVehicleDTO, CLLocationCoordinate2D coord)
        {
            this.coord = coord;
            Tracker = trackerVehicleDTO;
        }

        public override string Title
        {
            get
            {
                return String.Empty;
            }
        }

        public override CLLocationCoordinate2D Coordinate
        {
            get
            {
                return coord;
            }
        }

        
    }
}
