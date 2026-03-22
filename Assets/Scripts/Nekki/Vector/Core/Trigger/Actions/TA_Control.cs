using System.Xml;
using Nekki.Vector.Core.Models;

namespace Nekki.Vector.Core.Trigger.Actions
{
    public class TA_Control : TriggerAction
    {
        private Variable _ModelNameVar;

        private Variable _SwitchVar;

        private TA_Control(TA_Control p_copyAction)
            : base(p_copyAction._ParentLoop)
        {
            _ModelNameVar = p_copyAction._ModelNameVar;
            _SwitchVar = p_copyAction._SwitchVar;
        }

        public TA_Control(XmlNode p_node, TriggerLoop p_parent)
            : base(p_parent)
        {
            string value = p_node.Attributes["Model"].Value;
            string value2 = p_node.Attributes["Switch"].Value;
            InitActionVar(p_parent.ParentTrigger, ref _SwitchVar, value2);
            InitActionVar(p_parent.ParentTrigger, ref _ModelNameVar, value);
        }

        public override void Activate(ref bool p_isRunNext)
        {
            p_isRunNext = true;
            string valueString = _ModelNameVar.ValueString;
            ModelHuman model = GetModel(valueString);
            if (model != null)
            {
                model.ControllerKeys.Enable = _SwitchVar.ValueString == "On";
                model.ControllerKeys.ClearAnimation();
            }
        }

        public override TriggerAction Copy()
        {
            return new TA_Control(this);
        }

        public override string ToString()
        {
            return "Control: Model=" + _ModelNameVar.DebugStringValue + " Switch:" + _SwitchVar.DebugStringValue;
        }
    }
}
