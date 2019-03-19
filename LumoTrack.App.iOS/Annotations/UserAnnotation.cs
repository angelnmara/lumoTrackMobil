using System;
using CoreLocation;
using MapKit;

namespace LumoTrack.App.iOS.Annotations
{
    public class UserAnnotation : MKAnnotation
    {
        string title;
        CLLocationCoordinate2D coord;

        public UserAnnotation(CLLocationCoordinate2D coord)
        {
            this.coord = coord;
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
