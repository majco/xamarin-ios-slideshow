using System;
using System.Collections.Generic;
using UIKit;

namespace SlideshowDemo
{
	public partial class ViewController : UIViewController
	{
		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var slideShow = SlideshowLibrary.SlideshowView.Create();
			slideShow.Frame = new CoreGraphics.CGRect(0, 0, 375, 200);
			slideShow.SetNeedsLayout();
			slideShow.LayoutIfNeeded();

			List<string> urlList = new List<string>();
			urlList.Add("jeden");
			urlList.Add("jeden");
			urlList.Add("jeden");
			urlList.Add("jeden");

			slideshowContainer.Frame = new CoreGraphics.CGRect(0, 0, 375, 200);
			slideshowContainer.AddSubview(slideShow);

			slideShow.Initialize(urlList);


			View.SetNeedsLayout();
			View.LayoutIfNeeded();
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}
