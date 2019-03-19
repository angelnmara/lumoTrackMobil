using Foundation;
using UIKit;
using UserNotifications;
using System;
using Firebase.InstanceID;
using Firebase.Core;
using Firebase.CloudMessaging;
using LumoTrack.App.iOS.ViewControllers;
using LumoTrack.App.iOS.Helpers;

namespace LumoTrack.App.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        // class-level declarations
        private NSUserDefaults prefs;

        public static AppDelegate Self { get; private set; }

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            DeviceManager deviceManager = new DeviceManager();

            prefs = NSUserDefaults.StandardUserDefaults;

            if (!deviceManager.IsDeviceRegister())
            {
                prefs.SetBool(true, "PushNotification");
            }

            Firebase.Core.App.Configure();

            // Register your app for remote notifications.
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {

                // For iOS 10 display notification (sent via APNS)
                //UNUserNotificationCenter.Current.Delegate = this;

                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) => {
                    //.WriteLine(granted);
                });

                UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            //Register for APNs notifications
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            var r=UIApplication.SharedApplication.IsRegisteredForRemoteNotifications;
            Console.WriteLine("Is register? " + r);

           
             
            var token = InstanceId.SharedInstance.Token;
            Console.WriteLine(Messaging.SharedInstance.FcmToken);
           

            Messaging.SharedInstance.Delegate = this;
            //////Connect to FCM (Only used for Foreground notifications)
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            AppDelegate.Self = this;

            return true;
        }

        public override void OnActivated(UIApplication application)
        {
            Console.WriteLine("Foreground ");

            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;

        }


        public override void OnResignActivation(UIApplication application)
        {
            Console.WriteLine("Background ");
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Get current device token
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
            }

            // Get previous device token
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");

            // Has the token changed?
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                //TODO: Put your own logic here to notify your server that the device token has changed/been created!
            }

            // Save new device token 
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
        }

        /// <summary>
        /// Registering for push notifications can fail, for instance, if the device doesn't have network access.
        ///
        /// In this case, this method will be called.
        /// </summary>
        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {

            Console.WriteLine("Failed");
        }

        public void Unregister()
        {
            Console.WriteLine("Unregister");
            UIApplication.SharedApplication.UnregisterForRemoteNotifications();
        }

        public void Register()
        {
            Console.WriteLine("Register");
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        [Export("messaging:didReceiveMessage:")]
        public void DidReceiveMessage(Messaging messaging, RemoteMessage remoteMessage)
        {
            // Do your magic to handle the notification data
            Console.WriteLine("iOS 11 Foreground");
        }
        [Export("userNotificationCenter:didReceiveRemoteNotification:")]
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired till the user taps on the notification launching the application.
            // TODO: Handle data of notification

            // With swizzling disabled you must let Messaging know about the message, for Analytics
            //Messaging.SharedInstance.AppDidReceiveMessage (userInfo);

            // Print full message.
            Console.WriteLine("User Info");
        }


        //Shows local notification and is called when user taps notification
        [Export("userNotificationCenter:DidReceiveRemoteNotification:withCompletionHandler:")]
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {

            Console.WriteLine("Received a notficaiton");

            var content = new UNMutableNotificationContent();

            content.Title = "title";
            content.Subtitle = "subtitle";
            content.Body="Body";


            completionHandler(UIBackgroundFetchResult.NewData);
        }

        //To receive notifications in foreground on iOS 11 devices.
        [Export("userNotificationCenter:willPresent:withCompletionHandler:")]
        public void WillPresent(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Console.WriteLine("Handling iOS 11 foreground notification");
            completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert);
        }

        ////Called when tapping notification
        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            Console.WriteLine("Handling push notificaiton interaction");

            string action = response.Notification.Request.Content.CategoryIdentifier;

            Console.WriteLine("Accion:  " + action);

            UIStoryboard board = UIStoryboard.FromName("Main", null);
            UITabBarController ctrl = (UITabBarController)board.InstantiateViewController("TabBarController");

            switch (action)
            {
                case "Notification":
                    ctrl.SelectedIndex = 1;
                    break;
                case "Inbox":
                    ctrl.SelectedIndex = 2;
                    break;
                default:
                    ctrl.SelectedIndex = 0;
                    break;
            }
            Window.RootViewController.PresentViewController(ctrl, true, null);

            completionHandler();
        }

        //Receive data message on iOS 10 devices.
        public void ApplicationReceivedRemoteMessage(RemoteMessage remoteMessage)
        {
            Console.WriteLine("Handling iOS 10 data message notification");
        }

        //// To receive notifications in foreground on iOS 10 devices.
        [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            Console.WriteLine("Handling foreground notification");
            completionHandler(UNNotificationPresentationOptions.Alert);
        }

        public async void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            Console.WriteLine("Refresh Token :" + fcmToken);

            var deviceManager = new DeviceManager();

            deviceManager.SetFirebaseToken(fcmToken);

            await deviceManager.UpdateDevice(0, 0);

            Messaging.SharedInstance.Subscribe("allios");

            Console.WriteLine("Register device");
        }


    }
}

