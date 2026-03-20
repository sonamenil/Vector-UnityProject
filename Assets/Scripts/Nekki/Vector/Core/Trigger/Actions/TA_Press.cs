using Nekki.Vector.Core.Controllers;
using Nekki.Vector.Core.Models;
using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_Press : TriggerAction
	{
		private Variable _ModelNameVar;

		private Variable _KeyVar;

		private TA_Press(TA_Press p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _KeyVar = p_copyAction._KeyVar;
            _ModelNameVar = p_copyAction._ModelNameVar;
        }

		public TA_Press(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            InitActionVar(p_parent.ParentTrigger, ref _KeyVar, p_node.Attributes["Key"].Value);
            InitActionVar(p_parent.ParentTrigger, ref _ModelNameVar, p_node.Attributes["Model"].Value);
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
            string valueString = _ModelNameVar.ValueString;
            ModelHuman model = GetModel(valueString);
            if (model != null)
            {
                model.ControllerKeys.SetKeyVariable_force(new KeyVariables(_KeyVar.ValueString));
            }
        }

		public override TriggerAction Copy()
		{
            return new TA_Press(this);
        }

        public override string ToString()
		{
            return "Press Model=" + _ModelNameVar.DebugStringValue + " Key=" + _KeyVar.DebugStringValue;
        }
    }
}
