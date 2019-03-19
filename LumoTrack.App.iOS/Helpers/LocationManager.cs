using System;
using System.Threading.Tasks;
using CoreLocation;
using UIKit;

namespace LumoTrack.App.iOS.Helpers
{
    public class LocationManager
    {

        protected CLLocationManager locMgr;

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public CLLocationCoordinate2D UserCoordinate { get; set; }

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // works in background
                //locMgr.RequestWhenInUseAuthorization (); // only in foreground
            }

            // iOS 9 requires the following for background location updates
            // By default this is set to false and will not allow background updates
            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }

        }

        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        public CLLocationCoordinate2D GetCoordinate2D()
        {
            return UserCoordinate;
        }

        public void StartLocationUpdates()
        {

            // We need the user's permission for our app to use the GPS in iOS. This is done either by the user accepting
            // the popover when the app is first launched, or by changing the permissions for the app in Settings
            if (CLLocationManager.LocationServicesEnabled)
            {

                //set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = 1;

                LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) => {
                    // fire our custom Location Updated event
                    LocationUpdated(this, new LocationUpdatedEventArgs(e.Locations[e.Locations.Length - 1]));

                };


                LocMgr.StartUpdatingLocation();

            }
        }



        public CLAuthorizationStatus Status()
        {
            return CLLocationManager.Status;
        }

        public bool LocationServiceEnabled()
        {
            return CLLocationManager.LocationServicesEnabled;
        }

        public bool LocationEnabled()
        {
            bool enabled = false;

            var status = CLLocationManager.Status;

            if (status == CLAuthorizationStatus.Authorized || status == CLAuthorizationStatus.AuthorizedAlways ||
                status == CLAuthorizationStatus.AuthorizedWhenInUse)
            {
                enabled = true;
            }

            return enabled;
        }
    }
}
