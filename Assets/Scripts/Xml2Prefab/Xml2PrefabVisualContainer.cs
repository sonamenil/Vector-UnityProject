using UnityEngine;

namespace Xml2Prefab
{
    [ExecuteInEditMode]
    public class Xml2PrefabVisualContainer : MonoBehaviour
    {
        [SerializeField]
        private float _factor;

        public float Factor => _factor;

        public void Init(float factor)
        {
            _factor = factor;
        }

#if UNITY_EDITOR
        //private float _Scale;

        //public float FrameScale
        //{
        //    get
        //    {
        //        return _Scale * 2;
        //    }
        //}

        //void Update()
        //{
        //    if (Application.isPlaying)
        //    {
        //        return;
        //    }
        //    _Scale = Nekki.Vector.Core.Utilites.Math.Round(1 / ((1 / 0.5f - 1) * _factor + 1), 10);
        //    transform.localScale = new Vector3(FrameScale, FrameScale, 1);
        //    if (FindObjectOfType<Xml2PrefabCameraContainer>() != null)
        //    {
        //        Move(FindObjectOfType<Xml2PrefabCameraContainer>().transform.localPosition);
        //        return;
        //    }
        //    Move(new Vector3(0, 0, transform.position.z));
        //}

        //public void Move(Vector3 p_point)
        //{
        //    if (p_point == null)
        //    {
        //        return;
        //    }
        //    var vector = transform.position;
        //    vector.x = p_point.x + -(p_point.x * (_factor) * FrameScale);
        //    vector.y = -(p_point.y + -(p_point.y * (_factor) * FrameScale));
        //    transform.position = vector;
        //}
#endif
    }
}
