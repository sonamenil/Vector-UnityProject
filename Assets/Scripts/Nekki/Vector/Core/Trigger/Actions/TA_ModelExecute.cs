using System.Xml;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Models;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_ModelExecute : TriggerAction
	{
		private Variable _AnimVar;

		private Variable _FrameVar;

		private TA_ModelExecute(TA_ModelExecute p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _AnimVar = p_copyAction._AnimVar;
            _FrameVar = p_copyAction._FrameVar;
        }

		public TA_ModelExecute(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            InitActionVar(p_parent.ParentTrigger, ref _AnimVar, p_node.Attributes["AnimName"].Value);
			InitActionVar(p_parent.ParentTrigger, ref _FrameVar, p_node.Attributes["AnimFrame"].Value);
        }

		public override void Activate(ref bool p_isRunNext)
		{
            p_isRunNext = true;
			string[] reactions = {
				_AnimVar.ValueString,
				_FrameVar.ValueInt.ToString()
			};
			foreach (var model in _ParentLoop.ParentTrigger.ParentElements.Primitives)
			{
				if (model.Type == ModelType.PrimitiveAnimated)
				{
					((PrimitiveAnimatedRunner)model).PlayReaction(reactions);
				}
			}
        }

		public override TriggerAction Copy()
		{
			return new TA_ModelExecute(this);
		}

		public override string ToString()
		{
            return "ModelExecute AnimName" + _AnimVar.DebugStringValue + " AnimFrame" + _FrameVar.DebugStringValue;
        }
    }
}
