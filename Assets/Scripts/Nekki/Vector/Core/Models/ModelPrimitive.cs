using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Node;
using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Models
{
	public class ModelPrimitive : Model
	{
		private bool _IsStrike;

		private List<string> _Sounds;

		private PrimitiveRunner _Base;

		public float Impulse
		{
			get;
			set;
		}

		public int RenderTime
		{
			get;
			set;
		}

		public int RenderCount
		{
			get;
			set;
		}

		public virtual bool IsStrike
		{
			get
			{
				return _IsStrike;
			}
			set
			{
				_IsStrike = value;
				if (value)
				{
					_ControllerPhysics.Start();
					RenderTime = RenderCount;
				}
			}
		}

		public override Rectangle Rectangle
		{
			get
			{
				var rectangle = base.Rectangle;
				var node = _ModelObject.GetNode();
				rectangle.Origin.Add((float)node.Start.X, (float)node.Start.Y);
				return rectangle;
			}
		}

		public bool IsCollisible => _ControllerPhysics.IsPhysics ? RenderCount == 0 : false;

		public ModelPrimitive(PrimitiveRunner primitiveRunner, List<string> skins, List<string> sounds)
			: base(skins, ModelType.Primitive)
		{
			_Sounds = sounds;
			_Base = primitiveRunner;
			RenderCount = 150;
			Init();
		}

		public override void Strike(ModelLine edge, Vector3d point, Vector3d impulse)
		{
			IsStrike = true;
			PlaySound();
			_controllerStrike.Striking(edge, point, impulse * Impulse);
		}

		private void PlaySound()
		{
			int num = Random.Range(0, _Sounds.Count);
			SoundsManager.Instance.PlaySounds(_Sounds[num]);
		}
	}
}
