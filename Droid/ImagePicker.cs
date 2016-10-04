using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Xamarin.Forms;
using CanvasTest.Droid;
using Android.Content;

[assembly: Dependency(typeof(ImagePicker))]
namespace CanvasTest.Droid
{
	public class ImagePicker : IImagePicker
	{
		TaskCompletionSource<string> _completionSource;
		int _id;

		int NextId()
		{
			int id = _id;
			if (_id == Int32.MaxValue) _id = 0;
			else _id++;
			return id;
		}

		public Task<string> Pick()
		{
			var id = NextId();
			var next = new TaskCompletionSource<string>(id);

			EventHandler<ImagePicked> handler = null;

			handler = (sender, e) =>
			{
				var tcs = Interlocked.Exchange(ref _completionSource, null);

				ImagePickerActivity.ImagePicked -= handler;

				if (e.IsCanceled)
					tcs.SetResult("Cancelled");
				else if (e.Error != null)
					tcs.SetException(e.Error);
				else
					tcs.SetResult(e.AbsolutePath);
			};

			ImagePickerActivity.ImagePicked += handler;

			var pickerIntent = new Intent(Android.App.Application.Context, typeof(ImagePickerActivity));
			pickerIntent.PutExtra(ImagePickerActivity.ExtraId, id);
			pickerIntent.SetFlags(ActivityFlags.NewTask);
			Android.App.Application.Context.StartActivity(pickerIntent);

			if (Interlocked.CompareExchange(ref _completionSource, next, null) != null)
				throw new InvalidOperationException("Another task is already started.");

			return _completionSource.Task;
		}
	}
}
