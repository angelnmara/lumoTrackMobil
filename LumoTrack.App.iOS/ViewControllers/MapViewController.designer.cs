// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LumoTrack.App.iOS.ViewControllers
{
    [Register ("MapViewController")]
    partial class MapViewController
    {
        [Outlet]
        UIKit.UIButton CenterMapButton { get; set; }


        [Outlet]
        MapKit.MKMapView Map { get; set; }


        [Outlet]
        UIKit.UIButton RefreshMapButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CenterMapButton != null) {
                CenterMapButton.Dispose ();
                CenterMapButton = null;
            }

            if (Map != null) {
                Map.Dispose ();
                Map = null;
            }

            if (RefreshMapButton != null) {
                RefreshMapButton.Dispose ();
                RefreshMapButton = null;
            }
        }
    }
}