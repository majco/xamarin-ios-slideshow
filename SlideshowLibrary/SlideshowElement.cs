using System.Collections.Generic;
using UIKit;
using System.Linq;

namespace SlideshowLibrary
{
	public class SlideshowElements : Dictionary<UIImageView, UIActivityIndicatorView>
	{
		public UIImageView GetKeyByIndex(int index)
		{
			return Keys.ElementAt(index);
		}
	}
}
