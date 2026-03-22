using System.Collections.Generic;

namespace Nekki.Vector.Core.Trigger
{
	public class ExprToken
	{
		public enum TypeToken
		{
			NOTHING = -1,
			NUMBER,
			VARIABLE,
			OPERATOR,
			UNKNOWN
		}

		public enum OperatorID
		{
			ADD,
			SUB,
			MULT,
			DIV,
			MOD,
			POW,
			ROOT,
			BRACK_L,
			BRACK_R,
			UNKONOWN
		}

		private Variable _Var;

		private TypeToken _Type;

		private OperatorID _Operator;

		private int _Priority;

		public static Dictionary<OperatorID, int> OPERATOR_PRIORITY = new Dictionary<OperatorID, int>();

        public Variable Var => _Var;

        public OperatorID Operator => _Operator;

        public TypeToken Type => _Type;

        public int OperatorPriority
        {
            get => OPERATOR_PRIORITY[_Operator] + _Priority * OPERATOR_PRIORITY[OperatorID.BRACK_L];
            set => _Priority = value;
        }

        public static ExprToken create(Variable p_var, TypeToken p_type, OperatorID p_operator = OperatorID.UNKONOWN)
		{
            ExprToken exprToken = new ExprToken();
            exprToken._Var = p_var;
            exprToken._Type = p_type;
            exprToken._Operator = p_operator;
            return exprToken;
        }

		public static ExprToken create(Variable p_var)
		{
            ExprToken exprToken = new ExprToken();
            exprToken._Var = p_var;
            exprToken._Type = TypeToken.NUMBER;
            exprToken._Operator = OperatorID.UNKONOWN;
            return exprToken;
        }

		private ExprToken()
		{
            _Type = TypeToken.NOTHING;
            _Operator = OperatorID.UNKONOWN;
            _Priority = 1;
            _Var = null;
        }

		public void reset(Variable p_var, TypeToken p_type, OperatorID p_operator = OperatorID.UNKONOWN)
		{
            _Var = p_var;
            _Type = p_type;
            _Operator = p_operator;
        }

		public void reset(Variable p_var)
		{
            _Var = p_var;
            _Type = TypeToken.NUMBER;
            _Operator = OperatorID.UNKONOWN;
        }
	}
}
