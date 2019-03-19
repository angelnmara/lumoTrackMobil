using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Firebase.Messaging;
using static Android.Media.Audiofx.BassBoost;
using System.Linq;
using System.Collections.Generic;
using static Android.App.ActivityManager;
using Java.Lang;
using Android.Widget;

namespace LumoTrack.App.Android.Helpers
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class CustomFirebaseMessagingService : FirebaseMessagingService
    {

        private ISharedPreferences prefs;

        public override void OnMessageReceived(RemoteMessage message)
        {
            prefs = Application.Context.GetSharedPreferences("LumoTrack", FileCreationMode.Private);

            bool notificate = prefs.GetBoolean("PushNotification", true);
            if (notificate)
                Notify(message);

        }

        private void Notify(RemoteMessage message)
        {
            var isInBackground = IsAppOnBackground();

            if (isInBackground)
            {
                BackGroundNotification(message);
            }
            else
            {
                ForeGroundNotification(message);
            }

            //BackGroundNotification(message);


        }

        private void ForeGroundNotification(RemoteMessage message)
        {
            var data = message.Data;
            var title = data["title"];
            var body = data["body"];
            var extra = data["Action"];


            Intent intnt = new Intent("Notification");

            intnt.PutExtra("Action", extra);
            intnt.PutExtra("Title", title);
            intnt.PutExtra("Body", body);


            LocalBroadcastManager.GetInstance(this).SendBroadcast(intnt);

            ////var currentContext = LocalActivityManager;
            //LocalActivityManager.

            //AlertDialog.Builder dialog = new AlertDialog.Builder(currentContext);
            //AlertDialog alert = dialog.Create();
            //alert.SetTitle(title);
            //alert.SetIcon(Resource.Drawable.NotificationIcon);
            //alert.SetMessage(body);



            //currentContext.RunOnUiThread(() =>
            //            //Toast.MakeText(currentContext, "¡Lo sentimos! Las unidades no se encuentran disponibles.", ToastLength.Long).Show()
            //            alert.Show()
            //    );

        }

        private void BackGroundNotification(RemoteMessage message)
        {
            var data = message.Data;
            var title = data["title"];
            var body = data["body"];
            var extra = data["Action"];

            Intent intent = new Intent(this, typeof(MainActivity));

            intent.PutExtra("redirect", extra);


            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.OneShot);

            // Instantiate the builder and set notification elements:
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetSmallIcon(Resource.Mipmap.StatusIconTruck)
                .SetContentTitle(title)
                .SetContentText(body)
                .SetBadgeIconType(Resource.Mipmap.StatusIconTruck)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetVibrate(new long[] { 1000, 1000 })
                .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Mipmap.ic_launcher));

            // Build the notification:


            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.Lollipop)
            {
                builder.SetSmallIcon(Resource.Mipmap.StatusIconTruck);
                builder.SetColor(ContextCompat.GetColor(this, Resource.Color.GreenAlert));
            }
            else
            {
                builder.SetSmallIcon(Resource.Mipmap.StatusIconTruck);
            }


            Notification notification = builder.Build();

            //AutoCancel after user touch
            notification.Flags = NotificationFlags.AutoCancel;

            NotificationManager notificationManager =
                GetSystemService(Context.NotificationService) as NotificationManager;

            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);
        }

        private bool IsAppOnBackground()
        {
            bool isInBackground;

            RunningAppProcessInfo myProcess = new RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(myProcess);
            isInBackground = myProcess.Importance != Importance.Foreground;

            return isInBackground;
        }
    }
}