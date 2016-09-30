using System;
using System.Collections.Generic;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace SlideshowLibrary
{
	public partial class SlideshowView : UIView
	{
		private List<string> dataSource;

		#region Ctors
		public SlideshowView(IntPtr handle) : base(handle)
		{

		}
		#endregion

		#region
		public static SlideshowView Create()
		{
			var arr = NSBundle.MainBundle.LoadNib("SlideshowView", null, null);
			var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0)) as SlideshowView;

			return v;
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

		public void Initialize(List<string> dataSource)
		{
			this.dataSource = dataSource;

			List<UIColor> colorList = new List<UIColor>();
			colorList.Add(UIColor.Red);
			colorList.Add(UIColor.Blue);
			colorList.Add(UIColor.Yellow);
			colorList.Add(UIColor.Blue);
			colorList.Add(UIColor.Black);

			pager.Pages = dataSource.Count;

			int counter = 0;
			foreach (var url in dataSource)
			{
				UIImageView imageView = new UIImageView();
				imageView.BackgroundColor = colorList[counter];

				imageView.Frame = new CoreGraphics.CGRect(counter * 375, 0, 375, 200);
				contentContainer.AddSubview(imageView);
				counter++;
			}

			cstContentContainer.Constant = counter * 375;
			scrollView.ContentSize = new CoreGraphics.CGSize(counter * 375, 0);

			contentContainer.SetNeedsLayout();
			contentContainer.LayoutIfNeeded();

			scrollView.SetNeedsLayout();
			scrollView.LayoutIfNeeded();

			scrollView.Scrolled += (sender, e) => {

				var pageNumber = (int)Math.Round(scrollView.ContentOffset.X / 375);
				pager.CurrentPage = pageNumber;			
			};
		}

		#endregion
	}
}
