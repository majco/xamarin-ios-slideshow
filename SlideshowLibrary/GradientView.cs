using System;
using System.Collections.Generic;
using CoreGraphics;
using UIKit;

namespace SlideshowLibrary
{
	public class GradientView : UIView
	{
		#region Fields

		private List<CGColor> colors = new List<CGColor>() { UIColor.Clear.CGColor, UIColor.Black.ColorWithAlpha(0.6f).CGColor };
		private nfloat[] locations = new nfloat[] { 0f, 1f };
		private CGGradient gradient;
		#endregion

		#region Ctors
		public GradientView(IntPtr handle) : base(handle)
		{

		}

		public GradientView()
		{
			gradient = new CGGradient(colors[0].ColorSpace, colors.ToArray(), locations);
		}
		#endregion

		public override void Draw(CGRect rect)
		{
			var context = UIGraphics.GetCurrentContext();

			var startPoint = CGPoint.Empty;
			var endPoint =  new CGPoint(0, Bounds.Height);

			context.DrawLinearGradient(gradient, startPoint, endPoint, 0);
		}
	}
}
