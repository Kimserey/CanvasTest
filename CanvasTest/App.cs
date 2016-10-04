using System;
using System.IO;
using Xamarin.Forms;

namespace CanvasTest
{
	public class App : Application
	{
		public App()
		{
			MainPage = new NavigationPage(new ImagePickerPage());
		}
	}
}
