using System;

namespace UI
{
	public class Button
	{
		public Action PressedAction;

		public Action<string> PressedActionWithPayload;
	}
}
