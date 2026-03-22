using System.Xml;
using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.Transformation
{
	public abstract class TransformPrototype
	{
		protected Runner _Runner;

		protected bool _Pause;

		protected Type _Type;

		public virtual Runner Runner
		{
			get => _Runner;
			set => _Runner = value;
		}

		public bool Pause
		{
			get => _Pause;
			set => _Pause = value;
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
