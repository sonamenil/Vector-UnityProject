using Nekki.Vector.Core.Camera;
using UnityEngine;


namespace Nekki.Vector.Core.Scripts
{
    [ExecuteAlways]
    public class Parallax : MonoBehaviour
    {
        protected Transform _CachedTransform;

        [SerializeField]
        protected float _FactorX;

        [SerializeField]
        protected float _FactorY;

        protected Vector3 _StartPosition;

        protected Vector3 _LastCameraPosition;

        protected Vector3 _LastCameraOffset;

        public void SetFactor(float factorX, float factorY)
        {
            _FactorX = factorX;
            _FactorY = factorY;
        }


        protected virtual void Start()
        {
            _CachedTransform = base.transform;
            _StartPosition = _CachedTransform.position;
            if (UnityEngine.Camera.main != null && Mathf.Abs(_FactorX) > float.Epsilon)
            {
                Vector3 position = UnityEngine.Camera.main.transform.position;
                float p_x = _FactorX * position.x;
                float p_y = _FactorY * position.y;
                _LastCameraPosition = new Vector3f(p_x, p_y, 0f);
            }
            _CachedTransform.position = _StartPosition + _LastCameraPosition;
        }

        protected virtual void UpdatePosition()
        {
            Vector3 position = UnityEngine.Camera.main.transform.position;
            float num = position.x - (_FactorX * position.x * transform.localScale.x);
            float num2 = position.y - (_FactorY * position.y * transform.localScale.y);
            Vector3 vector = new Vector3(num - _LastCameraPosition.x, num2 - _LastCameraPosition.y);
            Vector3 vector3 = _CachedTransform.parent.InverseTransformVector(vector);
            _CachedTransform.localPosition += vector3;
            _LastCameraPosition = new Vector3(num, num2);
            _LastCameraOffset = vector;
        }

        private void FixedUpdate()
        {
            if (UnityEngine.Camera.main != null && Mathf.Abs(_FactorX) > float.Epsilon)
            {
                UpdatePosition();
            }
        }
    }

}

