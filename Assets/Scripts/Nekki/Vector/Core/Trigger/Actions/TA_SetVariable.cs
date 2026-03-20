using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_SetVariable : TriggerAction
	{
		private Variable _SetVar;

		private Variable _ValueVar;

		private TA_SetVariable(TA_SetVariable p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _SetVar = p_copyAction._SetVar;
            _ValueVar = p_copyAction._ValueVar;
        }

		public TA_SetVariable(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            string value = p_node.Attributes["Name"].Value;
            string value2 = p_node.Attributes["Value"].Value;
            if (value2[0] == '_')
            {
                _ValueVar = _ParentLoop.GetParentVar(value2);
                if (_ValueVar == null)
                {
                    DebugUtils.Dialog("Error create Action SetVariable no found : " + value2, true);
                }
            }
            else
            {
                _ValueVar = Variable.createVariable(value2, string.Empty, p_parent.ParentTrigger);
            }
            if (value[0] == '?')
            {
                _SetVar = Variable.createVariable(value, string.Empty, p_parent.ParentTrigger);
            }
            else
            {
                _SetVar = _ParentLoop.GetParentVar("_" + value);
            }
            if (_SetVar == null)
            {
                DebugUtils.Dialog("Error create Action SetVariable not found =" + value, true);
            }
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
            switch (_ValueVar.Type)
            {
                case VariableTypeE.VT_DOUBLE:
                case VariableTypeE.VT_FUNCTION:
                case VariableTypeE.VT_EXPRESSION:
                    _SetVar.setValue(_ValueVar.ValueFloat);
                    break;
                case VariableTypeE.VT_STRING:
                    _SetVar.setValue(_ValueVar.ValueString);
                    break;
                default:
                    _SetVar.setValue(_ValueVar.ValueInt);
                    break;
            }
        }

		public override TriggerAction Copy()
		{
            return new TA_SetVariable(this);
        }

        public override string ToString()
		{
            return "SetVariable Var:" + _SetVar.ToString() + " Value=" + _ValueVar.DebugStringValue;
        }
    }
}
