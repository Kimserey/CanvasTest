using System;

using Xamarin.Forms;

namespace CanvasTest
{
	public class TestView : Image
	{
		public static readonly BindableProperty SourcePathProperty =
			BindableProperty.Create(
				propertyName: "SourcePath",
			  	returnType: typeof(string),
			  	declaringType: typeof(TestView));

		public string SourcePath
		{
			get { return (string)GetValue(SourcePathProperty); }
			set { SetValue(SourcePathProperty, value); }
		}

		public static readonly BindableProperty SelectionWidthProperty =
			BindableProperty.Create(
				propertyName: "SelectionWidth",
			  	returnType: typeof(double),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0.5);

		public double SelectionWidth
		{
			get { return (double)GetValue(SelectionWidthProperty); }
			set { SetValue(SelectionWidthProperty, value); }
		}

		public static readonly BindableProperty SelectionHeightProperty =
			BindableProperty.Create(
				propertyName: "SelectionHeight",
			  	returnType: typeof(double),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0.5);

		public double SelectionHeight
		{
			get { return (double)GetValue(SelectionHeightProperty); }
			set { SetValue(SelectionHeightProperty, value); }
		}

		public static readonly BindableProperty SelectionXProperty =
			BindableProperty.Create(
				propertyName: "SelectionX",
			  	returnType: typeof(double),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0.5);

		public double SelectionX
		{
			get { return (double)GetValue(SelectionXProperty); }
			set { SetValue(SelectionXProperty, value); }
		}

		public static readonly BindableProperty SelectionYProperty =
			BindableProperty.Create(
				propertyName: "SelectionY",
			  	returnType: typeof(double),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0.5);

		public double SelectionY
		{
			get { return (double)GetValue(SelectionYProperty); }
			set { SetValue(SelectionYProperty, value); }
		}
	}

	public class CustomSlider : AbsoluteLayout
	{
		Slider slider =
			new Slider
			{
				Margin = 0,
				HeightRequest = 50,
				Value = 0.5,
				VerticalOptions = LayoutOptions.Center
			};

		EventHandler<ValueChangedEventArgs> handler = null;

		public CustomSlider(string text, Action<double> onValueChanged)
		{
			handler = (sender, e) =>
			{
				onValueChanged(e.NewValue);
			};

			var label =
				new Label
				{
					Text = text,
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center
				};

			slider.ValueChanged += handler;
			this.Children.Add(label, new Rectangle(0, 0, 0.1, 1), AbsoluteLayoutFlags.All);
			this.Children.Add(slider, new Rectangle(1, 0, 0.9, 1), AbsoluteLayoutFlags.All);
		}

		~CustomSlider()
		{
			slider.ValueChanged -= handler;
			handler = null;
		}
	}

	public class CropPage : ContentPage
	{
		public CropPage()
		{
			var layout = new AbsoluteLayout();
			var view = new TestView { SourcePath = "camera_test/IMG_20161002_041856316.jpg" };

			var xSlider =
				new CustomSlider(
					"X",
					value =>
					{
						view.SelectionX = value;
					});

			var ySlider =
				new CustomSlider(
					"Y",
					value =>
					{
						view.SelectionY = value;
					});

			var widthSlider =
				new CustomSlider(
					"W",
					value =>
					{
						view.SelectionWidth = value;
					});

			var heightSlider =
				new CustomSlider(
					"H",
					value =>
					{
						view.SelectionHeight = value;
					});

			var button =
				new Button
				{
					Text = "Crop image"
				};

			button.Clicked += (sender, e) =>
			{
				var croppedImagePath =
					DependencyService.Get<ICropService>().Crop(
						view.SourcePath,
						new CropOption
						{
							X = view.SelectionX,
							Y = view.SelectionY,
							Width = view.SelectionWidth,
							Height = view.SelectionHeight
						});

				this.Navigation.PushAsync(new PreviewPage(croppedImagePath));
			};

			var sliders =
				new StackLayout
				{
					Spacing = 0,
					Children = {
						xSlider,
						ySlider,
						widthSlider,
						heightSlider,
						button
					}
				};

			layout.Children.Add(view, new Rectangle(0, 0, 1, .7), AbsoluteLayoutFlags.All);
			layout.Children.Add(sliders, new Rectangle(0, 1, 1, .3), AbsoluteLayoutFlags.All);

			this.Title = "Crop test";
			this.Content = layout;
		}
	}

	public class PreviewPage : ContentPage
	{
		public PreviewPage(string imagePath)
		{
			this.Content =
				new Image
				{
					Source = ImageSource.FromFile(imagePath)
				};
		}
	}

	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new CropPage());
		}
	}
}
