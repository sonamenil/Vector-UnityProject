using Nekki.Vector.Core.Camera;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Utilites;
using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Visual
{
	public class VisualContainer
	{
        public float FrameScale
        {
            get
            {
                return _Scale * 2;
            }
        }

        private float _Scale;

        private GameObject _Object;

		private List<VisualRunner> _RenderVisual = new List<VisualRunner>();

		private float _Factor;

		private float _GameLayerScale;

		private List<string> _ToBatch = new List<string>();

		public GameObject Object => _Object;

		public List<VisualRunner> RenderVisual => _RenderVisual;

		public float Factor => _Factor;

		public VisualContainer(float factor, float z)
		{
			_Factor = factor;
			_Object = new GameObject("[Visual Container] " + _Factor);
			var vector = _Object.transform.localPosition;
			vector.z = z;
			_Object.transform.localPosition = vector;
		}

		public VisualContainer(float factor, GameObject container)
		{
			_Factor = factor;
			_Object = container;
		}

		public void Init()
		{

		}

		public void Add(VisualRunner Runner)
		{
			if (Runner != null)
			{
				if (Runner.IsVanishing)
				{
					return;
				}
				_RenderVisual.Add(Runner);
			}
		}

		public void Render()
		{
            _Scale = Math.Round(1 / ((1 / LocationCamera.CurrentZoom - 1) * _Factor + 1), 10);
            _Object.transform.localScale = new Vector3(FrameScale, FrameScale, 1);
            if (LocationCamera.Current != null)
            {
                Move(new Vector3(LocationCamera.Current.X, LocationCamera.Current.Y));
                return;
            }
            Move(Vector3.zero);
        }

        public void Move(Vector3 p_point)
        {
            if (p_point == null)
            {
                return;
            }
            var vector = _Object.transform.position;
            vector.x = p_point.x + -(p_point.x * (_Factor) * FrameScale);
            vector.y = p_point.y + -(p_point.y * (_Factor) * FrameScale);
            _Object.transform.position = vector;
        }

        public void Reset()
		{
		}

		public void Bounds(out float MinX, out float MaxX, out float MinY, out float MaxY)
		{
			MinX = default(float);
			MaxX = default(float);
			MinY = default(float);
			MaxY = default(float);
		}
	}
}
