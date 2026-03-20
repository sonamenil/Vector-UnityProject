using Nekki.Vector.Core.Location;
using System.Collections.Generic;

namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_StartGame : TriggerEvent
	{
		private static List<TriggerRunner> _WithThisEvent;

		public TE_StartGame(TriggerRunner p_parent)
		{
            _Type = EventType.TET_ON_START_GAME;
            if (p_parent != null)
            {
                AddToWithThisEvent(p_parent);
            }
        }

		private static void AddToWithThisEvent(TriggerRunner p_parent)
		{
            if (_WithThisEvent == null)
            {
                _WithThisEvent = new List<TriggerRunner>();
            }
            if (!_WithThisEvent.Contains(p_parent))
            {
                _WithThisEvent.Add(p_parent);
            }
        }

		public static void ActivateThisEvent()
		{
            if (_WithThisEvent == null)
            {
                return;
            }
            TE_StartGame p_event = new TE_StartGame(null);
            foreach (TriggerRunner item in _WithThisEvent)
            {
                item.CheckEvent(p_event, null);
            }
        }
	}
}
