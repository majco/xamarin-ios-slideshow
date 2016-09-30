// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SlideshowLibrary
{
	[Register ("SlideshowView")]
	partial class SlideshowView
	{
		[Outlet]
		UIKit.UIView contentContainer { get; set; }

		[Outlet]
		UIKit.NSLayoutConstraint cstContentContainer { get; set; }

		[Outlet]
		UIKit.UIPageControl pager { get; set; }

		[Outlet]
		UIKit.UIScrollView scrollView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (contentContainer != null) {
				contentContainer.Dispose ();
				contentContainer = null;
			}

			if (cstContentContainer != null) {
				cstContentContainer.Dispose ();
				cstContentContainer = null;
			}

			if (scrollView != null) {
				scrollView.Dispose ();
				scrollView = null;
			}

			if (pager != null) {
				pager.Dispose ();
				pager = null;
			}
		}
	}
}
