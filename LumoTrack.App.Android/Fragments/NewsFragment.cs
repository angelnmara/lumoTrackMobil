using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using LumoTrack.App.Android.Adapters;
using LumoTrack.App.Android.Helpers;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using LumoTrack.ProxyContract;
using Zorbek.Core.Api.Security.Client;
using v4 = Android.Support.V4.App;

namespace LumoTrack.App.Android.Fragments
{
    public class NewsFragment : v4.Fragment
    {
        private View _view;

        private ISharedPreferences prefs;

        public string UserID, FireBaseToken;

        public BasicProxiesFactory BasicProxiesFactory;

        private List<DTO.NotificationDTO> _notificationList;

        private ListView _notificationListView;

        public LinearLayout _lottieAnimation, _emptyState;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            prefs = Application.Context.GetSharedPreferences("LumoTrack", FileCreationMode.Private);

            UserID = prefs.GetString("DeviceId", null);

            FireBaseToken = prefs.GetString("FireBaseToken", null);

            BasicProxiesFactory = new BasicProxiesFactory();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.NewsLayout, container, false);

            _lottieAnimation = _view.FindViewById<LinearLayout>(Resource.Id.animation_view);
            _lottieAnimation.Visibility = ViewStates.Visible;

            return _view;
        }


        public override void OnPause()
        {
            base.OnPause();
        }


        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

        }

        public override void OnResume()
        {
            base.OnResume();

            var internetConnectionHelper = new InternetConnectionHelper();

            bool internetConnection = internetConnectionHelper.IsInternetAvailable();

            if (internetConnection)
            {

                try
                {
                    Init();
                }
                catch (Exception e)
                {
                    OnDestroyView();
                }
            }
            else
            {
                Activity.RunOnUiThread(() =>
                     Toast.MakeText(Activity, "Es necesario tener acceso a internet para usar la aplicación", ToastLength.Long).Show()
                    );
            }
        }

        private void Init()
        {
            Task.WhenAll(LoadData()).ContinueWith((task) =>
            {
                if (_view != null)
                    Activity.RunOnUiThread(() => RenderView());
            });
        }

        private void RenderView()
        {
            LoadElements();
        }

        private void GarbageCollector()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            GarbageCollector();
        }

        private void LoadElements()
        {


            if (_notificationList != null && _notificationList.Any())
            {
                _notificationListView = _view.FindViewById<ListView>(Resource.Id.notificationListView);

                var notificationAdapter = new NotificationAdapter(Activity, _notificationList);
                var newsLayout = _view.FindViewById<LinearLayout>(Resource.Id.newsLayoutId);
                newsLayout.Visibility = ViewStates.Visible;
                _lottieAnimation.Visibility = ViewStates.Gone;

                _notificationListView.Adapter = notificationAdapter;
                notificationAdapter.NotifyDataSetChanged();
            }
            else
            {
                _emptyState = _view.FindViewById<LinearLayout>(Resource.Id.empty_state);
                _emptyState.Visibility = ViewStates.Visible;
                _lottieAnimation.Visibility = ViewStates.Gone;
            }
            _lottieAnimation.Dispose();
            GarbageCollector();
        }

        private async Task LoadData()
        {
            try
            {
                INotificationProxy notificationProxy = BasicProxiesFactory.GetNotificationProxy();

                var response = await notificationProxy.GetNotification();

                _notificationList = response.Data.OrderByDescending(x => x.Id).ToList();
            }
            catch (Exception e)
            {
                Activity.RunOnUiThread(() =>
                Toast.MakeText(Activity, "¡Lo sentimos! Las notificaciones no se encuentra disponibles.", ToastLength.Long).Show()
                );
                //bool dialogResult = await AlertDialogHelper.ShowAsync(Activity, "Error", "Ha sucedido en error en la aplicación. Notificaciones", "Reintentar", "Cancelar");
                //Toast.MakeText(Activity, e.Message, ToastLength.Long).Show();

                //if (dialogResult)
                //{
                //    await LoadData();
                //}
            }
        }
    }
}