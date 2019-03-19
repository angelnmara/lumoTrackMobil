using System;
using CoreLocation;
using Foundation;
using UIKit;

namespace LumoTrack.App.iOS.ViewControllers
{
    public partial class CommentSentViewController : UIViewController
    {
        public CommentSentViewController() : base("CommentSentViewController", null)
        {

        }

        public CommentSentViewController(IntPtr handle) : base(handle)
        {

        }
        public override UIStatusBarStyle PreferredStatusBarStyle()
        {
            return UIStatusBarStyle.Default;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            var controller = segue.DestinationViewController as UITabBarController;

            controller.SelectedIndex = 2;

            this.Unwind(segue, controller);

        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

