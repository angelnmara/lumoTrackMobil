using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using LumoTrack.App.iOS.Helpers;
using LumoTrack.App.iOS.TableViewSource;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class NotificationTableViewController : UIViewController
    {
        NSObject _contentObs;

        public NotificationTableViewController() : base("NotificationViewController", null)
        {
        }

        public NotificationTableViewController(IntPtr handle) : base(handle)
        {

        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.Default;
        }

        public override void ViewDidAppear(bool animated)
        {
            _contentObs = NSNotificationCenter.DefaultCenter.AddObserver(
                            UIApplication.DidBecomeActiveNotification,
                                notification => ViewWillAppear(false));

            base.ViewDidAppear(animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ShowView(false, true);
        }

        private void ShowView(bool showTable, bool showEmptyState)
        {
            NotificationTable.Hidden = showTable;
            EmptyStateView.Hidden = showEmptyState;
        }

        private void BackgroundImage()
        {
            var backgroundImage = UIImage.FromBundle("Background");// UIImage.FromFile("images/1x/People@1x.png");
            var imageView = new UIImageView(backgroundImage);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imageView.Alpha =1f;
            NotificationTable.BackgroundView = imageView;
            NotificationTable.BackgroundColor = UIColor.Clear;
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var internetConnectionHelper = new InternetConnectionHelper();

            var internetConnection = internetConnectionHelper.isInternetAviable();

            if (internetConnection)
            {
                try
                {
                    await Initializer();
                }
                catch (Exception ex)
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "¡Lo sentimos!",
                        Message = "Las notificaciones no se encuentran disponibles."
                    };
                    alert.AddButton("OK");
                    alert.Show();
                }
            }
            else
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = "¡Problemas de conexión!",
                    Message = "Es necesario tener acceso a internet para usar la aplicación"
                };
                alert.AddButton("OK");


                alert.Show();
            }


        }

        private async Task Initializer()
        {
            List<NotificationDTO> notification = await GetNotification();//GetNotificationMockup();

            if (notification.Any())
            {
                ShowView(false, true);

                LoadData(notification);
            }
            else
            {
                ShowView(true, false);
            }

        }

        private void LoadData(List<NotificationDTO> notification)
        {
            BackgroundImage();
            NotificationTable.AllowsSelection = false;
            NotificationTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            NotificationTable.Source = new NotificationTableSource(notification.ToArray());
            NotificationTable.RowHeight = UITableView.AutomaticDimension;
            NotificationTable.EstimatedRowHeight = 280f;
           
            NotificationTable.ReloadData();
        }

        private async Task<List<NotificationDTO>> GetNotification()
        {
            var notificationProxy = new BasicProxiesFactory().GetNotificationProxy();

            var result = await notificationProxy.GetNotification();

            return result.Data.OrderByDescending(x=>x.Id).ToList();
        }

        public override void ViewDidDisappear(bool animated)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this._contentObs);
            base.ViewDidDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

