using System;

namespace UI
{
	public class AlertPayloadData
	{
		public string Title;

		public string Message;

		public Action<bool> OnClick;

		public string ButtonOKText;

		public string ButtonCancelText;

		public AlertPayloadData(string title, string message, string buttonOKText, string buttonCancelText, Action<bool> onClick)
		{
			Title = title;
			Message = message;
			OnClick = onClick;
			ButtonOKText = buttonOKText;
			ButtonCancelText = buttonCancelText;
		}
	}
}
