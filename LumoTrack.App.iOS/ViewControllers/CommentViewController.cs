using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using LumoTrack.App.iOS.Helpers;
using LumoTrack.App.iOS.PickerView;
using LumoTrack.DTO;
using LumoTrack.Proxy;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class CommentViewController : UIViewController
    {
        private string placeholder = "Escriba su comentario";

        private DeviceManager deviceManager;

        public int TruckId;

        NSObject _contentObs;

        public CommentViewController() : base("CommentViewController", null)
        {

        }
        public CommentViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidAppear(bool animated)
        {
            _contentObs = NSNotificationCenter.DefaultCenter.AddObserver(
                            UIApplication.DidBecomeActiveNotification,
                                notification => ViewWillAppear(false));

            base.ViewDidAppear(animated);
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
                    SendButton.Enabled = false;

                    await Initializer();

                    KeyboardAction();

                    StyleView();

                    List<VehicleDTO> trucks = await GetVehicles();

                    if (trucks != null)
                        LoadData(trucks);
                    else
                        throw new Exception();

                    SendButton.Enabled = true;
                }
                catch (Exception ex)
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "¡Lo sentimos!",
                        Message = "Las unidades no se encuentran disponibles."
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

        private void LoadData(List<VehicleDTO> trucks)
        {
            trucks.Insert(0, new VehicleDTO { Id = 0, TruckName = "No aplica/No conozco el dato" });

            TruckSpinner.Model = new TruckPicker(trucks);

            ReportSpinner.Model = new ReportPicker();

            TruckSpinner.ShowSelectionIndicator = true;

            ReportSpinner.ShowSelectionIndicator = true;

            if (TruckId != 0)
            {
                var truck = trucks.FirstOrDefault(x => x.Id == TruckId);
                var index = trucks.IndexOf(truck);
                TruckSpinner.Select(index, 0, true);
            }
            else
            {
                TruckSpinner.Select(100, 0, true);
                TruckSpinner.Select(0, 0, true);
            }


            Console.WriteLine("Load Picker");
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            bool canIsendInbox = FacadeCondition();

            if (!canIsendInbox)
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = "¡Atención!",
                    Message = "Para enviar el mensaje con éxito, es necesario llenar todos los campos."
                };
                alert.AddButton("OK");
                alert.Show();
            }
            return canIsendInbox;
        }

        private void StyleView()
        {
            ContentView.Layer.CornerRadius = 10;

            // border
            ContentView.Layer.BorderWidth = 1.0f;
            ContentView.Layer.BorderColor = UIColor.White.CGColor;  // shadow
            ContentView.Layer.ShadowColor = UIColor.Gray.CGColor;
            ContentView.Layer.ShadowOffset = new CoreGraphics.CGSize(width: 3, height: 3);
            ContentView.Layer.ShadowOpacity = 0.7f;
            ContentView.Layer.ShadowRadius = 4.0f;

            MessageTextView.Text = placeholder;

            MessageTextView.Layer.BorderWidth = 0.5f;
            MessageTextView.Layer.BorderColor = UIColor.Gray.CGColor;

            MessageTextView.ShouldBeginEditing += MessageTextView_ShouldBeginEditing;

            MessageTextView.ShouldEndEditing += MessageTextView_ShouldEndEditing;
        }

        private async Task Initializer()
        {
            deviceManager = new DeviceManager();

            if (!deviceManager.IsDeviceRegister())
            {
                await deviceManager.UpdateDevice(0.0, 0.0);
            }
        }

        private void KeyboardAction()
        {
            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardDownNotification);
        }

        private void KeyBoardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            CGRect r = UIKeyboard.BoundsFromNotification(notification);

            CGRect viewFrame = View.Bounds;

            // get new height of the content view
            nfloat currentViewHeight = viewFrame.Height - r.Height;

            // update scrollViewFrame
            Scrollview.Frame = new CGRect(Scrollview.Frame.X, Scrollview.Frame.Y, Scrollview.Frame.Width, currentViewHeight);

            CenterViewInScroll(MessageTextView, Scrollview, (float)r.Height);

        }

        private void KeyBoardDownNotification(NSNotification notification)
        {
            CGRect viewFrame = View.Bounds;

            Scrollview.Frame = new CGRect(Scrollview.Frame.X, Scrollview.Frame.Y,
                                Scrollview.Frame.Width, viewFrame.Height);

        }

        private void RestoreScrollPosition(UIScrollView scrollView)
        {
            scrollView.ContentInset = UIEdgeInsets.Zero;
            scrollView.ScrollIndicatorInsets = UIEdgeInsets.Zero;
        }

        private void CenterViewInScroll(UIView viewToCenter, UIScrollView scrollView, float keyboardHeight)
        {
            bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
            var spaceAboveKeyboard = (landscape ? scrollView.Frame.Width : scrollView.Frame.Height) - keyboardHeight;

            // Move the active field to the center of the available space

            var offset = MessageTextView.Frame.Y - (spaceAboveKeyboard - MessageTextView.Frame.Height) / 2;
            scrollView.ContentOffset = new PointF(0, (float)offset);
        }

        private async Task<List<VehicleDTO>> GetVehicles()
        {
            var vehicleProxy = new BasicProxiesFactory().GetVehicleProxy();

            var result = await vehicleProxy.GetVehicle();

            return result.Data;
        }

        private bool MessageTextView_ShouldEndEditing(UITextView textView)
        {
            if (string.IsNullOrEmpty(MessageTextView.Text))
                MessageTextView.Text = placeholder;

            return true;
        }

        private bool MessageTextView_ShouldBeginEditing(UITextView textView)
        {
            if (MessageTextView.Text == placeholder)
                MessageTextView.Text = string.Empty;

            return true;
        }

        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.Default;
        }

        public override async void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            try
            {
                SendButton.Enabled = false;
                bool canIsendInbox = FacadeCondition();

                if (canIsendInbox)
                {
                    await SendData();
                    SendButton.Enabled = true;
                    base.PrepareForSegue(segue, sender);
                }

                else
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "¡Atención!",
                        Message = "Para enviar el mensaje con éxito, es necesario llenar todos los campos."
                    };
                    alert.AddButton("OK");
                    alert.Show();
                }

            }
            catch (Exception ex)
            {
                UIAlertView alert = new UIAlertView()
                {
                    Title = "¡Lo sentimos!",
                    Message = "El envio de comentarios no se encuentra disponibles."
                };
                alert.AddButton("OK");
                alert.Show();
            }


        }

        private async Task SendData()
        {
            var inboxProxy = new BasicProxiesFactory().GetInboxProxy();

            InboxDTO inboxDTO = GetInbox();

            var result = await inboxProxy.Create(inboxDTO);
        }

        private InboxDTO GetInbox()
        {
            var inboxDTO = new InboxDTO();


            var vehicle = ((TruckPicker)TruckSpinner.Model).SelectedItem;

            inboxDTO.Message = MessageTextView.Text;
            inboxDTO.CreationDate = DateTime.Now;
            inboxDTO.ReportType = ((ReportPicker)ReportSpinner.Model).SelectedItem;
            inboxDTO.TruckId = vehicle.Id.ToString();
            inboxDTO.TruckName = vehicle.TruckName;
            inboxDTO.UserId = deviceManager.GetDeviceID();

            return inboxDTO;
        }

        private bool FacadeCondition()
        {
            bool response = true;


            if (MessageTextView.Text == placeholder || MessageTextView.Text=="")
            {
                response = false;
            }

            if (ReportSpinner.Model == null)
                response = false;

            if (TruckSpinner.Model == null)
                response = false;

            return response;
        }

        public override void ViewDidDisappear(bool animated)
        {
            NSNotificationCenter.DefaultCenter.RemoveObserver(this._contentObs);
            base.ViewDidDisappear(animated);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}

