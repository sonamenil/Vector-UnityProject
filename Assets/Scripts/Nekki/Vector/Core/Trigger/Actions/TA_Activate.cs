using System.Xml;
using Nekki.Vector.Core.Location.LevelCreation;
using Nekki.Vector.Core.Trigger.Events;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_Activate : TriggerAction
	{
		private Variable _ActivationID;

		private TA_Activate(TA_Activate p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
			_ActivationID = p_copyAction._ActivationID;
		}

		public TA_Activate(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            string value = p_node.Attributes["ActionID"].Value;
            InitActionVar(p_parent.ParentTrigger, ref _ActivationID, value);
        }

		public override void Activate(ref bool p_isRunNext)
		{
			p_isRunNext = true;
			ActivEvents(_ParentLoop.ParentTrigger.ParentElements.ParentObject);
		}

		private void ActivEvents(BaseObjectRunner p_object)
		{
			TE_Activate @event = new TE_Activate(_ActivationID.ValueString);
			foreach (var trigger in p_object.Element.Triggers)
			{
				trigger.CheckEvent(@event, GetModel());
			}
			foreach (var child in p_object.Childs)
			{
				ActivEvents(child);
			}
		}

		public override TriggerAction Copy()
		{
            return new TA_Activate(this);
        }

        public override string ToString()
		{
            return "Activate ID=" + _ActivationID.DebugStringValue;
        }
    }
}
