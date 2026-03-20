using Nekki.Vector.Core.Utilites;
using System.Xml;
using UnityEngine;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_MessageOnScreen : TriggerAction
	{
		public enum Animation
		{
			Fade
		}

		private Variable _TextVariable;

		private Variable _TimeVariable;

		private Color _Color;

		private Animation _AppearAnimation;

		private Animation _DisappearAnimation;

		private static Animation GetTypeByString(string value)
		{
			return default(Animation);
		}

		private TA_MessageOnScreen(TA_MessageOnScreen p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
			_TextVariable = p_copyAction._TextVariable;
			_TimeVariable = p_copyAction._TimeVariable;
			_AppearAnimation = p_copyAction._AppearAnimation;
			_DisappearAnimation = p_copyAction._DisappearAnimation;
		}

		public TA_MessageOnScreen(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
			_AppearAnimation = Animation.Fade;
			_DisappearAnimation = Animation.Fade;
			_Color = ColorUtils.FromHex(p_node.Attributes["Color"].Value);
			InitActionVar(p_parent.ParentTrigger, ref _TextVariable, p_node.Attributes["Text"].Value);
			InitActionVar(p_parent.ParentTrigger, ref _TimeVariable, p_node.Attributes["Frames"].Value);
		}

		public override void Activate(ref bool p_isRunNext)
		{
			p_isRunNext = true;
			LevelMainController.current.levelSceneController.MessageOnScreen(_TextVariable.ValueString, _TimeVariable.ValueInt, _Color, _AppearAnimation, _DisappearAnimation);
		}

		public override TriggerAction Copy()
		{
			return new TA_MessageOnScreen(this);
		}

		public override string ToString()
		{
			return "MessageOnScreen";
        }
	}
}
