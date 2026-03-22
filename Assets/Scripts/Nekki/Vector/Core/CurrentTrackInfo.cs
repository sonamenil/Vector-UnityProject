namespace Nekki.Vector.Core
{
	public class CurrentTrackInfo
	{
		private static CurrentTrackInfo _Instance = new CurrentTrackInfo();

		private string _LocationFile;

		public static CurrentTrackInfo Current => _Instance;

		public string LocationFile
		{
			get => _LocationFile;
			set => _LocationFile = value;
		}
	}
}
