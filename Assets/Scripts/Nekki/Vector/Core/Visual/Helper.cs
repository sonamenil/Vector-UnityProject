using System;
using System.Collections.Generic;

namespace Nekki.Vector.Core.Visual
{
	public static class Helper
	{
		private static List<string> _ToFlipX = new List<string>();

		private static List<string> _NewName = new List<string>();

		private static List<string> _Paths = new List<string>();

		public static List<string> Paths
		{
			get => _Paths;
			set => throw new NotImplementedException();
		}

		public static void Init()
		{

		}

		public static KeyValuePair<bool, string> Parameter(string Name)
		{
			return default(KeyValuePair<bool, string>);
		}
	}
}
