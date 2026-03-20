using System.Xml;

namespace Nekki.Vector.Core.Trigger.Conditions
{
	public class TC_Less : TC_Compare
	{
		public TC_Less(TriggerLoop p_parent, XmlNode p_node)
			: base(p_parent, p_node)
		{
		}

		public override bool Check()
		{
            if (_IsNot)
            {
                return !_Value1.isLess(_Value2);
            }
            return _Value1.isLess(_Value2);
        }

		public override string ToString()
		{
            string text = "Less: " + ((!_IsNot) ? "(" : "!(");
            string text2 = text;
            return text2 + _Value1.ToString() + "<" + _Value2.ToString() + ")";
        }
	}
}
