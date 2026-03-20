namespace Nekki.Vector.Core.Gadgets
{
	public abstract class Gadget
	{
		private bool _isActive;

		private GadgetType _gadgetType;

		public GadgetType gadgetType => _gadgetType;

		protected Gadget(GadgetType gadgetType)
		{
			_gadgetType = gadgetType;
		}

		public abstract bool IsCanUse();

		public virtual void Play()
		{
			_isActive = true;
		}

		public virtual void Stop()
		{
			_isActive = false;
        }
    }
}
