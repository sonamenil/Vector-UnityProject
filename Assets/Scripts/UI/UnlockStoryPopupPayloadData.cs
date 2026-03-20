using System;

namespace UI
{
	public class UnlockStoryPopupPayloadData
	{
		public int StarsNumber;

		public int Price;

		public string StoryName;

		public string Caption;

		public Action Action;

		public UnlockStoryPopupPayloadData(int starsNumber, int price, string caption, string storyName, Action action)
		{
			StarsNumber = starsNumber;
			Price = price;
            Caption = caption;
            Action = action;
            StoryName = storyName;
		}
	}
}
