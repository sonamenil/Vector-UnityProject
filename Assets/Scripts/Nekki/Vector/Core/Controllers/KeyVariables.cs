namespace Nekki.Vector.Core.Controllers
{
	public class KeyVariables
	{
		public const string UP = "Up";

		public const string DOWN = "Down";

		public const string LEFT = "Left";

		public const string RIGHT = "Right";

		public Key Key
		{
			get;
		}

		public KeyVariables(string p_key)
		{
			Key = Parse(p_key);
		}

		public KeyVariables(Key p_key)
		{
			Key = p_key;
		}

		public static Key Parse(string p_key)
		{
			switch (p_key)
			{
				case "Up":
					return Key.Up;
				case "Down":
					return Key.Down;
				case "Left":
					return Key.Left;
				case "Right":
					return Key.Right;
			}
			return Key.None;
		}

		public bool IsEqual(Key p_key)
		{
			return Key == p_key;
		}

		public bool IsEqual(KeyVariables p_keysVariables)
		{
			return Key == p_keysVariables.Key;
		}

		public override string ToString()
		{
			return Key.ToString();
		}
	}
}
