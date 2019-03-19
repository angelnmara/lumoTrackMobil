using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using LumoTrack.App.iOS.Helpers;
using LumoTrack.App.iOS.TableViewSource;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class CommentTableViewController : UIViewController
    {
        private DeviceManager deviceManager;

        NSObject _contentObs;

        public CommentTableViewController() : base("CommentTableViewController", null)
        {
        }

        public CommentTableViewController(IntPtr handle) : base(handle)
        {
            deviceManager = new DeviceManager();
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
            ShowView(false, true);
            base.ViewDidLoad();
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
                        Message = "El envío de comentarios no se encuentran disponibles."
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

        public override void ViewDidDisappear(bool animated)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this._contentObs);
            base.ViewDidDisappear(animated);
        }

        private void BackgroundImage()
        {
            var backgroundImage = UIImage.FromBundle("Background");
            var imageView = new UIImageView(backgroundImage);
            imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
            imageView.Alpha = 1f;

            MessageTableView.BackgroundView = imageView;
            MessageTableView.BackgroundColor = UIColor.Clear;
        }

        private async Task Initializer()
        {
            List<InboxDTO> inboxList = await GetInbox();//GetInboxMockup(); 

            if (inboxList!= null && inboxList.Any())
            {
                ShowView(false, true);
                BackgroundImage();
                MessageTableView.AllowsSelection = false;
                MessageTableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

                MessageTableView.Source = new InboxTableSource(inboxList.ToArray());
                MessageTableView.RowHeight = UITableView.AutomaticDimension;
                MessageTableView.EstimatedRowHeight = 450f;
                MessageTableView.ReloadData();


            }
            else
            {
                ShowView(true, false);
            }
        }

        private async Task<List<InboxDTO>> GetInbox()
        {
            var inboxProxy = new BasicProxiesFactory().GetInboxProxy();
            //var result = await inboxProxy.GetNotification("d8ed05b8-949b-4bad-a983-3020227a6193");
            var result = await inboxProxy.GetNotification(deviceManager.GetDeviceID());

            return result.Data.ToList();
        }


        private void ShowView(bool showTable, bool showEmptyState)
        {
            MessageTableView.Hidden = showTable;
            EmptyState.Hidden = showEmptyState;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}

