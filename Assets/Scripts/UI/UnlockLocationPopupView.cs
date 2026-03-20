using UnityEngine.UI;

namespace UI
{
	public class UnlockLocationPopupView : UnlockPopupView<UnlockLocationPopup, UnlockLocationPopupPayloadData>
	{
		public Text StoryName;

		public Text LocationName;

		public override void Init(UnlockLocationPopup yesNoLocationPopup)
		{
		}

		public override void PreShow(UnlockLocationPopupPayloadData payload)
		{
		}

		public override void PostShow(UnlockLocationPopupPayloadData payload)
		{
		}

		public override void Back()
		{
		}
	}
}
