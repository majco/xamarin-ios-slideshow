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

			var windowWidth = UIApplication.SharedApplication.Windows[0].Frame.Width;
			var slideShowHeight = 200;

			var slideShow = SlideshowLibrary.SlideshowView.Create();
			slideShow.Frame = new CoreGraphics.CGRect(0, 0, windowWidth, slideShowHeight);

			List<string> urlList = new List<string>();
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/04/22/22/11/dandelion-1346727_1280.jpg");
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/07/21/20/56/anemone-1533515_1280.jpg");
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/07/26/10/14/jumper-1542465_1280.jpg");
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/07/27/00/58/oldtimer-1544342_1280.jpg");
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/08/26/10/02/holocaust-1621728_1280.jpg");
			urlList.Add("https://pixabay.com/static/uploads/photo/2016/08/15/14/35/lavender-blossom-1595581_1280.jpg");

			slideshowContainer.Frame = new CoreGraphics.CGRect(0, 0, windowWidth, slideShowHeight);
			slideshowContainer.AddSubview(slideShow);

			var settings = new SlideshowLibrary.SlideshowSettings();
			settings.SwapIntervalMillis = 5000;
			settings.AddShadow = true;

			slideshowContainer.SetNeedsLayout();
			slideshowContainer.LayoutIfNeeded();

			slideShow.Initialize(urlList, settings);

			View.SetNeedsLayout();
			View.LayoutIfNeeded();
		}
	}
}
