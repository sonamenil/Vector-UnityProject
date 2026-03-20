using System.Collections.Generic;
using Nekki.Vector.Core.Utilites;

namespace Nekki.Vector.Core.Location
{
	public class CoinSetter
	{
		private class Groups
		{
			public Dictionary<int, List<CoinRunner>> groups = new Dictionary<int, List<CoinRunner>>();

			public List<int> groupIDs = new List<int>();
		}

		public static void SetCoins(List<CoinRunner> coinRunners, int totalCount)
		{
			if (coinRunners.Count != 0)
			{
				var groups = SetGroups(coinRunners);
				if (totalCount > 0)
				{
					for (int i = 0; i < totalCount; i++)
					{
						bool flag = IsAllEnableGroups(groups.groups, i);
						if (!flag)
						{
							UpdateGroups(groups, ref totalCount);
						}
					}
				}
			}
		}

		private static Groups SetGroups(List<CoinRunner> coinRunners)
		{
			var groups = new Groups();
			foreach (var coin in coinRunners)
			{
				if (!groups.groupIDs.Contains(coin.GroupID))
				{
					groups.groupIDs.Add(coin.GroupID);
				}
				if (!groups.groups.ContainsKey(coin.GroupID))
				{
					groups.groups[coin.GroupID] = new List<CoinRunner>();
				}
				groups.groups[coin.GroupID].Add(coin);
			}
			groups.groupIDs.Shuffle();
			return groups;
		}

		private static bool IsAllEnableGroups(Dictionary<int, List<CoinRunner>> groupsGroups, int count)
		{
			foreach (var group in groupsGroups.Values)
			{
				if (!IsEnabledGroup(group, count))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsEnabledGroup(List<CoinRunner> groupValue, int count)
		{
            bool flag = true;
            foreach (var coin in groupValue)
            {
                if (!coin.enabled)
                {
                    flag = false;
                }
            }
            return flag;

        }

        private static void UpdateGroups(Groups groups, ref int count)
		{
			foreach (var id in groups.groupIDs)
			{
				if (count <= 0)
				{
					return;
				}
				var coin = GetCoin(groups.groups[id]);
				coin.SetEnable(true);
				count -= coin.score;
			}
		}

		private static CoinRunner GetCoin(List<CoinRunner> groupsGroup)
		{
			groupsGroup.Shuffle();
			foreach (var coin in groupsGroup)
			{
				return coin;
			}
			return null;
		}
	}
}
