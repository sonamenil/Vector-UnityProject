namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_Activate : TriggerEvent
	{
		public string ActionID;

		public TE_Activate(string p_ActionID)
		{
			ActionID = p_ActionID;
			_Type = EventType.TET_ACTIVATE;
		}
	}
}
