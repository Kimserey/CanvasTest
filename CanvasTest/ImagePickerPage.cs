using System;
using Xamarin.Forms;

namespace CanvasTest
{
	public class ImagePickerPage : ContentPage
	{
		public ImagePickerPage()
		{
			var layout =
				new AbsoluteLayout
				{
					BackgroundColor = Color.Blue
				};

			var image =
				new Image
				{
					BackgroundColor = Color.White
				};

			var button =
				new Button
				{
					Text = "Pick image",
					HeightRequest = 50,
					VerticalOptions = LayoutOptions.Center
				};

			button.Clicked += async (sender, e) =>
			{
				var imagePath = await DependencyService.Get<IImagePicker>().Pick();
				image.Source = ImageSource.FromFile(imagePath);
			};

			layout.Children.Add(image, new Rectangle(0, 0, 1, .7), AbsoluteLayoutFlags.All);
			layout.Children.Add(button, new Rectangle(0, 1, 1, .3), AbsoluteLayoutFlags.All);

			this.Title = "Pick image to crop";
			this.Content = layout;
		}
	}
}
