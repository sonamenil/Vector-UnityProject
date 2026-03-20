using System;
using System.Xml;
using Nekki.Vector.Core.Utilites;
using UnityEngine;

namespace Nekki.Vector.Core.Location
{
    public abstract class MatrixSupport : Runner
    {
        private bool _IsSerialized;

        protected GameObject _Support;

        protected Matrix4x4 _Transformation = Matrix4x4.identity;

        public override GameObject UnityObject
        {
            get
            {
                if (_Support == null)
                {
                    return _UnityObject;
                }
                return _Support;
            }
        }

        public MatrixSupport(float x, float y, XmlNode node)
            : base(x, y)
        {
            ParseTransform(node);
        }

        private void ParseTransform(XmlNode matrixNode)
        {
            if (matrixNode != null)
            {
                _Transformation[0, 0] = float.Parse(matrixNode.Attributes["A"].Value);
                _Transformation[0, 1] = float.Parse(matrixNode.Attributes["B"].Value);
                _Transformation[1, 0] = float.Parse(matrixNode.Attributes["C"].Value);
                _Transformation[1, 1] = float.Parse(matrixNode.Attributes["D"].Value);
                _Transformation[2, 2] = 1f;
                _Transformation[3, 3] = 1f;
            }
        }

        public override void Generate(GameObject existRunner)
        {
            base.Generate(existRunner);
            if (_UnityObject.transform.childCount > 0)
            {
                var support = _UnityObject.transform.GetChild(0);
                if (support != null && support.name == "Support")
                {
                    _Support = support.gameObject;
                    _IsSerialized = true;
                }
            }
        }

        protected override void GenerateObject()
        {
            base.GenerateObject();
            if (!Matrix.IsIdentity(_Transformation))
            {
                Matrix4x4 transpose = _Transformation.transpose;
                QRDecomposition qRDecomposition = new QRDecomposition(transpose);
                if (qRDecomposition.ContainsSkew())
                {
                    _Support = new GameObject
                    {
                        name = "Support"
                    };
                    _Support.transform.SetParent(_CachedTransform, false);
                    return;
                }
                Matrix4x4 rotation = qRDecomposition.Rotation;
                Quaternion quaternion = default(Quaternion);
                int num = ((rotation[0, 0] * rotation[1, 1] - rotation[0, 1] * rotation[1, 0] > 0f) ? 1 : (-1));
                quaternion = ((num >= 0) ? Quaternion.LookRotation(rotation.GetColumn(2), rotation.GetColumn(1)) : Quaternion.LookRotation(-rotation.GetColumn(2), rotation.GetColumn(1)));
                _CachedTransform.localRotation = quaternion;
                _CachedTransform.localScale = new Vector3(qRDecomposition.ScaleX, qRDecomposition.ScaleY, 1f);
            }

        }

        protected virtual void Transform()
        {
            if (_Support != null && !_IsSerialized)
            {
                AffineDecomposition affineDecomposition = new AffineDecomposition(_Transformation);
                _Support.transform.localScale = new Vector3(affineDecomposition.ScaleX1, affineDecomposition.ScaleY1, 1f);
                _Support.transform.Rotate(0f, 0f, affineDecomposition.Angle1);
                if (!float.IsNaN(affineDecomposition.Angle2) && !float.IsNaN(affineDecomposition.ScaleX2) && !float.IsNaN(affineDecomposition.ScaleY2))
                {
                    _CachedTransform.localScale = new Vector3(affineDecomposition.ScaleX2, affineDecomposition.ScaleY2, 1f);
                    _CachedTransform.Rotate(0f, 0f, affineDecomposition.Angle2);
                }
            }

        }
    }
}
