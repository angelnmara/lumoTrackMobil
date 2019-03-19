using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Airbnb.Lottie;
using LumoTrack.App.Android.Helpers;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using LumoTrack.ProxyContract;
using Zorbek.Core.Api.Security.Client;
using static Android.Gms.Maps.GoogleMap;
using v4 = Android.Support.V4.App;

namespace LumoTrack.App.Android.Fragments
{
    public class MapFragment : v4.Fragment, IOnMapReadyCallback, IInfoWindowAdapter, IOnInfoWindowClickListener
    {
        public delegate void LoadFragment(string truckId);

        public event LoadFragment loadFrament;

        readonly string[] PermissionsLocation =
          {
                Manifest.Permission.AccessCoarseLocation,
                Manifest.Permission.AccessFineLocation
          };

        const int RequestLocationId = 0;

        const string permission = Manifest.Permission.AccessFineLocation;

        FusedLocationProviderClient fusedLocationProviderClient;

        private List<TrackerVehicleDTO> VehicleLocation { get; set; }

        LumoLocationDTO _userLocation;

        private GoogleMap _googleMap;

        private View _view;

        private BasicProxiesFactory BasicProxiesFactory;

        private LinearLayout _lottieAnimation, _centerMapIcon, _refreshMapIcon;

        private FrameLayout _mapLayout;

        private List<MarkerOptions> MarkerOptionsArray;

        private Bitmap baseBitmap, bitmap;

        private System.Timers.Timer timer;

        private LatLng CurrentPosition { get; set; }

