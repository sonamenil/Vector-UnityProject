using System.Collections.Generic;

namespace Utils
{
	public class EventDispatcher
	{
		private Dictionary<string, EventListener> dic = new Dictionary<string, EventListener>();

		public void AddListener(string eventType, EventListener.EventHandler eventHandler)
		{
            if (!dic.TryGetValue(eventType, out var listener))
            {
                listener = new EventListener();
                dic[eventType] = listener;
            }
            listener.eventHandler += eventHandler;
        }

		public void RemoveListener(string eventType, EventListener.EventHandler eventHandler)
		{
            if (dic.TryGetValue(eventType, out var listener))
            {
                listener.eventHandler -= eventHandler;
                if (listener.eventHandler == null)
                {
                    dic.Remove(eventType);
                }
            }
        }

		public bool HasListener(string eventType)
		{
            return dic.TryGetValue(eventType, out var listener) && listener.eventHandler != null;
        }

		public void DispatchEvent(string eventType, params object[] args)
		{
            if (dic.TryGetValue(eventType, out var listener))
            {
                var eventArgs = (args == null)
                    ? new EventArgs(eventType)
                    : new EventArgs(eventType, args);

                listener.Invoke(eventArgs);
            }
        }

		public void Clear()
		{
			foreach (var listener in dic.Values)
			{
				listener.Clear();
			}
			dic.Clear();
		}
	}
}
