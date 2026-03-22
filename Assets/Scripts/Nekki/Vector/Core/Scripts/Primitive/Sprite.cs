using UnityEngine;

namespace Nekki.Vector.Core.Scripts.Primitive
{
	public class Sprite : MonoBehaviour
	{
		private Color _Color;

		private Texture2D _Texture;

		private static Shader _Shader;

		private Material _Material;

		public Color Color
		{
			get => default;
			set
			{
			}
		}

		public Texture2D Texture
		{
			get => null;
			set
			{
			}
		}

		private static Shader Shader => null;

		private void Start()
		{
		}
	}
}
