using System;
using System.Threading.Tasks;

namespace CanvasTest
{
	public interface IImagePicker
	{
		Task<string> Pick();
	}
}
