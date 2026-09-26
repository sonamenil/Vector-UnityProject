using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Trigger.Events;
using UnityEngine;

namespace Nekki.Vector.Core.Trigger
{
	public class TriggerTimer
	{
		private bool _IsActiv;

		private int _Counter;

		private TriggerRunner _Parent;

        private TextMesh label;

		public TriggerTimer(TriggerRunner p_parent)
		{
            _Parent = p_parent;
        }

        public void Start(int p_value)
		{
            RunnerRender.AddRunner(_Parent);
            _Counter = p_value;
            _IsActiv = true;

            if (Game.Instance.SnailSett.ShowTriggers && QuadsRenderer.Instance != null)
            {
                label = QuadsRenderer.Instance.AddLabel(new Vector3(_Parent.rectangle.MidX, _Parent.rectangle.MidY, -14), "");
            }
        }

		public bool Render()
		{
            if (_IsActiv)
            {
                if (label != null)
                {
                    label.text = _Counter.ToString();
                }
                _Counter--;
                if (_Counter <= 0)
                {
                    if (label != null)
                    {
                        QuadsRenderer.Instance.RemoveLabel(label);
                    }
                    _IsActiv = false;
                    TE_Timeout p_event = new TE_Timeout();
                    ModelHuman modelByName = LevelMainController.current.Location.GetModelByName(_Parent.ModelVar.ValueString);
                    _Parent.CheckEvent(p_event, modelByName);
                    return !_IsActiv;
                }
                return false;
            }
            return true;
        }

		public void Reset()
		{
            _IsActiv = false;

            if (label != null)
            {
                QuadsRenderer.Instance.RemoveLabel(label);
            }
		}
	}
}
