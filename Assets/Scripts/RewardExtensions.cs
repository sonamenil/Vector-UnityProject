using System;
using System.Collections.Generic;

public static class RewardExtensions
{
	public static int GetReward(this Dictionary<int, int> rewardData, int baseStars, int stars)
	{
		if (baseStars < stars && baseStars < 4 && stars < 4)
		{
			if (stars < baseStars + 1)
			{
				return 0;
			}

			int num = 0;
			for (int i = 0; i < stars; i++)
			{
				var j = rewardData[i + 1];
				num += j;
			}
			return num;
		}
		new Exception();
		return 0;
	}
}
