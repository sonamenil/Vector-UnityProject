using Nekki.Vector.Core.Models;
using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_Kill : TriggerAction
	{
		private Variable _ModelVar;

		public TA_Kill(TA_Kill p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _ModelVar = p_copyAction._ModelVar;
        }

        public TA_Kill(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            InitActionVar(p_parent.ParentTrigger, ref _ModelVar, p_node.Attributes["Model"].Value);
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
            ModelHuman model = GetModel(_ModelVar.ValueString);
            if (model != null)
            {
                model.StartPhysics();
            }
        }

		public override TriggerAction Copy()
		{
            return new TA_Kill(this);
        }

        public override string ToString()
		{
            return "Kill Model=" + _ModelVar.DebugStringValue;
        }
    }
}
