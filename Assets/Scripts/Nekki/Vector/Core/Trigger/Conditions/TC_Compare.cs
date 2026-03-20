using System.Xml;

namespace Nekki.Vector.Core.Trigger.Conditions
{
	public class TC_Compare : TriggerCondition
	{
		protected Variable _Value1;

		protected Variable _Value2;

		public TC_Compare(TriggerLoop p_parent, XmlNode p_node)
			: base(p_parent, p_node)
		{
			_Type = ConditionsType.TCT_None;
            Parse(p_node);
        }

        protected void Parse(XmlNode p_node)
		{
            _Value1 = GetOrCreateVar(p_node.Attributes["Value1"] == null ? p_node.Attributes["Value"].Value : p_node.Attributes["Value1"].Value);
            _Value2 = GetOrCreateVar(p_node.Attributes["Value2"] == null ? p_node.Attributes["Than"].Value : p_node.Attributes["Value2"].Value);
        }

        public override bool Check()
		{
			return false;
		}
	}
}
