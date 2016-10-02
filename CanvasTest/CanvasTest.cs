using System;

using Xamarin.Forms;

namespace CanvasTest
{
	public class TestView : Image
	{
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
	}

	public class App : Application
	{
		public App()
		{
			var layout = new AbsoluteLayout();
			var view = new TestView();

			var widthSlider =
				new Slider
				{
					HeightRequest = 50,
					Value = 0.5,
					VerticalOptions = LayoutOptions.Center
				};

			var heightSlider =
				new Slider
				{
					HeightRequest = 50,
					Value = 0.5,
					VerticalOptions = LayoutOptions.Center
				};

			var button =
				new Button
				{
					Text = "Crop image"
				};

			var sliders =
				new StackLayout
				{
					Spacing = 0,
					Children = {
						widthSlider,
						heightSlider,
						button
					}
				};

			widthSlider.ValueChanged += (sender, e) =>
			{
				view.SelectionWidth = e.NewValue;
			};

			heightSlider.ValueChanged += (sender, e) =>
			{
				view.SelectionHeight = e.NewValue;
			};

			layout.Children.Add(view, new Rectangle(0, 0, 1, .7), AbsoluteLayoutFlags.All);
			layout.Children.Add(sliders, new Rectangle(0, 1, 1, .3), AbsoluteLayoutFlags.All);

			var content =
				new ContentPage
				{
					Title = "Canvas test",
					Content = layout
				};

			MainPage = new NavigationPage(content);
		}
	}
}
