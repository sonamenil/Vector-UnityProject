using Nekki.Vector.Core.Location;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Nekki.Vector.Core.Trigger
{
    public class TriggerExpression : Variable
    {
        private ExprParser _Parser;

        private float _Result;

        private bool _IsCalculated;

        public override int ValueInt
        {
            get
            {
                CalculateExpression();
                return (int)_Result;
            }
        }

        public override float ValueFloat
        {
            get
            {
                CalculateExpression();
                return _Result;
            }
        }

        public TriggerExpression(string p_value, string p_name, TriggerRunner p_parent) : base(p_value, p_name, p_parent)
        {
            type = VariableTypeE.VT_EXPRESSION;
            p_value = p_value.Replace("{", string.Empty);
            p_value = p_value.Replace("}", string.Empty);
            p_value = p_value.Replace(" ", string.Empty);
            _Parser = ExprParser.Create(p_value, p_parent);
            if (_Parser == null)
            {
                Debug.LogError("TRIGGER EXPRESSION: expression is incorrect!");
            }
            else if (_Parser.RequiredVariables.Count == 0)
            {
                _IsCalculated = _Parser.TryGetResult(ref _Result);
            }
            else
            {
                InitVariables();
            }
        }

        private void InitVariables()
        {
            if (_Parser.RequiredVariables.Count == 0)
            {
                return;
            }
            foreach (string requiredVariable in _Parser.RequiredVariables)
            {
                Variable p_var;
                if (GetTypeByValue(requiredVariable) == VariableTypeE.VT_FUNCTION)
                {
                    p_var = createVariable(requiredVariable, string.Empty, parent);
                    _Parser.AddVariable(requiredVariable, p_var);
                    continue;
                }
                p_var = parent.GetVar(requiredVariable);
                if (p_var != null)
                {
                    _Parser.AddVariable(requiredVariable, p_var);
                }
                else
                {
                    Debug.LogError("In Expression {" + strValue + "} unknown var= " + requiredVariable);
                }
            }
            _Parser.RequiredVariables.Clear();
        }

        public void CalculateExpression()
        {
            if (!_IsCalculated)
            {
                InitVariables();
                _IsCalculated = _Parser.TryGetResult(ref _Result);
            }
        }
    }
}
