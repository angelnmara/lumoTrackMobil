using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using CoreLocation;
using Foundation;
using LumoTrack.App.iOS.Annotations;
using LumoTrack.App.iOS.Delegate;
using LumoTrack.App.iOS.Helpers;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using MapKit;
using MetalKit;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class MapViewController : UIViewController
    {
        public static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        MapDelegate mapDelegate;

        public static LocationManager Manager { get; set; }

        volatile private List<TrackerVehicleDTO> trackerList;

        private DeviceManager deviceManager;

        CLLocationCoordinate2D userLocationCoordinate;
        public LumoLocationDTO _userLocation;

        public bool _haveLocation,showalert;

        private List<TruckAnnotation> _MapAnnotation;

        private UserAnnotation _UserAnnotation;

        private System.Timers.Timer timer;

        public int Truckid;

        NSObject _contentObs;

        public MapViewController() : base("MapViewController", null)
        {

        }

        public MapViewController(IntPtr handle) : base(handle)
        {

        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.Default;
        }

        public override void ViewDidLoad()
        {
            Console.WriteLine("ViewDidLoad");
            base.ViewDidLoad();
        }

        public override void ViewDidAppear(bool animated)
        {
            Console.WriteLine("ViewDidAppear");

            _contentObs = NSNotificationCenter.DefaultCenter.AddObserver(
     UIApplication.DidBecomeActiveNotification,
     notification => ViewWillAppear(false));

            base.ViewDidAppear(animated);

        }

        public override void ViewWillAppear(bool animated)
        {
            Console.WriteLine("ViewWillAppear");

            var internetConnectionHelper = new InternetConnectionHelper();

            var internetConnection = internetConnectionHelper.isInternetAviable();



            if (internetConnection)
            {
                try
                {
                    CleanMap();

                    Initializer();

                    InitializeComponent();

                    InitializeMapBegin();

                    if (Manager.LocationEnabled())
                    {
                        timer.Start();
                    }
                    else
                    {
                        if (Manager.Status() != CLAuthorizationStatus.NotDetermined)
                        {
                            InvokeOnMainThread(delegate {



                                UIAlertView alert = new UIAlertView()
                                {
                                    Title = "¡Localización desactivada!",
                                    Message = "Activa Localización en Configuración > Privacidad para permitir que Sigue tu camión determine tu ubicaciónactual"
                                };
                                alert.AddButton("Configuración");
                                alert.AddButton("OK");

                                alert.Clicked += Location_Clicked;

                                alert.Show();
                            });
                        }

                    }

                }
                catch (Exception ex)
                {
                }
            }
            else
            {

                InvokeOnMainThread(delegate {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "¡Problemas de conexión!",
                        Message = "Es necesario tener acceso a internet para usar la aplicación"
                    };
                    alert.AddButton("OK");


                    alert.Show();
                });

            }




        }



        public override void ViewDidDisappear(bool animated)
        {
            timer.Stop();
            NSNotificationCenter.DefaultCenter.RemoveObserver(this._contentObs);
            Console.WriteLine("ViewDidDisappear");
            base.ViewDidDisappear(animated);
        }
        private void Location_Clicked(object sender, UIButtonEventArgs e)
        {
            if (e.ButtonIndex == 0)
            {
                 UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIKit.UIApplication.OpenSettingsUrlString));

            }
        }




        private void Initializer()
        {
            Manager = new LocationManager();

            deviceManager = new DeviceManager();

            _userLocation = new LumoLocationDTO();
            userLocationCoordinate = new CLLocationCoordinate2D();

            _haveLocation = false;

            timer = new System.Timers.Timer(30000);
            timer.Elapsed += Reload;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Stop();
        }

        private void InitializeComponent()
        {
            //ButtonsEvent
            CenterMapButton.TouchDown += CenterButton_Clicked;
            RefreshMapButton.TouchDown += RefreshButton_Clicked;
            //CenterMapButton.Clicked += CenterButton_Clicked;
            //RefreshMapButton.Clicked += RefreshButton_Clicked;

            //LocationManager]
            UIApplication.Notifications.ObserveDidBecomeActive((sender, args) => {
                Manager.LocationUpdated += HandleLocationChanged;
            });

            UIApplication.Notifications.ObserveDidEnterBackground((sender, args) => {
                Manager.LocationUpdated -= HandleLocationChanged;
            });

            Map.MapType = MKMapType.Standard;
            Map.ShowsUserLocation = false;
            Map.ZoomEnabled = true;
            Map.ScrollEnabled = true;

            mapDelegate = new MapDelegate();
            Map.Delegate = mapDelegate;
        }

        private void InitializeMapBegin()
        {

            Manager.StartLocationUpdates();

            CLLocationCoordinate2D mapCenter = new CLLocationCoordinate2D(24.65224,-104.22646);
            MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 10000000, 10000000);
            Map.CenterCoordinate = mapCenter;
            Map.Region = mapRegion;
        }

        private void CenterCamara()
        {
            CLLocationCoordinate2D mapCenter = userLocationCoordinate;
            MKCoordinateRegion mapRegion = MKCoordinateRegion.FromDistance(mapCenter, 1000, 1000);
            Map.CenterCoordinate = mapCenter;
            Map.Region = mapRegion;
        }

        public async void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {

            CLLocation location = e.Location;

            _userLocation.Latitude = location.Coordinate.Latitude;
            _userLocation.Longitude = location.Coordinate.Longitude;

           //_userLocation.Latitude = 20.65524;
           //_userLocation.Longitude =-105.22744999999;
            userLocationCoordinate = location.Coordinate;

            if (!_haveLocation)
            {
                _haveLocation = true;
                await InitializeMap();
            }
                     
        }

        private async Task InitializeMap()
        {
            timer.Start();

            showalert = true;

            await LoadData();

            RenderMap();

            CenterCamara();
        }

        private void RenderMap()
        {
            CleanMap();

            DisplayTruck();

            LoadUserPosition();
        }

        private async void Reload(object sender, ElapsedEventArgs e)
        {
            await LoadData();

            InvokeOnMainThread(delegate {
                RenderMap();
            });

            await UpdatePositionOnServer();
        }

        private async Task LoadData()
        {
            try
            {
                trackerList = await GetVehicleLocation(); //GetTruckLocationMock();
            }
            catch (Exception ex)
            {
                if(showalert)
                {
                    showalert = false;
                    InvokeOnMainThread(delegate {
                        UIAlertView alert = new UIAlertView()
                        {
                            Title = "¡Lo sentimos!",
                            Message = "Las unidades no se encuentran disponibles."
                        };
                        alert.AddButton("OK");
                        alert.Clicked += delegate (object a, UIButtonEventArgs b) {
                            showalert = true;
                        };
                        /*alert.AddButton("custom button 1");
                        alert.AddButton("Cancel");
                        // last button added is the 'cancel' button (index of '2')
                        alert.Clicked += delegate (object a, UIButtonEventArgs b) {
                        };*/
                        alert.Show();
                    });

                }


            }
        }

        private void CleanMap()
        {
            if(_UserAnnotation!=null)
            {
                Map.RemoveAnnotation(_UserAnnotation);
               
            }
            if(_MapAnnotation!=null)
            {
                Map.RemoveAnnotations(_MapAnnotation.ToArray());
            }
        }

        private async Task<List<TrackerVehicleDTO>> GetVehicleLocation()
        {
            var vehicleProxy = new BasicProxiesFactory().GetVehicleProxy();

            var result = await vehicleProxy.GetVehicleInRange(_userLocation);

            return result.Data.ToList();
        }

        private void LoadUserPosition()
        {
            var userannotation = new UserAnnotation(userLocationCoordinate);

            _UserAnnotation = userannotation;

            Map.AddAnnotation(userannotation);
        }

        private void DisplayTruck()
        {
            var mapAnnotation = new List<TruckAnnotation>();

            if(trackerList!=null)
            {
                foreach (var truck in trackerList)
                {
                    var truckLocation = new CLLocationCoordinate2D(truck.latitude, truck.longitude);
                    var annotations = new TruckAnnotation(truck, truckLocation);

                    mapAnnotation.Add(annotations);
                }

                _MapAnnotation = mapAnnotation;

                Map.AddAnnotations(mapAnnotation.ToArray());
            }
        }

        private void CenterButton_Clicked(object sender, EventArgs e)
        {
            CenterCamara();
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            await LoadData();

            RenderMap();
        }

        private async Task UpdatePositionOnServer()
        {
            deviceManager = new DeviceManager();
    
            await deviceManager.UpdateDevice(_userLocation.Latitude, _userLocation.Longitude);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            timer.Stop();

            if (segue.Identifier.Equals("ReportTruckSegue"))
            {

                var viewController = (CommentViewController)segue.DestinationViewController;
                viewController.TruckId = Truckid;
            }
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }


    }
}

