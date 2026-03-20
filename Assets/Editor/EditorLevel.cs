using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Xml2Prefab
{
    [ExecuteAlways]
    public class EditorLevel : MonoBehaviour
    {
        static EditorLevel Current;
        private void Awake()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.EnteredPlayMode)
            {
                Current = this;
                DontDestroyOnLoad(this);

            }
            Current = null;
        }
    }
}

