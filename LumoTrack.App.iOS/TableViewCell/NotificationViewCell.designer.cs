// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LumoTrack.App.iOS.TableViewCell
{
    [Register ("NotificationViewCell")]
    partial class NotificationViewCell
    {
        [Outlet]
        UIKit.UIView ContentLayout { get; set; }


        [Outlet]
        UIKit.UILabel Date { get; set; }


        [Outlet]
        UIKit.UIView ImportantView { get; set; }


        [Outlet]
        UIKit.UILabel Text { get; set; }


        [Outlet]
        UIKit.UILabel Title { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContentLayout != null) {
                ContentLayout.Dispose ();
                ContentLayout = null;
            }

            if (Date != null) {
                Date.Dispose ();
                Date = null;
            }

            if (ImportantView != null) {
                ImportantView.Dispose ();
                ImportantView = null;
            }

            if (Text != null) {
                Text.Dispose ();
                Text = null;
            }

            if (Title != null) {
                Title.Dispose ();
                Title = null;
            }
        }
    }
}