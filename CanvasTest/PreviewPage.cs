using System;
using Xamarin.Forms;

namespace CanvasTest
{
	public class PreviewPage : ContentPage
	{
		public PreviewPage(string imagePath)
		{
			this.Title = "Cropped preview";

			this.Content =
				new Image
				{
					Source = ImageSource.FromFile(imagePath)
				};
		}
	}
}
