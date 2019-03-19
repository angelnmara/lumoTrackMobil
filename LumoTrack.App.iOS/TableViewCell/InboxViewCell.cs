using System;
using System.Globalization;
using Foundation;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.TableViewCell
{
    public partial class InboxViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("InboxViewCell");
        public static readonly UINib Nib;

        static InboxViewCell()
        {
            Nib = UINib.FromName("InboxViewCell", NSBundle.MainBundle);
        }

        protected InboxViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public void SetMessage(string message)
        {
            Text.Text = message+ "I have a basic say hello button on a tabbed layout in the third view (in total I have 3 vi";
            ResponseTitle.Text = "Respuesta de Lumo";
            ResponseDate.Text = "20 de noviembre";
            ResponseText.Text = "I have a basic say hello button on a tabbed layout in the third view (in total I have 3 virecontrollers:FirstViewController, SecondViewController,ThirdViewController). In the third I put a button anf from xcode ui builder I have defined outlet and action for the button (action: sayhello)";

            //ImportantView.RemoveFromSuperview();


        }

        internal void SetView(InboxDTO item)
        {
            CultureInfo mx = new CultureInfo("es-MX");

            Date.Text = item.CreationDate.ToString("dd MMMM yyyy",mx);
            Text.Text = item.Message;

            if(item.ResponseDate==null)
            {
                /*ResponseTitle.Hidden=true;
                ResponseDate.Hidden=true;
                ResponseTitle.RemoveFromSuperview();
                ResponseText.RemoveFromSuperview();
                ResponseDate.RemoveFromSuperview();
                               
                               
                */
                ResponseDate.Text = "";
                ResponseText.Text = "";
                ResponseTitle.Text = "";
            }
            else
            {
                /*
                ResponseTitle.Hidden = false;
                ResponseDate.Hidden = false;
                ResponseText.Hidden = false;
                */
                ResponseDate.Text = item.ResponseDate?.ToString("dd MMMM yyyy", mx);
                ResponseText.Text = item.Response;
                }

                            ResponseDate.Text = item.ResponseDate?.ToString("dd MMMM yyyy", mx);
                ResponseText.Text = item.Response;
            // corner radius
            ContentLayout.Layer.CornerRadius = 10;

            // border
            ContentLayout.Layer.BorderWidth = 0.8f;
            ContentLayout.Layer.BorderColor = UIColor.White.CGColor;

            // shadow
            ContentLayout.Layer.ShadowColor = UIColor.Gray.CGColor;
            ContentLayout.Layer.ShadowOffset = new CoreGraphics.CGSize(width: 3, height: 3);
            ContentLayout.Layer.ShadowOpacity = 0.7f;
            ContentLayout.Layer.ShadowRadius = 4.0f;
        }
    }
}
