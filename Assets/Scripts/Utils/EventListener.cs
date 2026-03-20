namespace Utils
{
	public class EventListener
	{
		public delegate void EventHandler(EventArgs eventArgs);

		public EventHandler eventHandler;

		public void Invoke(EventArgs eventArgs)
		{
            eventHandler?.Invoke(eventArgs);
        }

		public void Clear()
		{
			eventHandler = null;
		}
	}
}
