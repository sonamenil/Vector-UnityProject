using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_EndGame : TriggerAction
	{
		private Variable _TimeVar;

		private Variable _ResultVar;

		private Variable _ModelVar;

		private TA_EndGame(TA_EndGame p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _ResultVar = p_copyAction._ResultVar;
            _TimeVar = p_copyAction._TimeVar;
            _ModelVar = p_copyAction._ModelVar;
        }

		public TA_EndGame(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            string value = p_node.Attributes["Result"].Value;
            string p_nameOrValue = "80";
            if (p_node.Attributes["Frames"] != null)
            {
                p_nameOrValue = p_node.Attributes["Frames"].Value;
            }
            string value2 = p_node.Attributes["Model"].Value;
            InitActionVar(p_parent.ParentTrigger, ref _ResultVar, value);
            InitActionVar(p_parent.ParentTrigger, ref _TimeVar, p_nameOrValue);
            InitActionVar(p_parent.ParentTrigger, ref _ModelVar, value2);
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
            string valueString = _ModelVar.ValueString;
            switch (_ResultVar.ValueString)
            {
                case "Win":
                    LevelMainController.current.Win(GetModel(valueString), _TimeVar.ValueInt / 60f);
                    break;
                case "Loss":
                    LevelMainController.current.Loss(GetModel(valueString), _TimeVar.ValueInt / 60f);
                    break;
                case "Death":
                    LevelMainController.current.Death(GetModel(valueString), _TimeVar.ValueInt / 60f);
                    break;
            }
        }

		public override TriggerAction Copy()
		{
            return new TA_EndGame(this);
        }

        public override string ToString()
		{
            return "EndGame Model=" + _ModelVar.DebugStringValue + " Result=" + _ResultVar.DebugStringValue + " Frames" + _TimeVar.DebugStringValue;
        }
    }
}
