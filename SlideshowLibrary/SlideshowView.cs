using System;
using System.Collections.Generic;
using System.Threading;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace SlideshowLibrary
{
	public partial class SlideshowView : UIView
	{
		#region Fields

		private static string TAG = "SlideshowView";

		private List<string> dataSource;
		private SlideshowElements images;
		private SlideshowSettings slideShowSettings;
		private Timer slideshowTimer;
		private int scrollSliderWidth;

		private int prevIndex = FirstImageIndex;
		private int currentIndex = SecondImageIndex;
		private int nextIndex = ThirdImageIndex;

		private const int UIImgeViewsCount = 3;

		private const int FirstImageIndex = 0;
		private const int SecondImageIndex = 1;
		private const int ThirdImageIndex = 2;

		#endregion

		#region Ctors
		public SlideshowView(IntPtr handle) : base(handle)
		{

		}
		#endregion

		#region Methods
		public static SlideshowView Create()
		{
			var arr = NSBundle.MainBundle.LoadNib(TAG, null, null);
			var v = Runtime.GetNSObject<UIView>(arr.ValueAt(0)) as SlideshowView;

			return v;
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
		}

		public void Initialize(List<string> dataSource, SlideshowSettings slideshowSettings)
		{
			this.slideShowSettings = slideshowSettings;
			this.dataSource = dataSource;

			scrollSliderWidth = (int)cstContentContainer.Constant;
			InitSlideshow();

			scrollView.DecelerationStarted += OnDeccelerationStarted;
			scrollView.ScrollAnimationEnded += OnScrollAnimationEnded;
			scrollView.DecelerationEnded += OnDeccelerationEnded; 

			StartSlideshowTimer();
		}

		#region Private

		private void LoadImage(int imageViewIndex, int dataSourceIndex)
		{
			var image = images.GetKeyByIndex(imageViewIndex);
			var activityIndicator = images[image];

			activityIndicator.Hidden = false;
			image.SetImage(dataSource[dataSourceIndex], (NSError error) =>
			{				
				if (error == null)
				{					
					activityIndicator.Hidden = true;
				}
			});
		}

		partial void onPagerValueChanged(UIPageControl sender)
		{
			StopSlideshowTimer();

			var shouldMoveRight = Math.Abs(sender.CurrentPage - currentIndex) > 1 
			                     ? (sender.CurrentPage - currentIndex) < 0 
			                     : sender.CurrentPage - currentIndex > 0;
			
			if (shouldMoveRight)
				MoveRight();
			else
				MoveLeft();			

			scrollView.SetContentOffset(new CGPoint(scrollSliderWidth, 0), false);
			scrollView.UserInteractionEnabled = true;

			StartSlideshowTimer();
		}

		private void MoveRight()
		{
			LoadImage(FirstImageIndex, currentIndex);

			currentIndex = GetCurrentIndexOnMoveRight();
			LoadImage(SecondImageIndex, currentIndex);

			nextIndex = GetCurrentIndexOnMoveRight();
			LoadImage(ThirdImageIndex, nextIndex);

			prevIndex = ((currentIndex - 1) < 0 
			             ? (dataSource.Count + (currentIndex - 1)) 
			             : currentIndex - 1) % dataSource.Count;
		}

		private void MoveLeft()
		{
			LoadImage(ThirdImageIndex, currentIndex);

			currentIndex = GetCurrentIndexOnMoveLeft();
			LoadImage(SecondImageIndex, currentIndex);

			prevIndex = GetCurrentIndexOnMoveLeft();
			LoadImage(FirstImageIndex, prevIndex);

			nextIndex = (currentIndex + 1) % dataSource.Count;
		}

		private void SlideshowTimerCallback(object state)
		{
			InvokeOnMainThread(() =>
			{
				int direction = 2;
				scrollView.SetContentOffset(new CGPoint(direction * scrollSliderWidth, 0), true);
			});
		}

		private void InitSlideshow()
		{
			if(slideShowSettings.AddShadow)
				InitShadow();

			images = new SlideshowElements();
			for (int i = 0; i < UIImgeViewsCount; i++)
			{
				UIImageView imageView = new UIImageView();
				imageView.ContentMode = UIViewContentMode.ScaleAspectFill;
				imageView.BackgroundColor = UIColor.LightGray;
				imageView.Frame = new CGRect(i * cstContentContainer.Constant,
				                                          0, cstContentContainer.Constant, Frame.Height);

				UIActivityIndicatorView activityIndicator = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
				activityIndicator.Frame = imageView.Frame;
				activityIndicator.Color = UIColor.Black;
				activityIndicator.StartAnimating();

				imageView.SetImage(dataSource[i], (error) =>
				{
					activityIndicator.Hidden |= error == null;
				});

				contentContainer.AddSubview(imageView);
				contentContainer.AddSubview(activityIndicator);

				images.Add(imageView, activityIndicator);
			}
			cstContentContainer.Constant *= UIImgeViewsCount;
			scrollView.ContentSize = new CGSize(cstContentContainer.Constant, 0);

			scrollView.SetNeedsLayout();
			scrollView.LayoutIfNeeded();

			scrollView.ContentOffset = new CGPoint(scrollSliderWidth, 0);

			pager.Pages = dataSource.Count;
			pager.CurrentPage = currentIndex;
		}

		private void OnDeccelerationStarted(object sender, EventArgs args)
		{
			StopSlideshowTimer();
			scrollView.UserInteractionEnabled = false;
		}

		private void OnDeccelerationEnded(object sender, EventArgs args)
		{
			if (scrollView.ContentOffset.X > scrollView.Frame.Size.Width)
				MoveRight();
			else if (scrollView.ContentOffset.X < scrollView.Frame.Size.Width)
				MoveLeft();

			scrollView.SetContentOffset(new CGPoint(scrollSliderWidth, 0), false);

			pager.CurrentPage = currentIndex;
			scrollView.UserInteractionEnabled = true;

			StartSlideshowTimer();
		}

		private void OnScrollAnimationEnded(object sender, EventArgs args)
		{
			MoveRight();
			scrollView.SetContentOffset(new CGPoint(scrollSliderWidth, 0), false);

			pager.CurrentPage = currentIndex;
			scrollView.UserInteractionEnabled = true;
		}

		private void StartSlideshowTimer()
		{
			slideshowTimer = new Timer(new TimerCallback(SlideshowTimerCallback), null, slideShowSettings.SwapIntervalMillis, 
			                  										  slideShowSettings.SwapIntervalMillis);
		}

		private void StopSlideshowTimer()
		{
			slideshowTimer.Dispose();
		}

		private int GetCurrentIndexOnMoveRight()
		{
			return (currentIndex >= dataSource.Count - 1) ? 0 : currentIndex + 1;
		}

		private int GetCurrentIndexOnMoveLeft()
		{
			return (currentIndex == 0) ? dataSource.Count - 1 : currentIndex - 1;
		}

		private void InitShadow()
		{
			GradientView gradientView = new GradientView();
			gradientView.Frame = new CGRect(0, 0, shadowLine.Frame.Width, shadowLine.Frame.Height);
			gradientView.BackgroundColor = UIColor.Clear;
			gradientView.UserInteractionEnabled = false;

			shadowLine.AddSubview(gradientView);
		}

		#endregion

		#endregion
	}
}
