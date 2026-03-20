using System;

namespace UI
{
	public class ViewAttribute : Attribute
	{
		public string Path;

		public ViewAttribute(string path)
		{
			Path = path;
		}
	}
}
