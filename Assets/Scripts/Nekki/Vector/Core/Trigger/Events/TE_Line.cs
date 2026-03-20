using System.Xml;

namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_Line : TriggerEvent
	{
		private Variable _Line;

		private static int _CounterID;

		private string _TypeStr;

		public int ID
		{
			get;
			set;
		}

		public TE_Line(TriggerLoop p_parent, XmlNode p_node, int p_ID = -1)
		{
            _Type = EventType.TET_LINE;
            if (p_parent == null)
            {
                ID = p_ID;
                return;
            }
            ID = _CounterID++;
            string value = p_node.Attributes["Position"].Value;
            if (value[0] == '_')
            {
                _Line = p_parent.ParentTrigger.GetVar(value);
            }
            else
            {
                _Line = Variable.createVariable(value, string.Empty);
            }
            _TypeStr = p_node.Attributes["Type"].Value;
            p_parent.SetLine(_TypeStr, _Line, ID);
        }

		public override bool IsEqual(TriggerEvent p_value)
		{
            if (!base.IsEqual(p_value))
            {
                return false;
            }
            return ID == (p_value as TE_Line).ID;
        }
	}
}
