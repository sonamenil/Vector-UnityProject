using System.Xml;

namespace Nekki.Vector.Core.Trigger.Events
{
	public class TE_ChangeVar : TriggerEvent
	{
		private bool _IsInit;

		private Variable _Var;

		private int _OldValue;

		public TE_ChangeVar(XmlNode p_node)
		{
            _Type = EventType.TET_VAR_CHANGE;
            _Var = Variable.createVariable(p_node.Attributes["Value"].Value, string.Empty);
        }

		private void Init()
		{
            _IsInit = true;
            _OldValue = _Var.ValueInt;
        }

		public void Reset()
		{
            _IsInit = false;
        }

		public bool IsChange()
		{
            if (!_IsInit)
            {
                Init();
                return false;
            }
            if (_Parent.ParentTrigger.IsActive)
            {
                return _OldValue != _Var.ValueInt;
            }
            return false;
        }
	}
}
