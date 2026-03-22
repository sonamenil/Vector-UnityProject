using System.Collections.Generic;
using System.Xml;

namespace Nekki.Vector.Core.Trigger.Conditions
{
	public class TC_OperatorAnd : TriggerCondition
	{
		public List<TriggerCondition> _Conditions = new List<TriggerCondition>();

		public TC_OperatorAnd(TriggerLoop p_parent, XmlNode p_node)
			: base(p_parent, p_node)
		{
			ParseConditions(p_node, _Parent, _Conditions);
		}

		public override bool Check()
		{
            foreach (TriggerCondition condition in _Conditions)
            {
                if (!condition.Check())
                {
                    return _IsNot;
                }
            }
            return !_IsNot;
        }

        public override string ToString()
		{
            string text = "And: ";
            foreach (TriggerCondition condition in _Conditions)
            {
                text = text + "\n    " + condition;
            }
            return text;
        }
	}
}
