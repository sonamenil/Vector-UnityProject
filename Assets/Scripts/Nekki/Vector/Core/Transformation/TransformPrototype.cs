using Nekki.Vector.Core.Location;
using System.Xml;

namespace Nekki.Vector.Core.Transformation
{
	public abstract class TransformPrototype
	{
		protected Runner _Runner;

		protected bool _Pause = false;

		protected Type _Type;

		public virtual Runner Runner
		{
			get
			{
				return _Runner;
			}
			set
			{
				_Runner = value;
			}
		}

		public bool Pause
		{
			get
			{
				return _Pause;
			}
			set
			{
				_Pause = value;
			}
		}

		public Type Type => _Type;

		public bool IsMoveType => _Type == Type.Move;

		public abstract int Frames
		{
			get;
		}

		public abstract void Parse(XmlNode Node);

		public abstract bool Iteration();

		public abstract void CalcDelta();

		public abstract TransformPrototype Clone();

		public virtual void Reset()
		{
			_Pause = false;
		}
	}
}
