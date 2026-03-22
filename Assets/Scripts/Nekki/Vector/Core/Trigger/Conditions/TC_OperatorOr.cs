using System.Collections.Generic;
using System.Xml;

namespace Nekki.Vector.Core.Trigger.Conditions
{
	public class TC_OperatorOr : TriggerCondition
	{
		public List<TriggerCondition> _Conditions = new List<TriggerCondition>();

		public TC_OperatorOr(TriggerLoop p_parent, XmlNode p_node)
			: base(p_parent, p_node)
		{
			ParseConditions(p_node, _Parent, _Conditions);
		}

		public override bool Check()
		{
            foreach (TriggerCondition condition in _Conditions)
            {
                bool flag = condition.Check();
                if (flag)
                {
                    return !_IsNot;
                }
            }
            return _IsNot;
        }

		public override string ToString()
		{
            string text = "Or: ";
            foreach (TriggerCondition condition in _Conditions)
            {
                text = text + "\n    " + condition;
            }
            return text;
        }
	}
}
