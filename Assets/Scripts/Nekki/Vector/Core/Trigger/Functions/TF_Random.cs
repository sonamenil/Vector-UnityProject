using Nekki.Vector.Core.Location;
using UnityEngine;

namespace Nekki.Vector.Core.Trigger.Functions
{
	public class TF_Random : TriggerFunction
	{
		private Variable _Min;

		private Variable _Max;

		public override int ValueInt => 0;

		public override float ValueFloat => 0f;

		public override string ValueString => null;

		public override string DebugStringValue => null;

		public TF_Random(string p_value, string p_name, TriggerRunner p_parent)
			: base(p_name, p_parent)
		{
			Debug.LogError("RANDOM USE?");
		}
	}
}
