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
    [Register ("CommentTableViewController")]
    partial class CommentTableViewController
    {
        [Outlet]
        UIKit.UIView EmptyState { get; set; }


        [Outlet]
        UIKit.UITableView MessageTableView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (EmptyState != null) {
                EmptyState.Dispose ();
                EmptyState = null;
            }

            if (MessageTableView != null) {
                MessageTableView.Dispose ();
                MessageTableView = null;
            }
        }
    }
}