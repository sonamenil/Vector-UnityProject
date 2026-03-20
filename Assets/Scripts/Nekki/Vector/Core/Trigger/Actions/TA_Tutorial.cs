using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_Tutorial : TriggerAction
	{
		private TA_Tutorial(TA_Tutorial p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
		}

		public TA_Tutorial(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{

		}

		public override void Activate(ref bool p_isRunNext)
		{
			p_isRunNext = true;
		}

		public override TriggerAction Copy()
		{
			return new TA_Tutorial(this);
		}

		public override string ToString()
		{
			return "Tutorial";
		}
	}
}
