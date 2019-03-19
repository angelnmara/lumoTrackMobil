using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using an= Android;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using LumoTrack.Proxy;
using v4 = Android.Support.V4.App;
using Android.Support.V4.Content;
using System.Drawing;
using Android.Content.Res;

namespace LumoTrack.App.Android.Fragments
{
    public class ConfigurationFragment : v4.Fragment
    {
        private View _view;

        private ISharedPreferences prefs;

        public string UserID, FireBaseToken;

        public BasicProxiesFactory BasicProxiesFactory;

        Switch _pushNotification;
        
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
            _view = inflater.Inflate(Resource.Layout.ConfigurationLayout, container, false);

            return _view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            try
            {
                LoadElements();
            }
            catch (Exception e)
            {
                OnDestroyView();
            }
        }

        private void LoadElements()
        {
            _pushNotification = _view.FindViewById<Switch>(Resource.Id.pushnotificationId);
            _pushNotification.Checked = prefs.GetBoolean("PushNotification", true);

            StateListDrawable thumbStates = new StateListDrawable();
            int white = ContextCompat.GetColor(Activity, Resource.Color.white);
            int mainColor = ContextCompat.GetColor(Activity, Resource.Color.mainColor);
            int defaultColor = ContextCompat.GetColor(Activity, Resource.Color.grey);

            ColorStateList thumbStateColor = new ColorStateList(
                 new int[][]{
                new int[]{an.Resource.Attribute.StateChecked},
                new int[]{an.Resource.Attribute.StateEnabled},
                new int[]{}
        },
        new int[]{
                mainColor,
                defaultColor,
                white
        });


            ColorStateList trackStateColor = new ColorStateList(
                 new int[][]{
                new int[]{an.Resource.Attribute.StateChecked},
                new int[]{an.Resource.Attribute.StateEnabled},
                new int[]{}
        },
        new int[]{
                white,
                defaultColor,
                defaultColor
        });

            _pushNotification.TrackDrawable.SetTintList(trackStateColor);
            _pushNotification.ThumbDrawable.SetTintList(thumbStateColor);
        }

        public override void OnDestroyView()
        {
            var notification = _pushNotification.Checked;

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("PushNotification", notification);
            editor.Apply();

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