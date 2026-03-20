using System;

namespace UI
{
	public class BoostPayloadData
	{
		public enum DialogType
		{
			BOOST,
			AFTER_BOOST,
			AFTER_PUSH
		}

		public Action<bool> OnClick;

		public DialogType Type;

		public BoostPayloadData(DialogType type, Action<bool> onClick)
		{
		}
	}
}
