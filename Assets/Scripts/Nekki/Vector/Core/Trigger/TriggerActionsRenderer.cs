using Nekki.Vector.Core.Location;
using System.Collections.Generic;

namespace Nekki.Vector.Core.Trigger
{
    public class TriggerActionsRenderer
    {
        private static TriggerActionsRenderer _Current;

        private TriggerRunner _Parent;

        private List<List<TriggerAction>> _DelayActions = new List<List<TriggerAction>>();

        public static TriggerActionsRenderer Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new TriggerActionsRenderer();
                }
                return _Current;
            }
        }

        public static void Reset()
        {
            Current._DelayActions.Clear();
        }

        public void FreeMemory()
        {
            _Current = null;
        }

        public void AddActions(List<TriggerAction> p_actions)
        {
            int count = p_actions.Count;
            for (int i = 0; i < count; i++)
            {
                bool isRunNext = true;
                p_actions[i].Activate(ref isRunNext);
                if (!isRunNext)
                {
                    CopyActionToDelay(p_actions, i, count);
                    break;
                }
            }
        }

        public void CopyActionToDelay(List<TriggerAction> p_actions, int p_from, int size)
        {
            List<TriggerAction> list = new List<TriggerAction>(size - p_from);
            for (int num = size - 1; num >= p_from; num--)
            {
                list.Add(p_actions[num]);
            }
            _DelayActions.Add(list);
        }

        public void Render()
        {
            if (_DelayActions.Count == 0)
            {
                return;
            }
            for (int i = 0; i < _DelayActions.Count; i++)
            {
                List<TriggerAction> list = _DelayActions[i];
                bool isRunNext = false;
                while (list.Count != 0)
                {
                    TriggerAction triggerRunnerAction = list[list.Count - 1];
                    triggerRunnerAction.Activate(ref isRunNext);
                    if (isRunNext)
                    {
                        list.Remove(triggerRunnerAction);
                        continue;
                    }
                    break;
                }
            }
            for (int num = _DelayActions.Count - 1; num >= 0; num--)
            {
                if (_DelayActions[num].Count == 0)
                {
                    _DelayActions.RemoveAt(num);
                }
            }
        }
    }
}