        private void InitTimer()
        {
            timer = new System.Timers.Timer(30000);
            timer.Elapsed += OnTimeElapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            timer.Stop();
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            BasicProxiesFactory = new BasicProxiesFactory();
            InitTimer();
            MarkerOptionsArray = new List<MarkerOptions>();

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.MapLayout, container, false);
            _lottieAnimation = _view.FindViewById<LinearLayout>(Resource.Id.animation_view);
            _lottieAnimation.Visibility = ViewStates.Visible;
            return _view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);
        }


        public override async void OnResume()
        {
            base.OnResume();

            var internetConnectionHelper = new InternetConnectionHelper();

            bool internetConnection = internetConnectionHelper.IsInternetAvailable();

            if (internetConnection)
            {
                try
                {


                    LoadElement();
                    bool locationPermision = false;

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        locationPermision = await CheckLocationPermision();
                    }

                    if (locationPermision)
                    {
                        SetMap();
                    }
                }
                catch (Exception e)
                {
                }
            }
            else
            {
                Activity.RunOnUiThread(() =>
                     Toast.MakeText(Activity, "Es necesario tener acceso a internet para usar la aplicación", ToastLength.Long).Show()
                    );
            }

        }



        public async void SetMap()
        {
            await LoadLocation();

            var mapFragment = SupportMapFragment.NewInstance();

            var resource = Activity.FindViewById<FrameLayout>(Resource.Id.map);

            if (resource != null)
            {
                var r = ChildFragmentManager.BeginTransaction();
                r.Add(Resource.Id.map, mapFragment, "map");
                r.Commit();

                using (var handler = new Handler(Looper.MainLooper))
                    handler.Post(() =>
                    {
                        mapFragment.GetMapAsync(this);
                    });
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _googleMap = googleMap;
            Init();
            SetCamera(CurrentPosition);
        }

        private async Task LoadData()
        {
            System.Diagnostics.Debug.WriteLine("LoadData()");
            await LoadLocation();
            await LoadVehicles();
            await UpdatePositionOnServer();
        }

        private void Init()
        {
            Console.WriteLine("Init");
            Task.WhenAll(LoadData()).ContinueWith((task) =>
            {
                if (_view != null)
                    Activity.RunOnUiThread(() => RenderMap());
            });

            Task.Run(UpdatePositionOnServer);
        }

        private void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Reload timer");
            Init();
        }

        private void CenterMap()
        {
            if (_googleMap != null)
                SetCamera(new LatLng(_userLocation.Latitude, _userLocation.Longitude));
        }

        private void LoadElement()
        {
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(Activity);

            _userLocation = new LumoLocationDTO();

            _refreshMapIcon = _view.FindViewById<LinearLayout>(Resource.Id.refreshIcon);
            _refreshMapIcon.Click += RefreshMap_OnClick;

            _centerMapIcon = _view.FindViewById<LinearLayout>(Resource.Id.mapcenterIcon);
            _centerMapIcon.Click += CenterMap_OnClick;

            _mapLayout = _view.FindViewById<FrameLayout>(Resource.Id.map);
        }

        private void CenterMap_OnClick(object sender, EventArgs e)
        {
            CenterMap();
        }

        private void RefreshMap_OnClick(object sender, EventArgs e)
        {
            RefreshMapOnClick();
        }

        private void RefreshMapOnClick()
        {
            if (_googleMap != null)
            {
                _lottieAnimation.Visibility = ViewStates.Visible;
                _mapLayout.Visibility = ViewStates.Gone;
                Init();
            }
        }

        public override void OnDestroyView()
        {
            timer.Stop();
            timer.Dispose();

            CleanMap();

            base.OnDestroyView();

            GarbageCollector();
        }

        private void RenderMap()
        {
            _googleMap.Clear();

            CleanMap();

            DisplayVehicles();

            LoadPositionMarker();

            _lottieAnimation.Visibility = ViewStates.Gone;
            _mapLayout.Visibility = ViewStates.Visible;

            _googleMap.SetInfoWindowAdapter(this);
            _googleMap.SetOnInfoWindowClickListener(this);

        }

        private void CleanMap()
        {
            foreach (var marker in MarkerOptionsArray)
            {
                marker.Dispose();
            }
            MarkerOptionsArray.Clear();
            if (_googleMap != null)
                _googleMap.Clear();

            GarbageCollector();
        }

        private void GarbageCollector()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        public override void OnPause()
        {
            timer.Stop();

            base.OnPause();
        }

        private void LoadPositionMarker()
        {
            MarkerOptions marker = new MarkerOptions();

            marker.SetPosition(CurrentPosition);

            marker.SetIcon(GetCustomBitmapDescriptor("", Resource.Mipmap.UserPin));

            MarkerOptionsArray.Add(marker);

            _googleMap.AddMarker(marker);
        }

        private void DisplayVehicles()
        {
            if (VehicleLocation != null)
            {
                foreach (var vehicle in VehicleLocation)
                {
                    LatLng vehicleLocation = new LatLng(vehicle.latitude, vehicle.longitude);

                    MarkerOptions markerVehicle = GetMarker(vehicleLocation, Resource.Mipmap.TruckPin, vehicle.trackerId);

                    MarkerOptionsArray.Add(markerVehicle);

                    _googleMap.AddMarker(markerVehicle);

                }
            }

        }

        private async Task LoadVehicles()
        {
            System.Diagnostics.Debug.WriteLine("LoadVehicles()");
            try
            {
                IVehicleProxy vehicleProxy = BasicProxiesFactory.GetVehicleProxy();
                var response = await vehicleProxy.GetVehicleInRange(_userLocation);
                VehicleLocation = response.Data.ToList();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error");
                //bool dialogResult = await AlertDialogHelper.ShowAsync(Activity, "Error", "Ha sucedido en error en la aplicación. Vehiculos", "Reintentar", "Cancelar");

                Activity.RunOnUiThread(() =>
                Toast.MakeText(Activity, "¡Lo sentimos! Las unidades no se encuentran disponibles.", ToastLength.Long).Show()

                );
                Activity.RunOnUiThread(() =>
                RenderMap()
                );
            }

        }

        private void SetCamera(LatLng latlngall)
        {
            CameraUpdate cameraUpdate = GetCamera(latlngall);
            _googleMap.MoveCamera(cameraUpdate);
        }

        private async Task UpdatePositionOnServer()
        {
            System.Diagnostics.Debug.WriteLine("UpdatePositionOnServer start");
            var deviceManager = new DeviceManager();

            await deviceManager.UpsertDeviceAsync(_userLocation.Latitude, _userLocation.Longitude);

            System.Diagnostics.Debug.WriteLine("UpdatePositionOnServer end");
        }

        private async Task LoadLocation()
        {
            Permission permissionId;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                permissionId = Activity.CheckSelfPermission(permission);
            }
            else
            {
                permissionId = (int)Permission.Granted;
            }

            LatLng latlngall = new LatLng(0.0, 0.0);
            if (permissionId == (int)Permission.Granted)
            {
                Location location = await fusedLocationProviderClient.GetLastLocationAsync();

                //latlngall = new LatLng(19.4715, -99.2495);

                //latlngall = new LatLng(20.65524, -105.22744999999999);

                latlngall = new LatLng(location.Latitude, location.Longitude);

                _userLocation.Latitude = latlngall.Latitude;
                _userLocation.Longitude = latlngall.Longitude;
                CurrentPosition = latlngall;
            }
            else
            {
                await LoadLocation();
            }


        }

        private CameraUpdate GetCamera(LatLng location)
        {
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(location);
            builder.Zoom(15);

            CameraPosition cameraPosition = builder.Build();

            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

            return cameraUpdate;
        }

        private MarkerOptions GetMarker(LatLng location, int resourceId, int trackerId)
        {
            MarkerOptions marker = new MarkerOptions();

            marker.SetPosition(location);
            marker.SetTitle(trackerId.ToString());
            marker.SetIcon(GetCustomBitmapDescriptor("", resourceId));

            return marker;
        }

        private BitmapDescriptor GetCustomBitmapDescriptor(string iconName, int resourceID)
        {
            BitmapDescriptor icon;
            using (Paint paint = new Paint(PaintFlags.AntiAlias))
            {
                using (Rect bounds = new Rect())
                {
                    using (baseBitmap = BitmapFactory.DecodeResource(Resources, resourceID))
                    {
                        bitmap = baseBitmap.Copy(Bitmap.Config.Argb8888, true);

                        paint.GetTextBounds(iconName, 0, iconName.Length, bounds);

                        float x = bitmap.Width / 2.0f;
                        float y = (bitmap.Height - bounds.Height()) / 2.0f - bounds.Top;

                        Canvas canvas = new Canvas(bitmap);

                        canvas.DrawText(iconName, x, y, paint);

                        icon = BitmapDescriptorFactory.FromBitmap(bitmap);

                        bitmap.Dispose();
                        baseBitmap.Dispose();
                    }
                    bounds.Dispose();
                }
                paint.Dispose();
            }
            return (icon);
        }

        private async Task<bool> CheckLocationPermision()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {

            }
            var permissionId = Activity.CheckSelfPermission(permission);

            if (permissionId == (int)Permission.Granted)
            {
                return true;
            }

            bool dialogResult = await AlertDialogHelper.ShowAsync(Activity, "Permisos", "Para seguir usando esta applicacion necesitas los permisos de localizacion.", "Activar", "No Activar");

            if (dialogResult)
            {
                var permissionAndroid = Task.Run(() => RequestPermissions(PermissionsLocation, RequestLocationId));

                permissionAndroid.Wait();

                permissionId = Activity.CheckSelfPermission(permission);

                if (permissionId == (int)Permission.Granted)
                {
                    dialogResult = await Task.FromResult<bool>(true);
                }
                else
                {
                    dialogResult = await Task.FromResult<bool>(false);
                }
            }

            return await Task.FromResult<bool>(dialogResult);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            if (grantResults[0] == Permission.Granted)
            {
                SetMap();
            }

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            if (marker.Title != null)
            {
                var vehicleInfo = VehicleLocation.FirstOrDefault(x => x.trackerId.ToString() == marker.Title);


                View view = LayoutInflater.Inflate(Resource.Layout.WindowsInfoAdapterLayout, null, false);

                var label = view.FindViewById<TextView>(Resource.Id.vehicleLabel);
                var model = view.FindViewById<TextView>(Resource.Id.vehicleModel);
                var regNumber = view.FindViewById<TextView>(Resource.Id.vehicleregNumber);

                if (vehicleInfo.label != "")
                    label.Text = vehicleInfo.label;
                else
                    label.Visibility = ViewStates.Gone;

                if (vehicleInfo.model != "")
                    model.Text = vehicleInfo.model;
                else
                    model.Visibility = ViewStates.Gone;

                if (vehicleInfo.regNumber != "")
                    regNumber.Text = vehicleInfo.regNumber;
                else
                    regNumber.Visibility = ViewStates.Gone;

                return view;
            }
            else
            {
                return null;
            }

        }

        public void OnInfoWindowClick(Marker marker)
        {
            //FragmentManager.BeginTransaction().Replace(Resource.Id.fragments_container, new Fragments.CommentFragment(marker.Title)).Commit();

            loadFrament.Invoke(marker.Title);
        }


    }
}