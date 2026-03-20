using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_OnShow : TriggerEvent
	{
		public TE_OnShow()
		{
			_Type = EventType.TET_ON_SHOW;
		}

		public TE_OnShow(TriggerRunner p_parent)
		{
            _Type = EventType.TET_ON_SHOW;
        }
    }
}
