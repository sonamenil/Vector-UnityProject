namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_Key : TriggerEvent
	{
		public string Key;

		public TE_Key(string p_value = "")
		{
            Key = p_value;
            _Type = EventType.TET_KEY;
        }
	}
}
