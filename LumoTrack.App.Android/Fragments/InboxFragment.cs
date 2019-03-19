using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LumoTrack.App.Android.Adapters;
using LumoTrack.App.Android.Helpers;
using LumoTrack.Proxy;
using LumoTrack.ProxyContract;
using v4 = Android.Support.V4.App;

namespace LumoTrack.App.Android.Fragments
{
    public class InboxFragment : v4.Fragment
    {
        public delegate void LoadFragment();

        public event LoadFragment loadFrament;

        private View _view;

        private string UserID, FireBaseToken;

        private BasicProxiesFactory BasicProxiesFactory;

        private List<DTO.InboxDTO> _inboxList;

        private ListView _inboxListView;

        private LinearLayout _lottieAnimation, _emptyState, _addCommentIcon;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            BasicProxiesFactory = new BasicProxiesFactory();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.InboxLayout, container, false);

            _lottieAnimation = _view.FindViewById<LinearLayout>(Resource.Id.animation_view);
            _lottieAnimation.Visibility = ViewStates.Visible;

            _addCommentIcon = _view.FindViewById<LinearLayout>(Resource.Id.addCommentIcon);
            _addCommentIcon.Click += AddComment_OnClick;

            return _view;
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

        private void LoadElements()
        {


            if (_inboxList != null && _inboxList.Any())
            {
                _inboxListView = _view.FindViewById<ListView>(Resource.Id.inboxListView);

                var notificationAdapter = new InboxAdapter(Activity, _inboxList);

                var inboxLayout = _view.FindViewById<LinearLayout>(Resource.Id.inboxListLayout);
                inboxLayout.Visibility = ViewStates.Visible;
                _lottieAnimation.Visibility = ViewStates.Gone;

                _inboxListView.Adapter = notificationAdapter;
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

        private void AddComment_OnClick(object sender, EventArgs e)
        {
            //FragmentManager.BeginTransaction().Replace(Resource.Id.fragments_container, new Fragments.CommentFragment()).Commit();
            loadFrament.Invoke();
        }

        private async Task LoadData()
        {
            try
            {
                IInboxProxy inboxProxy = BasicProxiesFactory.GetInboxProxy();

                var deviceManager = new DeviceManager();

                if (!deviceManager.IsDeviceRegister())
                {
                    await deviceManager.UpsertDeviceAsync(0.0, 0.0);
                }

                var response = await inboxProxy.GetNotification(deviceManager.GetDeviceID());

                _inboxList = response.Data.OrderByDescending(x => x.Id).ToList();
            }
            catch (Exception e)
            {
                Activity.RunOnUiThread(() =>
                        Toast.MakeText(Activity, "¡Lo sentimos! Los comentarios no se encuentra disponibles.", ToastLength.Long).Show()
                    );
            }
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            GarbageCollector();
        }

        private void GarbageCollector()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }
    }
}