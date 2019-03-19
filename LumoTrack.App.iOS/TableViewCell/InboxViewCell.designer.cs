// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace LumoTrack.App.iOS.TableViewCell
{
	[Register ("InboxViewCell")]
	partial class InboxViewCell
	{
		[Outlet]
		UIKit.UIView ContentLayout { get; set; }

		[Outlet]
		public UIKit.UILabel Date { get; private set; }

		[Outlet]
		UIKit.UILabel Important { get; set; }

		[Outlet]
		UIKit.UIView ImportantView { get; set; }

		[Outlet]
		UIKit.UIView LayoutContent { get; set; }

		[Outlet]
		UIKit.UILabel ResponseDate { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ResponseMessageHeight { get; set; }

		[Outlet]
		UIKit.UILabel ResponseText { get; set; }

		[Outlet]
		UIKit.UILabel ResponseTitle { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint ResponseTitleHeight { get; set; }

		[Outlet]
		UIKit.UILabel Text { get; set; }

		[Outlet]
		public UIKit.UILabel Title { get; private set; }

		[Outlet]
		UIKit.NSLayoutConstraint TitleHeight { get; set; }
		
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

			if (Important != null) {
				Important.Dispose ();
				Important = null;
			}

			if (ImportantView != null) {
				ImportantView.Dispose ();
				ImportantView = null;
			}

			if (LayoutContent != null) {
				LayoutContent.Dispose ();
				LayoutContent = null;
			}

			if (ResponseDate != null) {
				ResponseDate.Dispose ();
				ResponseDate = null;
			}

			if (ResponseText != null) {
				ResponseText.Dispose ();
				ResponseText = null;
			}

			if (ResponseTitle != null) {
				ResponseTitle.Dispose ();
				ResponseTitle = null;
			}

			if (Text != null) {
				Text.Dispose ();
				Text = null;
			}

			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}

			if (TitleHeight != null) {
				TitleHeight.Dispose ();
				TitleHeight = null;
			}

			if (ResponseTitleHeight != null) {
				ResponseTitleHeight.Dispose ();
				ResponseTitleHeight = null;
			}

			if (ResponseMessageHeight != null) {
				ResponseMessageHeight.Dispose ();
				ResponseMessageHeight = null;
			}
		}
	}
}
