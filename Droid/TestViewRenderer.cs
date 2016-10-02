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

[assembly: ExportRenderer(typeof(TestView), typeof(TestViewRenderer))]
namespace CanvasTest
{

	public class CropService : ICropService
	{
		public string Crop(string imagePath, CropOption options)
		{
			throw new NotImplementedException();
		}
	}

	[Preserve(AllMembers = true)]
	public class TestViewRenderer: ImageRenderer
	{
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == TestView.SelectionWidthProperty.PropertyName
			    || e.PropertyName == TestView.SelectionHeightProperty.PropertyName
			    || e.PropertyName == TestView.SelectionXProperty.PropertyName
			    || e.PropertyName == TestView.SelectionYProperty.PropertyName
			    || e.PropertyName == TestView.SourcePathProperty.PropertyName)
			{
				this.Invalidate();
			}
		}

		protected override bool DrawChild(Canvas canvas, Android.Views.View child, long drawingTime)
		{
			var sourcePath = ((TestView)this.Element).SourcePath;

			if(string.IsNullOrWhiteSpace(sourcePath))
				return base.DrawChild(canvas, child, drawingTime);
			
			var selectionWidth = ((TestView)this.Element).SelectionWidth * this.Width;
			var selectionHeight = ((TestView)this.Element).SelectionHeight * this.Height;
			var selectionX = ((TestView)this.Element).SelectionX * this.Width;
			var selectionY = ((TestView)this.Element).SelectionY * this.Height;

			sourcePath = System.IO.Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).Path, sourcePath);
			var bitmap = BitmapFactory.DecodeFile(sourcePath);

			var path = new Android.Graphics.Path();
			path.AddRect(
				new RectF(
					Convert.ToSingle(selectionX - (selectionWidth / 2.0)), 
					Convert.ToSingle(selectionY - (selectionHeight / 2.0)),
					Convert.ToSingle(selectionX + (selectionWidth / 2.0)),
					Convert.ToSingle(selectionY + (selectionHeight / 2.0))),
				Android.Graphics.Path.Direction.Ccw);

			var paint = new Paint();
			paint.StrokeWidth = 5;
			paint.SetStyle(Paint.Style.Stroke);
			paint.Color = Android.Graphics.Color.Blue;

			canvas.DrawBitmap(bitmap, null, new Rect(0, 0, this.Width, this.Height), paint);
			canvas.DrawPath(path, paint);
			
			path.Dispose();
			paint.Dispose();

			return base.DrawChild(canvas, child, drawingTime);
		}
	}
}
