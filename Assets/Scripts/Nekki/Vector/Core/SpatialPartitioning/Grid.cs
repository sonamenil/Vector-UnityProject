using Nekki.Vector.Core.Location;
using UnityEngine;
using System.Collections.Generic;

namespace Nekki.Vector.Core.SpatialPartitioning
{
    public class Grid
    {
        public static class Cell
        {
            public static float Width = 200;

            public static float Height = 200;
        }

        private Dictionary<QuadRunner, HashSet<HashSet<QuadRunner>>> _Cells = new Dictionary<QuadRunner, HashSet<HashSet<QuadRunner>>>();

        private Columns _Columns = new Columns();

        private HashSet<QuadRunner> _Transformation = new HashSet<QuadRunner>();

        public HashSet<QuadRunner> Transformation => _Transformation;

        private RectangleInt Bounds(List<QuadRunner> p_quads)
        {
            return null;
        }

        private RectangleInt Bounds(Rectangle rectangle)
        {
            if (rectangle == null) return null;

            int minX = Mathf.FloorToInt(rectangle.MinX / Cell.Width);
            int minY = Mathf.FloorToInt(rectangle.MinY / Cell.Height);
            int maxX = Mathf.FloorToInt(rectangle.MaxX / Cell.Width);
            int maxY = Mathf.FloorToInt(rectangle.MaxY / Cell.Height);

            return new RectangleInt(minX, minY, maxX, maxY);
        }

        public void Prepare(RectangleInt p_rectangle)
        {
            _Columns.Prepare(p_rectangle);
        }

        public void Add(List<QuadRunner> p_quads)
        {
            foreach (QuadRunner q in p_quads)
            {
                Add(q);
            }
        }

        public void Add(QuadRunner p_quad)
        {
            if (_Cells.ContainsKey(p_quad))
            {
                return;
            }
            p_quad.OnTransformationStart += OnTransformationStart;
            p_quad.OnTransformationEnd += OnTransformationEnd;

            var rectangle = Bounds(p_quad.rectangle);
            _Cells.Add(p_quad, _Columns.Add(p_quad, rectangle));
        }

        public void Remove(List<QuadRunner> p_quads)
        {
            foreach (var quad in p_quads)
            {
                quad.OnTransformationStart -= OnTransformationStart;
                quad.OnTransformationEnd -= OnTransformationEnd;
                Remove(quad);
            }
        }

        public void Remove(QuadRunner p_quad)
        {
            if (!_Cells.ContainsKey(p_quad))
            {
                return;
            }
            foreach (var hash in _Cells[p_quad])
            {
                hash.Remove(p_quad);
            }
            _Cells.Remove(p_quad);
        }

        public void GetAll(Rectangle p_rectangle, HashSet<QuadRunner> culledQuads)
        {
            culledQuads.UnionWith(_Transformation);
            _Columns.Get(Bounds(p_rectangle), culledQuads);
        }

        protected void Get(RectangleInt p_rectangle, HashSet<QuadRunner> culledQuads)
        {
            _Columns.Get(p_rectangle, culledQuads);
        }

        public void OnTransformationStart(QuadRunner p_quad)
        {
            Remove(p_quad);
            _Transformation.Add(p_quad);
        }

        public void OnTransformationEnd(QuadRunner p_quad)
        {
            _Transformation.Remove(p_quad);
            Add(p_quad);
        }

        public void Reset()
        {
            foreach (var quad in _Transformation)
            {
                Add(quad);
            }
            _Transformation.Clear();
        }
    }
}
