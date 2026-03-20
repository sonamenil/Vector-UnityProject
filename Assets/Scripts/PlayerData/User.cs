using System;
using System.Collections.Generic;

namespace PlayerData
{
	[Serializable]
	public class User
	{
		public Settings Settings;

		public GameStats GameStats;

		public ShopData ShopData;

		public Dictionary<string, string> MiscOldData;
	}
}
