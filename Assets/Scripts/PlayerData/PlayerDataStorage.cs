namespace PlayerData
{
	public class PlayerDataStorage
	{
		public static string Filename;

		public static T GetData<T>(string fieldName)
		{
			return default(T);
		}

		public static bool HasData()
		{
			return false;
		}

		private static string ReadData()
		{
			return null;
		}

		public static void Clear()
		{
		}
	}
}
