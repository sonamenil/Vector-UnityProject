using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_ShowLog : TriggerAction
	{
		private TA_ShowLog(TA_ShowLog p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
		}

		public TA_ShowLog(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
		}

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
        }

        public override TriggerAction Copy()
		{
            return new TA_ShowLog(this);
        }

        private void GetLogString(ref string result)
		{
		}

		public override string ToString()
		{
            return "ShowLog";
        }
    }
}
