using System.Collections.Generic;
using UnityEngine;

namespace Nekki.Vector.Core.Detector
{
    public class DetectorRender
    {
        private GameObject _layer;

        private bool _isDebug;

        private readonly List<GameObject> _lines = new List<GameObject>();

        public Color Color
        {
            get;
            set;
        }

        public string Name
        {
            get => gObject.name;
            set => gObject.name = value;
        }

        public GameObject Layer
        {
            get => _layer;
            set
            {
                _layer = value;
                gObject.transform.SetParent(!(_layer != null) ? null : _layer.transform, false);
            }
        }

        public bool IsDebug
        {
            get => false;
            set
            {
            }
        }

        public GameObject gObject
        {
            get;
        }

        public Edge Edge
        {
            get;
        }

        public DetectorRender()
        {
            Color = new Color(0, 0, 0, 1);
            gObject = new GameObject("Detector");
            gObject.AddComponent<Scripts.Geometry.Edge>();
            gObject.SetActive(false);
        }

        public void Add(Vector3dLine line)
        {
            line.Stroke = 1.75f;
            line.Color = Color;
            GameObject gameObject = new GameObject("Line");
            Scripts.Geometry.Edge edge = gameObject.AddComponent<Scripts.Geometry.Edge>();
            edge.Base = line;
            edge.SortingOrder = 0;
            _lines.Add(gameObject);
            gameObject.transform.parent = gObject.transform;
        }
    }
}
