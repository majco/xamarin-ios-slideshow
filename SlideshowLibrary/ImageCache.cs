using System;
using Foundation;
using SDWebImage;
using UIKit;

namespace SlideshowLibrary
{
	public static class ImageCache
	{
		public static void SetImage(this UIImageView imageView, string url, Action<NSError> callback)
		{
			Uri uri;
			UIImage placeHolder = UIImage.FromBundle("no-image");

			if (Uri.TryCreate(url, UriKind.Absolute, out uri))
			{
				imageView.SetImage(
					new NSUrl(uri.AbsoluteUri),
					placeHolder,
					SDWebImageOptions.TransformAnimatedImage,
					(image, error, cacheType, imageUrl) =>
					{
						if (callback != null)
						{
							callback(error);
						}
					});
			}
			else
			{
				imageView.Image = placeHolder;
			}
		}
	}
}
