using System;
using Foundation;
using LumoTrack.App.iOS.Helpers;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class ConfigurationViewController : UIViewController
    {
        private DeviceManager deviceManager;

        private NSUserDefaults prefs;

        public ConfigurationViewController() : base("ConfigurationViewController", null)
        {
            Initializer();
        }

        public ConfigurationViewController(IntPtr handle) : base(handle)
        {
            Initializer();
        }

        private void Initializer()
        {
            deviceManager = new DeviceManager();

            prefs = NSUserDefaults.StandardUserDefaults;


        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.Default;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Styles();

            var switchNotification = prefs.BoolForKey("PushNotification");

            Switch.On = switchNotification;
            

        }

        private void Styles()
        {
            PushNotificationLayout.Layer.CornerRadius = 10;

            // border
            PushNotificationLayout.Layer.BorderWidth = 1.0f;
            PushNotificationLayout.Layer.BorderColor = UIColor.White.CGColor;  // shadow
            PushNotificationLayout.Layer.ShadowColor = UIColor.Gray.CGColor;
            PushNotificationLayout.Layer.ShadowOffset = new CoreGraphics.CGSize(width: 3, height: 3);
            PushNotificationLayout.Layer.ShadowOpacity = 0.7f;
            PushNotificationLayout.Layer.ShadowRadius = 4.0f;
        }

        private void PushNotificationSwitch_PrimaryActionTriggered(object sender, EventArgs e)
        {
            Console.WriteLine("Hi");
        }

        public override void ViewDidDisappear(bool animated)
        {
            var isSetNotification = Switch.On;

            prefs.SetBool(isSetNotification, "PushNotification");

            if(isSetNotification)
            {
                AppDelegate.Self.Register();
            }
            else
            {
                AppDelegate.Self.Unregister();
            }


            base.ViewDidDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

