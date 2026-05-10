using Nekki.Vector.Core.Scripts;
using UnityEngine;
using System.IO;
using System;
using Core._Common;

namespace Nekki.Vector.Core.Location.Animation
{
	public class AnimationRendering : Runner
	{
		private int _CurrentFrame;

		private bool _Replay;

		protected bool _IsPlay;

		private int _CountHalf;

		private int _TotalHalf;

		public string Path = "";

		protected float _Width;

		protected float _Height;

		protected int _TotalFrames;

		protected AnimationSprite Animator;

        public AnimationRendering(string name, float X = 0f, float Y = 0f, float width = 100f, float height = 100f, bool replay = false, int speed = 1)
			: base(X, Y)
		{
			Name = name;
			_Width = width;
			_Height = height;
			_Replay = replay;
			_TotalHalf = speed;
		}

		public virtual void Init(float pivotX = 0, float pivotY = 1)
		{
			var render = _UnityObject.AddComponent<SpriteRenderer>();
			render.flipY = true;
            Animator = _UnityObject.AddComponent<AnimationSprite>();
			string path = System.IO.Path.Combine(VectorPaths.AnimatedTextures, _Name);
			Animator.Init(path, render, pivotX, pivotY);
            _TotalFrames = Animator.TotalFrames;
			IsEnabled = false;
		}

		public override bool Render()
		{
			if (_IsPlay)
			{
				if (_CountHalf < _TotalHalf)
				{
					_CountHalf++;
					return false;
				}
				Animator.SetSpriteFrame(_CurrentFrame);
				_CountHalf = 0;
				_CurrentFrame++;
				if (_CurrentFrame < _TotalFrames)
				{
					return false;
				}
				if (_Replay)
				{
					_CurrentFrame = 0;
					return false;
				}
				Stop();
			}
			return true;
		}

		public override void Reset()
		{
			base.Reset();
			_CurrentFrame = 0;
			_CountHalf = 0;
			_IsPlay = false;
			Animator.SetSpriteFrame(0);
		}

		public virtual void Stop(bool value = false)
		{
			_IsPlay = false;
			if (value)
			{
				IsEnabled = false;
			}
		}

		public void PlayFrom(int value)
		{
			_CurrentFrame = value;
			_CountHalf = _TotalHalf;
			Render();
			_IsPlay = true;
		}
	}
}
