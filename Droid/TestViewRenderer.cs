using System;
using System.IO;
using Xamarin.Forms;
using Android.Views;
using CanvasTest;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using Android.Runtime;
using Android.Graphics.Drawables;
using Android.Content;
using Android.Graphics.Drawables.Shapes;
using CanvasTest.Droid;
using Android.OS;
using Java.IO;

[assembly: ExportRenderer(typeof(CropView), typeof(CropViewViewRenderer))]
[assembly: Dependency(typeof(CropService))]
[assembly: Dependency(typeof(CanvasTest.Path))]
namespace CanvasTest
{
	public class Path : IPath
	{
		public string GetPublicDirectory()
		{
			return Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path;
		}
	}

	public class CropService : ICropService
	{
		public string Crop(string imagePath, CropOption options)
		{
			var original = BitmapFactory.DecodeFile(imagePath);
			var resized = Bitmap.CreateBitmap(original, (int)(options.X * original.Width), (int)(options.Y * original.Height), (int)(options.Width * original.Width), (int)(options.Height * original.Height));

			var destinationPath = System.IO.Path.Combine(DependencyService.Get<IPath>().GetPublicDirectory(), "crop_test.jpg");
			using (var file = System.IO.File.Create(destinationPath))
			{
				resized.Compress(Bitmap.CompressFormat.Jpeg, 100, file);
			}

			original.Dispose();
			resized.Dispose();

			return destinationPath;
		}
	}

	[Preserve(AllMembers = true)]
	public class CropViewViewRenderer: ImageRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == CropView.SelectionWidthProperty.PropertyName
			    || e.PropertyName == CropView.SelectionHeightProperty.PropertyName
			    || e.PropertyName == CropView.SelectionXProperty.PropertyName
			    || e.PropertyName == CropView.SelectionYProperty.PropertyName)
			{
				this.Invalidate();
			}
		}

		protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
		{
			var selectionWidth = ((CropView)this.Element).SelectionWidth * this.Width;
			var selectionHeight = ((CropView)this.Element).SelectionHeight * this.Height;
			var selectionX = ((CropView)this.Element).SelectionX * this.Width;
			var selectionY = ((CropView)this.Element).SelectionY * this.Height;

			var path = new Android.Graphics.Path();
			path.AddRect(
				new RectF(
					Convert.ToSingle(selectionX), 
					Convert.ToSingle(selectionY),
					Convert.ToSingle(selectionX + selectionWidth),
					Convert.ToSingle(selectionY + selectionHeight)),
				Android.Graphics.Path.Direction.Ccw);

			var paint = new Paint();
			paint.StrokeWidth = 5;
			paint.SetStyle(Paint.Style.Stroke);
			paint.Color = Android.Graphics.Color.Blue;

			// Draws the Xamarin.Forms image first before adding
			// the selecttion path on top of it.
			var result = base.DrawChild(canvas, child, drawingTime);

			canvas.DrawPath(path, paint);
			path.Dispose();
			paint.Dispose();

			return result;
		}
	}
}
