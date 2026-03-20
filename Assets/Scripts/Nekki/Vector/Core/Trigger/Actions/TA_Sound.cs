using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_Sound : TriggerAction
	{
		private Variable _SoundVar;

		private TA_Sound(TA_Sound p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
			_SoundVar = p_copyAction._SoundVar;
		}

		public TA_Sound(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
			InitActionVar(p_parent.ParentTrigger, ref _SoundVar, p_node.Attributes["Name"].Value);
		}

		public override void Activate(ref bool p_isRunNext)
		{
			p_isRunNext = true;
			var sounds = _SoundVar.ValueString.Split('|');
			SoundsManager.Instance.PlaySounds(sounds[UnityEngine.Random.Range(0, sounds.Length - 1)]);
		}

		public override TriggerAction Copy()
		{
			return new TA_Sound(this);
		}

		public override string ToString()
		{
			return "Sound:" + _SoundVar.DebugStringValue;
		}
	}
}
