using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;

namespace Nekki.Vector.Core.Gadgets
{
	public class GadgetSlowTime : Gadget
	{
		public GadgetSlowTime()
			: base(GadgetType.SlowTime)
		{ }

        public override bool IsCanUse()
		{
			return true;
		}

		public override void Play()
		{
		}

		private void SlowTime()
		{
		}
	}
}
