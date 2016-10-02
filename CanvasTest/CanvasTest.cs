using System;

using Xamarin.Forms;

namespace CanvasTest
{
	public class TestView : Image
	{
		public static readonly BindableProperty SelectionWidthProperty =
			BindableProperty.Create(
				propertyName: nameof(SelectionWidth),
			  	returnType: typeof(int),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0);

		public int SelectionWidth
		{
			get { return (int)GetValue(SelectionWidthProperty); }
			set { SetValue(SelectionWidthProperty, value); }
		}

		public static readonly BindableProperty SelectionHeightProperty =
			BindableProperty.Create(
				propertyName: nameof(SelectionHeight),
			  	returnType: typeof(int),
			  	declaringType: typeof(TestView),
			  	defaultValue: 0);

		public int SelectionHeight
		{
			get { return (int)GetValue(SelectionHeightProperty); }
			set { SetValue(SelectionHeightProperty, value); }
		}
	}

	public class App : Application
	{
		public App()
		{
			var layout = new AbsoluteLayout();
			var view = new TestView { HeightRequest = 100, WidthRequest = 100 };
			var widthSlider = new Slider();
			var heightSlider = new Slider();

			var sliders = new StackLayout
			{
				Spacing = 0,
				Children = {
					widthSlider,
					heightSlider
				}
			};

			widthSlider.ValueChanged += (sender, e) => {
				view.SelectionWidth = (int)e.NewValue;
			};
			heightSlider.ValueChanged += (sender, e) => { 
				view.SelectionHeight = (int)e.NewValue;
			};

			layout.Children.Add(view, new Rectangle(0, 0, 1, .8), AbsoluteLayoutFlags.All);
			layout.Children.Add(sliders, new Rectangle(0, 1, 1, .2), AbsoluteLayoutFlags.All);

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
