using System;

using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class TabBarController : UITabBarController
    {
        public TabBarController() : base("TabBarController", null)
        {

        }
        public TabBarController(IntPtr handle) : base(handle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TabBar.UnselectedItemTintColor = UIColor.FromRGB(87,16,31);
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidAppear(bool animated)
        {
            Console.WriteLine("Appear");
            base.ViewDidAppear(animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            Console.WriteLine("WillAppear");
            base.ViewWillAppear(animated);
        }
    }
}

