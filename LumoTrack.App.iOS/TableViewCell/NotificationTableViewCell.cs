using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LumoTrack.App.iOS.TableViewCell
{
    public class NotificationTableViewCell :UITableViewCell
    {
        UILabel headingLabel, subheadingLabel;
        UIImageView imageView;

        public NotificationTableViewCell(string cellId) : base(UITableViewCellStyle.Default, cellId)
        {
            var notificationView = new UIView();

            notificationView.BackgroundColor = UIColor.Gray;

            var width = ContentView.Bounds.Width;

            var yPosition = 0.0f;

            UILabel important = new UILabel()
            {
                BackgroundColor = UIColor.Red,
                Text = "Importante",
                Font = UIFont.FromName("Arial", 15),
                TextColor=UIColor.White,
                TextAlignment = UITextAlignment.Center,
                Frame = new CGRect(0, yPosition, width/3, 20)
            };

            yPosition += 40f;

            UILabel Title = new UILabel()
            {
                BackgroundColor = UIColor.Clear,
                Text = "Felices Fiestas",
                Font = UIFont.FromName("Arial", 15),
                TextAlignment = UITextAlignment.Left,
                Frame = new CGRect(30, yPosition, ContentView.Bounds.Width, 20)
            };

            yPosition += 25f;

            UILabel Date = new UILabel()
            {
                BackgroundColor = UIColor.Clear,
                Text = "17 de diciembre",
                Font = UIFont.FromName("Arial", 15),
                TextAlignment = UITextAlignment.Left,
                Frame = new CGRect(30, yPosition, ContentView.Bounds.Width, 20)
            };

            yPosition += 40;

            UILabel text = new UILabel()
            {
                BackgroundColor = UIColor.Clear,
                Text = "Por motivos blablablabla",
                Font = UIFont.FromName("Arial", 15),
                LineBreakMode=UILineBreakMode.WordWrap,
                Lines=0,
                TextAlignment = UITextAlignment.Left,
                Frame = new CGRect(30, yPosition, ContentView.Bounds.Width,30)
            };

            notificationView.Add(important);
            notificationView.Add(Title);
            notificationView.Add(Date);
            notificationView.Add(text);

            notificationView.Frame = new CGRect(0, 0, ContentView.Bounds.Width, 120);

            ContentView.AddSubview(notificationView);

        }

        public void UpdateCell(string caption, string subtitle, UIImage image)
        {
            /*imageView.Image = image;
            headingLabel.Text = caption;
            subheadingLabel.Text = subtitle;*/
            //heightForRowAtIndexPath = 120;
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            /*imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 33, 33);
            headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
            subheadingLabel.Frame = new CGRect(100, 18, 100, 20);*/
        }
    }
}
