using UnityEngine;
using UnityEngine.UI;

namespace Nekki.Vector.GUI.Scenes.Run
{
    public class FPSMeter : MonoBehaviour
    {
        [SerializeField]
        private float _UpdateInterval = 0.2f;

        [SerializeField]
        private Text _Label;

        private float _UpdateTimeout;

        private int _LastFramesCount;

        private float _LastTime;

        public static float FPS { get; private set; }

        private void Start()
        {
            SetTime();
        }

        private void Update()
        {
            _UpdateTimeout -= Time.deltaTime;
            if (_UpdateTimeout <= 1E-06f)
            {
                CalculateFps();
            }
        }

        private void SetTime()
        {
            _UpdateTimeout = _UpdateInterval;
            _LastFramesCount = Time.frameCount;
            _LastTime = Time.realtimeSinceStartup;
        }

        private void CalculateFps()
        {
            int num = Time.frameCount - _LastFramesCount;
            float num2 = Time.realtimeSinceStartup - _LastTime;
            SetTime();
            FPS = num / num2;
            _Label.text = string.Format("FPS: {0:F1}", FPS);
        }

    }
}
