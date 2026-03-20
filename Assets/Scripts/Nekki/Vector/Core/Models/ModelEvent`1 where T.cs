namespace Nekki.Vector.Core.Models
{
	public class ModelEvent<T> where T : class
	{
		private T _Data;

		private bool _Bubbles;

		private bool _Cancelable;

		public T Data => _Data;

		public bool Bubbles => _Bubbles;

		public bool Cancelable => _Cancelable;

		public ModelEvent(T Data = null, bool Bubbles = false, bool Cancelable = false)
		{
			_Data = Data;
			_Bubbles = Bubbles;
			_Cancelable = Cancelable;
		}
	}
}
