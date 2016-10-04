using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Provider;

namespace CanvasTest.Droid
{
	[Activity(ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class ImagePickerActivity : Activity
	{
		internal const string ExtraId = "id";
		int id;

		internal static event EventHandler<ImagePicked> ImagePicked;

		protected override void OnCreate(Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			var bundle = savedInstanceState ?? this.Intent.Extras;
			id = bundle.GetInt(ExtraId);

			var intent = new Intent(Intent.ActionGetContent);
			intent.SetType("image/*");
			StartActivityForResult(intent, id);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			if (requestCode != id
				|| ImagePicked == null
				|| resultCode == Result.Canceled)
				return;

			var uri = data.Data;
			switch (uri.Scheme)
			{
				case "file":
					ImagePicked(this, new ImagePicked { AbsolutePath = (new System.Uri(uri.ToString())).AbsolutePath });
					break;
				case "content":
					ICursor cursor = null;
					try
					{
						string[] proj = null;
						if ((int)Build.VERSION.SdkInt >= 22)
							proj = new[] { MediaStore.MediaColumns.Data };

						cursor = Application.Context.ContentResolver.Query(uri, proj, null, null, null);

						if (cursor == null || !cursor.MoveToNext())
						{
							ImagePicked(this, new ImagePicked { IsCanceled = true });
						}
						else
						{
							var directory = Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures).Path;
							var tempName = "pick.tmp";
							string contentPath = System.IO.Path.Combine(directory, tempName);

							try
							{
								using (Stream input = Application.Context.ContentResolver.OpenInputStream(uri))
								using (Stream output = File.Create(contentPath))
									input.CopyTo(output);
							}
							catch (Java.IO.FileNotFoundException)
							{
								//log file not found exception
							}

							ImagePicked(this, new ImagePicked { AbsolutePath = contentPath });
						}
					}
					finally
					{
						if (cursor != null)
						{
							cursor.Close();
							cursor.Dispose();
						}
					}
					break;
				default:
					ImagePicked(this, new ImagePicked { Error = new NotSupportedException("Uri scheme not supported.") });
					break;

			}

			Finish();
		}
	}

	public class ImagePicked : EventArgs
	{
		public string AbsolutePath
		{
			get;
			set;
		}

		public Exception Error
		{
			get;
			set;
		}

		public bool IsCanceled
		{
			get;
			set;
		}
	}
}
