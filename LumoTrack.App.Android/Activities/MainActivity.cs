using Android;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Android.Gms.Location;
using System.Threading.Tasks;
using Android.Content.PM;
using Android.Locations;

using AlertDialog = Android.App.AlertDialog;
using System;
using LumoTrack.App.Android.Helpers;
using Android.Gms.Common;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using v4 = Android.Support.V4.App;
using Android.Support.Design.Internal;
using Android.Support.V4.Content;
using System.Globalization;
using Android.Content.Res;
using Java.Util;
using Android.Preferences;
using static Android.App.ActivityManager;
using Android.Content;

namespace LumoTrack.App.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppCompatTheme", Icon = "@mipmap/ic_launcher", RoundIcon = "@mipmap/ic_launcher_round", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AppCompatActivity
    {

        BottomNavigationView navigationView;

        v4.FragmentTransaction _fragmentTransaction;

        Fragments.MapFragment _mapFragment;

        Fragments.InboxFragment _inboxFragment;

        public v4.Fragment CurrenFragment;

        //ImageView _refreshMap, _addComment, _centerMap;

        TextView _title;//, _cancelText;

        //LinearLayout _cancelLinearLayout;

        protected Receiver mNotificationReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            SetColorToActionBar();

            navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            LoadElement();

            RemoveShiftMode(navigationView);

            navigationView.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadView();


        }

        private void LoadView()
        {
            var action = Intent.GetStringExtra("Action");

            switch (action)
            {
                case "Notification":
                    LoadNotification();
                    break;
                case "Inbox":
                    LoadInbox();
                    break;
                default:
                    LoadMap();
                    break;
            }


        }

        private void LoadElement()
        {
            navigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);

            _title = FindViewById<TextView>(Resource.Id.titleTextview);

            //_cancelText = FindViewById<TextView>(Resource.Id.cancelText);
            //_cancelText.Text = "< Cancelar";

            //_addComment = FindViewById<ImageView>(Resource.Id.addcomment);
            //_addComment.Click += AddCommentIcon_Click;

            //_cancelLinearLayout = FindViewById<LinearLayout>(Resource.Id.cancelTextView);
            //_cancelLinearLayout.Click += CancelComment_Click;



            mNotificationReceiver = new Receiver(this);

            LocalBroadcastManager.GetInstance(this).RegisterReceiver(mNotificationReceiver, new IntentFilter("Notification"));


        }

        private void CancelComment_Click(object sender, EventArgs e)
        {
            VisibilityIconGone();
            LoadInbox();
        }

        private void AddCommentIcon_Click(object sender, EventArgs e)
        {
            VisibilityIconGone();
            //_cancelLinearLayout.Visibility = ViewStates.Visible;
            CurrenFragment= LoadFragment(new Fragments.CommentFragment());
        }

        private void SetColorToActionBar()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

                int colorInt = ContextCompat.GetColor(this, Resource.Color.white);

                Color color = new Color(colorInt);

                Window.SetStatusBarColor(color);

            }
        }

        private void CommentFragmentChangeFromInbox()
        {
            navigationView.SelectedItemId = Resource.Id.messages_item;

            CurrenFragment = LoadFragment(new Fragments.CommentFragment());
        }

        private void CommenFragmentChange(string truckId)
        {
            navigationView.SelectedItemId = Resource.Id.messages_item;

            CurrenFragment = LoadFragment(new Fragments.CommentFragment(truckId));
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            int itemId = e.Item.ItemId;

            VisibilityIconGone();
            RemoveShiftMode(navigationView);
            switch (itemId)
            {
                case Resource.Id.map_item:
                    LoadMap();
                    break;
                case Resource.Id.notification_item:
                    LoadNotification();
                    break;
                case Resource.Id.messages_item:
                    LoadInbox();
                    break;
                case Resource.Id.configuration_item:
                    LoadConfiguration();

                    break;
                default:
                    break;
            }

            GarbageCollector();
        }


        private void GarbageCollector()
        {
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
        }

        private void VisibilityIconGone()
        {
            //_refreshMap.Visibility = ViewStates.Gone;
            //_addComment.Visibility = ViewStates.Gone;
            //_centerMap.Visibility = ViewStates.Gone;
            //_cancelLinearLayout.Visibility = ViewStates.Invisible;
        }

        public void LoadConfiguration()
        {
            CurrenFragment = LoadFragment(new Fragments.ConfigurationFragment());
        }

        public void LoadInbox()
        {
            navigationView.Menu.GetItem(2).SetChecked(true);
             
            _inboxFragment= (Fragments.InboxFragment)LoadFragment(new Fragments.InboxFragment());

            CurrenFragment = _inboxFragment;

            _inboxFragment.loadFrament += CommentFragmentChangeFromInbox;
            //_refreshMap.Visibility = ViewStates.Invisible;
            //_addComment.Visibility = ViewStates.Visible;
        }



        public void LoadNotification()
        {
            navigationView.Menu.GetItem(1).SetChecked(true);
            CurrenFragment = LoadFragment(new Fragments.NewsFragment());
        }

        public void LoadMap()
        {
            navigationView.Menu.GetItem(0).SetChecked(true);
            _mapFragment = (Fragments.MapFragment)LoadFragment(new Fragments.MapFragment());
            CurrenFragment = _mapFragment;
            _mapFragment.loadFrament += CommenFragmentChange;

            //_refreshMap.Visibility = ViewStates.Visible;
            //_centerMap.Visibility = ViewStates.Visible;
        }

        private v4.Fragment LoadFragment(v4.Fragment fragment)
        {
            try
            {
                _fragmentTransaction = SupportFragmentManager.BeginTransaction();
                _fragmentTransaction.Replace(Resource.Id.fragments_container, fragment);
                //_fragmentTransaction.AddToBackStack(null);
                _fragmentTransaction.Commit();

                return fragment;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        private void RemoveShiftMode(BottomNavigationView navigationView)
        {
            var menuView = (BottomNavigationMenuView)navigationView.GetChildAt(0);
            try
            {
                var shiftingMode = menuView.Class.GetDeclaredField("mShiftingMode");
                shiftingMode.Accessible = true;
                shiftingMode.SetBoolean(menuView, false);
                shiftingMode.Accessible = false;

                for (int i = 0; i < menuView.ChildCount; i++)
                {
                    var item = (BottomNavigationItemView)menuView.GetChildAt(i);
                    item.SetShiftingMode(false);
                    // set checked value, so view will be updated
                    item.SetChecked(item.ItemData.IsChecked);


                }

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine((ex.InnerException ?? ex).Message);
            }
        }

        public override void OnBackPressed()
        {
            if (CurrenFragment is Fragments.CommentFragment)
            {
                if (((Fragments.CommentFragment)CurrenFragment).truckID != null)
                {
                    LoadMap();
                }
                else
                {
                    LoadInbox();
                }
            }
        }

        protected override void OnDestroy()
        {
            LocalBroadcastManager.GetInstance(this).UnregisterReceiver(mNotificationReceiver);

            base.OnDestroy();
        }
    }
}

