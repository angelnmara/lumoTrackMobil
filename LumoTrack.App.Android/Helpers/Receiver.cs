using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LumoTrack.App.Android.Fragments;

namespace LumoTrack.App.Android.Helpers
{
    [BroadcastReceiver]
    public class Receiver : BroadcastReceiver
    {
        private MainActivity mainActivity;

        public Receiver(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public Receiver()
        {

        }

        public override void OnReceive(Context context, Intent intent)
        {

            var action = intent.GetStringExtra("Action");
            var title = intent.GetStringExtra("Title");
            var body = intent.GetStringExtra("Body");

            var fragment = mainActivity.CurrenFragment;

            if (action == "Notification" && fragment is Fragments.NewsFragment)
            {
                mainActivity.LoadNotification();
            }
            else if (action == "Inbox" && fragment is InboxFragment)
            {
                mainActivity.LoadInbox();
            }
            else if (action == "Map" && fragment is Fragments.MapFragment)
            {
                mainActivity.LoadInbox();
            }

            AlertDialog.Builder dialog = new AlertDialog.Builder(mainActivity);
            AlertDialog alert = dialog.Create();
            alert.SetTitle(title);
            alert.SetIcon(Resource.Mipmap.ic_launcher);
            alert.SetMessage(body);

            mainActivity.RunOnUiThread(() =>
                       alert.Show()
                       );


        }
    }
}