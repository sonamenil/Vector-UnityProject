using Nekki.Vector.Core.Models;
using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_ForceAnimation : TriggerAction
	{
		private Variable _FramesVar;

		private Variable _NameVar;

		private Variable _ModelNameVar;

		private Variable _DirectionVar;

		private TA_ForceAnimation(TA_ForceAnimation p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _FramesVar = p_copyAction._FramesVar;
            _NameVar = p_copyAction._NameVar;
            _ModelNameVar = p_copyAction._ModelNameVar;
            _DirectionVar = p_copyAction._DirectionVar;
        }

		public TA_ForceAnimation(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            InitActionVar(p_parent.ParentTrigger, ref _FramesVar, XmlUtils.ParseString(p_node.Attributes["Frame"], "-1"));
            InitActionVar(p_parent.ParentTrigger, ref _NameVar, XmlUtils.ParseString(p_node.Attributes["Name"]));
            InitActionVar(p_parent.ParentTrigger, ref _ModelNameVar, XmlUtils.ParseString(p_node.Attributes["Model"]));
            InitActionVar(p_parent.ParentTrigger, ref _DirectionVar, XmlUtils.ParseString(p_node.Attributes["Reversed"], "0"));
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
            string valueString = _ModelNameVar.ValueString;
            ModelHuman model = GetModel(valueString);
            if (model != null)
            {
                model.PlayAnimation(_NameVar.ValueString, _DirectionVar.ValueInt == 1, _FramesVar.ValueInt);
            }
        }

		public override TriggerAction Copy()
		{
            return new TA_ForceAnimation(this);
        }

        public override string ToString()
		{
            return "ForceAnimation Model=" + _ModelNameVar.DebugStringValue + " Name=" + _NameVar.DebugStringValue + " Frame=" + _FramesVar.DebugStringValue + " Revers=" + _DirectionVar.DebugStringValue;
        }
    }
}
