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
    [Register ("NotificationTableViewController")]
    partial class NotificationTableViewController
    {
        [Outlet]
        UIKit.UIView EmptyStateView { get; set; }


        [Outlet]
        UIKit.UITableView NotificationTable { get; set; }


        [Outlet]
        Foundation.NSObject Tabbar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EmptyStateView != null) {
                EmptyStateView.Dispose ();
                EmptyStateView = null;
            }

            if (NotificationTable != null) {
                NotificationTable.Dispose ();
                NotificationTable = null;
            }
        }
    }
}