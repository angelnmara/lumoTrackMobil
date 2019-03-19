using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using v4 = Android.Support.V4.App;

namespace LumoTrack.App.Android.Fragments
{
    public class MessageSend : v4.Fragment
    {
        private LinearLayout _acceptButton;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.MessageSentLayout, container, false);

            _acceptButton = view.FindViewById<LinearLayout>(Resource.Id.messageSendLinearLayout);
            _acceptButton.Click += AcceptButtonOnClick;

            return view;
        }

        private void AcceptButtonOnClick(object sender, EventArgs e)
        {
            var mainactivity = (MainActivity)Activity;
            mainactivity.LoadInbox();
        }

        public override void OnPause()
        {
            base.OnPause();
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