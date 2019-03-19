using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using LumoTrack.App.iOS.Annotations;
using LumoTrack.App.iOS.ViewControllers;
using MapKit;
using UIKit;

namespace LumoTrack.App.iOS.Delegate
{
    public class MapDelegate : MKMapViewDelegate
    {

        static string annotationId = "MapAnnotation";
        UIView venueView;

        public MapDelegate()
        {
        }

        public override void CalloutAccessoryControlTapped(MKMapView mapView, MKAnnotationView view, UIControl control)
        {
            var responder = (UIResponder)view;

            while (responder != null && !(responder is MapViewController))
            {
                responder = responder.NextResponder;
            }

            var myController = (MapViewController)responder;

            // Wait 3 seconds and then execute the following:
            var tracker = ((TruckAnnotation)view.Annotation).Tracker;
            myController.Truckid = tracker.trackerId;
            myController.PerformSegue("ReportTruckSegue", myController);
            //base.CalloutAccessoryControlTapped(mapView, view, control);
        }



        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
            {
                return null;
            }

            if (annotation is UserAnnotation)
            {
                annotationView = mapView.DequeueReusableAnnotation(annotationId);

                if (annotationView == null)
                    annotationView = new MKAnnotationView((MapKit.IMKAnnotation)annotation, annotationId);

                annotationView.Image = UIImage.FromBundle("UserPinIcon");
                annotationView.CanShowCallout = true;

                UIButton rightAccessory = new UIButton(UIButtonType.DetailDisclosure);
                annotationView.RightCalloutAccessoryView = rightAccessory;
            }

            if (annotation is TruckAnnotation)
            {
                annotationView = mapView.DequeueReusableAnnotation(annotationId);

                if (annotationView == null)
                    annotationView = new MKAnnotationView((MapKit.IMKAnnotation)annotation, annotationId);

                annotationView.Image = UIImage.FromBundle("TruckPinIcon");
                annotationView.CanShowCallout = true;

                UIButton rightAccessory = new UIButton(UIButtonType.DetailDisclosure);
                annotationView.RightCalloutAccessoryView = rightAccessory;
            }

            return annotationView;

        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            // show an image view when the conference annotation view is selected
            if (view.Annotation is TruckAnnotation)
            {
                var basicannotaion = view.Annotation as TruckAnnotation;
                var model = basicannotaion.Tracker;
                //var result=basicannotaion.Tracker;
                if (venueView != null)
                {
                    venueView.RemoveFromSuperview();
                }

                venueView = new UIView();

                venueView.BackgroundColor = UIColor.White;//UIColor.FromPatternImage(UIImage.FromFile("images/truck_1.png"));
                venueView.ContentMode = UIViewContentMode.ScaleAspectFit;

                var margins = venueView.LayoutMarginsGuide;

                UILabel title = new UILabel()
                {
                    BackgroundColor = UIColor.Clear,
                    Text = "Información",
                    Font = UIFont.FromName("Arial", 15),
                    TextAlignment = UITextAlignment.Center,
                    Frame = new CGRect(5, 5, 200, 20)// 
                };
                venueView.AddSubview(title);

                UILabel nameLabel = new UILabel()
                {
                    BackgroundColor = UIColor.Clear,
                    Text = model.label,
                    Font = UIFont.FromName("Arial", 12),
                    TextAlignment = UITextAlignment.Center,
                    Frame = new RectangleF(5, 30, 200, 20),//
                };
                venueView.AddSubview(nameLabel);

                UILabel modelLabel = new UILabel()
                {
                    BackgroundColor = UIColor.Clear,
                    Text = model.model,
                    Font = UIFont.FromName("Arial", 12),
                    TextAlignment = UITextAlignment.Center,
                    Frame = new RectangleF(5, 50, 200, 20)
                };
                venueView.AddSubview(modelLabel);


                UILabel regNumberLabel = new UILabel()
                {
                    BackgroundColor = UIColor.Clear,
                    Text = model.regNumber,
                    Font = UIFont.FromName("Arial", 12),
                    TextAlignment = UITextAlignment.Center,
                    Frame = new RectangleF(5, 60, 200, 30)
                };

                venueView.UserInteractionEnabled = false;

                venueView.AddSubview(regNumberLabel);

                var widthConstraint = NSLayoutConstraint.Create(venueView, NSLayoutAttribute.Width, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 210);
                venueView.AddConstraint(widthConstraint);

                var heightConstraint = NSLayoutConstraint.Create(venueView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, null, NSLayoutAttribute.NoAttribute, 1, 90);
                venueView.AddConstraint(heightConstraint);

                view.DetailCalloutAccessoryView = venueView;
            }
        }

        public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            // remove the image view when the conference annotation is deselected
            if (view.Annotation is TruckAnnotation)
            {

                venueView.RemoveFromSuperview();
                venueView.Dispose();
                venueView = null;
            }
        }
    }
}
