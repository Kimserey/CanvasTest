using System;

namespace CanvasTest
{
	public class CropOption
	{ 
		public double Width
		{
			get;
			set;
		}

		public double Height
		{
			get;
			set;
		}

		public double X
		{
			get;
			set;
		}

		public double Y
		{
			get;
			set;
		}
	}
	
	public interface ICropService
	{
		string Crop(CropOption options);
	}
}
