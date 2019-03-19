// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace LumoTrack.App.iOS.ViewControllers
{
    [Register ("CommentViewController")]
    partial class CommentViewController
    {
        [Outlet]
        UIKit.UIView ContentView { get; set; }

        [Outlet]
        UIKit.UITextView MessageTextView { get; set; }

        [Outlet]
        UIKit.UIPickerView ReportSpinner { get; set; }

        [Outlet]
        UIKit.UIScrollView Scrollview { get; set; }

        [Outlet]
        UIKit.UIButton SendButton { get; set; }

        [Outlet]
        UIKit.UIPickerView TruckSpinner { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ContentView != null) {
                ContentView.Dispose ();
                ContentView = null;
            }

            if (MessageTextView != null) {
                MessageTextView.Dispose ();
                MessageTextView = null;
            }

            if (ReportSpinner != null) {
                ReportSpinner.Dispose ();
                ReportSpinner = null;
            }

            if (Scrollview != null) {
                Scrollview.Dispose ();
                Scrollview = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (TruckSpinner != null) {
                TruckSpinner.Dispose ();
                TruckSpinner = null;
            }
        }
    }
}