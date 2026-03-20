using Nekki.Vector.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Nekki.Vector.Core.Location.LevelCreation
{
    public abstract class BaseObjectRunner
    {
        protected BaseObjectRunner _Parent;

        protected List<BaseObjectRunner> _Childs = new List<BaseObjectRunner>();

        protected Point _Position = new Point(0, 0);

        protected Point _LocalPosition;

        protected GameObject _UnityObject;

        protected Transform _CachedTransform;

        protected Dictionary<string, TransformSystem> _TransformationDictionary = new Dictionary<string, TransformSystem>();

        protected List<TransformSystem> _TransformationData = new List<TransformSystem>();

        public List<object> LayerOrder = new List<object>();

        protected float _Factor;

        protected string _Name;

        protected BaseElements _Element;

        protected bool _IsInitialized;

        private GameObject _Layer;

        protected bool _IsEnabled = true;

        protected bool _debugMode = true;

        public List<BaseObjectRunner> Childs => _Childs;

        public Point Position => _Position;

        public GameObject UnityObject => _UnityObject;

        public Transform CachedTransform => _CachedTransform;

        public List<TransformSystem> TransformationData => _TransformationData;

        public float Factor
        {
            get
            {
                return _Factor;
            }
            set
            {
                _Factor = value;
            }
        }

        public BaseElements Element => _Element;

        public bool IsInitialized
        {
            get
            {
                return _IsInitialized;
            }
            set
            {
                _IsInitialized = value;
            }
        }

        public GameObject Layer => _Layer;

        public virtual bool IsEnabled
        {
            get
            {
                return _IsEnabled;
            }
            set
            {
                _IsEnabled = value;
                foreach (var element in _Element.Runners)
                {
                    element.IsEnabled = value;
                }
            }
        }

        public bool DebugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                _debugMode = value;
                foreach (var child in _Childs)
                {
                    child.DebugMode = value;
                }
                foreach (var element in _Element.Runners)
                {
                    element.IsDebug = value;
                }
            }
        }

        public uint Index
        {
            get;
            protected set;
        }

        public void SetLayer(GameObject parent)
        {
            _Layer = parent;
            _CachedTransform.parent = _Layer.transform;
            if (!Xml2PrefabRoot.Serialize)
            {
                _CachedTransform.localPosition = Vector3.zero;
            }
            else
            {
                _CachedTransform.localPosition = _LocalPosition;
            }
            foreach (var obj in LayerOrder)
            {
                if (obj is BaseObjectRunner child)
                {
                    child.SetLayer(_UnityObject);
                }
                if (obj is Runner runner)
                {
                    runner.Layer = _UnityObject;
                }
            }
            foreach (var child in _Childs)
            {
                child.SetLayer(_UnityObject);
            }
            foreach (var element in _Element.Runners)
            {
                element.Layer = _UnityObject;
            }
        }

        protected BaseObjectRunner(uint index, BaseObjectRunner parent)
        {
            Index = index;
            _Parent = parent;
        }

        public virtual void Init()
        {
            InitInnerGo(CreateObject());
            InitElements();
            foreach (var child in _Childs)
            {
                BuildTranformationTable(child.TransformationData);
            }
            BuildTranformationTable(TransformationData);
            _IsInitialized = true;
        }

        protected void InitInnerGo(GameObject go)
        {
            if (_UnityObject == null)
            {
                _UnityObject = go;
                _CachedTransform = _UnityObject.transform;
                _CachedTransform.position = Vector3.zero;
            }
        }

        protected virtual GameObject CreateObject()
        {
            if (string.IsNullOrEmpty(_Name))
            {
                _Name = "[Object Holder]";
            }
            return new GameObject(_Name);
        }

        protected virtual void InitElements()
        {
        }

        public virtual void ParseTransformationData(XmlNode node)
        {
            if (node == null || node["Dynamic"] == null)
            {
                return;
            }
            foreach (XmlNode node2 in node["Dynamic"])
            {
                if (node2.Name == "Transformation")
                {
                    var system = TransformSystem.Create(node2);
                    system.MainParent = this;
                    AddTransformationData(system);
                }
            }
        }

        public void BuildTranformationTable(List<TransformSystem> systems)
        {
            foreach (TransformSystem system in systems)
            {
                _TransformationDictionary[system.Name] = system;
            }
        }

        public int RunTranformation(string name)
        {
            if (_TransformationDictionary.ContainsKey(name))
            {
                _TransformationDictionary[name].Reset();
                return _TransformationDictionary[name].Run();
            }
            int num2 = 0;
            for (int j = 0; j < _Childs.Count; j++)
            {
                int num3 = _Childs[j].RunTranformation(name);
                if (num3 != 0)
                {
                    num2 = Math.Max(num3, num2);
                }
            }
            return num2;
        }

        public int GetTransformationFrame(string name)
        {
            if (_TransformationDictionary.ContainsKey(name))
            {
                return _TransformationDictionary[name].FramesCount;
            }
            int num2 = 0;
            for (int j = 0; j < _Childs.Count; j++)
            {
                int transformationFrame = _Childs[j].GetTransformationFrame(name);
                if (transformationFrame != 0)
                {
                    num2 = Math.Max(transformationFrame, num2);
                }
            }
            return num2;
        }

        protected void AddTransformationData(TransformSystem p_data)
        {
            if (_TransformationData == null)
            {
                _TransformationData = new List<TransformSystem>();
            }
            _TransformationData.Add(p_data);
        }
    }
}
