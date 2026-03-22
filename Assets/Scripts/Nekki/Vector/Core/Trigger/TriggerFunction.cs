using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Trigger.Functions;

namespace Nekki.Vector.Core.Trigger
{
	public class TriggerFunction : Variable
	{
		protected TriggerFunction(string p_name, TriggerRunner p_parent) : base(p_name, p_parent)
		{
			type = VariableTypeE.VT_FUNCTION;
		}

		public static TriggerFunction Create(string p_value, string p_name, TriggerRunner p_parent)
		{
            string text = p_value.Substring(0, p_value.IndexOf("["));
            if (text.IndexOf("getModel") != -1)
            {
                return new TF_Model(p_value, p_name, p_parent);
            }
            if (text.IndexOf("Random") != -1)
            {
                return new TF_Random(p_value, p_name, p_parent);
            }
            return null;
        }

		public static void InitFuncVar(TriggerRunner p_parent, ref Variable p_var, string p_name)
		{
            if (p_name.Length != 0 && p_name[0] == '_')
            {
                p_var = p_parent.GetVar(p_name);
            }
            else
            {
                p_var = createVariable(p_name, string.Empty, p_parent);
            }
        }
	}
}
