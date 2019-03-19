using System;
using System.Globalization;
using Foundation;
using LumoTrack.DTO;
using UIKit;

namespace LumoTrack.App.iOS.TableViewCell
{
    public partial class NotificationViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("NotificationViewCell");
        public static readonly UINib Nib;

        static NotificationViewCell()
        {
            Nib = UINib.FromName("NotificationViewCell", NSBundle.MainBundle);
        }

        protected NotificationViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal void SetView(NotificationDTO item)
        {
            CultureInfo mx = new CultureInfo("es-MX");

            Title.Text = item.Title;
            Date.Text = $"{item.NotificationDate.ToString("dd MMMM yyyy", mx)} - {item.ExpiryDate.ToString("dd MMMM yyyy", mx) }";
             
            Text.Text = item.Message;

            if (item.Important)
                ImportantView.Hidden = false;
            else
                ImportantView.Hidden = true;


            // corner radius
            ContentLayout.Layer.CornerRadius = 10;

            // border
            ContentLayout.Layer.BorderWidth = 1.0f;
            ContentLayout.Layer.BorderColor = UIColor.White.CGColor;

            // shadow
            ContentLayout.Layer.ShadowColor = UIColor.Gray.CGColor;
            ContentLayout.Layer.ShadowOffset = new CoreGraphics.CGSize(width: 3, height: 3);
            ContentLayout.Layer.ShadowOpacity = 0.7f;
            ContentLayout.Layer.ShadowRadius = 4.0f;
        }
    }
}
