using System;
using Nekki.Vector.Core.Models;
using UnityEngine;

namespace Nekki.Vector.Core.Location
{
	public abstract class ItemRunner : Runner
	{
		protected int _Type;

		protected float _Radius = 80;

		protected bool _inclusion;

		protected Color _DefaultColor = Color.white;

		protected Color _CurrentColor = Color.white;

		public int Type => _Type;

		public override bool Render()
		{
			return true;
		}

		public abstract void Init();

		public ItemRunner(float x, float y, int type)
			: base(x, y)
		{
			_Type = type;
			_TypeClass = RunnerType.Platform;
		}

		public override void Move(Point shift)
		{
			base.Move(shift);
		}

		public virtual void Collision(ModelHuman modelHuman)
		{
			if (!_inclusion)
			{
				if (!modelHuman.IsBot)
				{
					if (Inside(modelHuman.ModelObject.CenterOfMassNode.Start))
					{
						Play();
						_inclusion = true;
						AddBonus(modelHuman);
					}
				}
			}
		}

		public virtual void AddBonus(ModelHuman modelHuman)
		{
		}

		public bool Inside(Vector3d vector)
		{
			return Math.Sqrt((Position.X - vector.X) * (Position.X - vector.X) + (Position.Y - vector.Y) * (Position.Y - vector.Y)) < _Radius;
		}

		public override void Reset()
		{
			base.Reset();
			_inclusion = false;
		}

		public virtual void Play()
		{
		}
	}
}
