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
    [Register ("TabBarController")]
    partial class TabBarController
    {
        [Outlet]
        UIKit.UITabBar TabBar { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TabBar != null) {
                TabBar.Dispose ();
                TabBar = null;
            }
        }
    }
}