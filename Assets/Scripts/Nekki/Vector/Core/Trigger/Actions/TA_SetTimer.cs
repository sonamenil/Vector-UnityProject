using System.Xml;

namespace Nekki.Vector.Core.Trigger.Actions
{
	public class TA_SetTimer : TriggerAction
	{
		private Variable _FramesVar;

		private TA_SetTimer(TA_SetTimer p_copyAction)
			: base(p_copyAction._ParentLoop)
		{
            _FramesVar = p_copyAction._FramesVar;
        }

        public TA_SetTimer(XmlNode p_node, TriggerLoop p_parent)
			: base(p_parent)
		{
            InitActionVar(p_parent.ParentTrigger, ref _FramesVar, p_node.Attributes["Frames"].Value);
        }

        public override void Activate(ref bool p_isRunNext)
        {
            p_isRunNext = true;
            _ParentLoop.ParentTrigger.SetTimer(_FramesVar.ValueInt);
        }

		public override TriggerAction Copy()
		{
            return new TA_SetTimer(this);
        }

        public override string ToString()
		{
            return "SetTimer Frames=" + _FramesVar.DebugStringValue;
        }
    }
}
