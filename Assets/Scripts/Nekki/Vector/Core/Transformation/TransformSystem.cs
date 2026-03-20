using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Location.LevelCreation;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace Nekki.Vector.Core.Transformation
{
    public class TransformSystem
    {
        private List<TransformPrototype> _Storage = new List<TransformPrototype>();

        private int _CurrentSystem = 0;

        private Runner _Parent;

        private BaseObjectRunner _MainParent;

        private bool _IsPaused;

        private string _Name;

        private bool _IsSystemTemplate = false;

        private bool _FirstIteration = true;

        public List<TransformPrototype> Storage => _Storage;

        public Runner Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                _Parent = value;
                foreach (var t in _Storage)
                {
                    t.Runner = _Parent;
                }
            }
        }

        public BaseObjectRunner MainParent
        {
            get
            {
                return _MainParent;
            }
            set
            {
                _MainParent = value;
            }
        }

        public bool IsPaused
        {
            get
            {
                return _IsPaused;
            }
            set
            {
                _IsPaused = value;
                if (_MainParent == null)
                {
                    return;
                }
                var list = LevelMainController.current.Location.transformManager.Find(_Name);
                foreach (var t in list)
                {
                    t.IsPaused = _IsPaused;
                }
                list.Clear();
            }
        }

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public bool IsSystemTemplate
        {
            get
            {
                return _IsSystemTemplate;
            }
            set
            {
                _IsSystemTemplate = value;
            }
        }

        public int FramesCount
        {
            get
            {
                int frames = 0;
                foreach (TransformPrototype p in _Storage)
                {
                    frames += p.Frames;
                }
                return frames;
            }
        }

        public static TransformSystem Create(XmlNode p_node)
        {
            if (p_node == null)
            {
                return null;
            }
            var t = new TransformSystem();
            if (t.Parse(p_node))
            {
                return t;
            }
            return null;
        }

        private bool Parse(XmlNode node)
        {
            _Name = node.Attributes["Name"].Value;
            foreach (XmlNode transformNode in node.ChildNodes)
            {
                switch (transformNode.Name)
                {
                    case "Move":
                        var move = MoveTransform.Create(transformNode);
                        if (move != null)
                            _Storage.Add(move);
                        break;
                    case "Color":
                        var color = ColorTransform.Create(transformNode);
                        if (color != null)
                            _Storage.Add(color);
                        break;
                    case "Size":
                        var size = SizeTransform.Create(transformNode);
                        if (size != null)
                            _Storage.Add(size);
                        break;
                    case "Rotation":
                        var rotate = RotateTransform.Create(transformNode);
                        if (rotate != null)
                            _Storage.Add(rotate);
                        break;
                }
            }
            return true;
        }

        public void Reset()
        {
            _FirstIteration = true;
            _CurrentSystem = 0;
            _IsPaused = true;
            foreach (var t in _Storage)
            {
                t.Reset();
            }
        }

        public bool Update()
        {
            if (!_IsPaused)
            {
                int num = 0;
                foreach (var t in _Storage)
                {
                    if (_FirstIteration)
                    {
                        t.CalcDelta();
                    }
                    if (!t.Iteration())
                    {
                        num++;
                    }
                }
                if (_FirstIteration)
                {
                    _FirstIteration = false;
                }
                return _Storage.Count != 0 && num != _Storage.Count;
            }
            return true;
            
        }

        public int Run()
        {
            IsPaused = false;
            if (_MainParent == null)
            {
                LevelMainController.current.Location.transformManager.Add(this);
            }
            else
            {
                RunTransformOnObjectRunner(_MainParent);
            }
            return FramesCount;
        }

        public void RunTransformOnObjectRunner(BaseObjectRunner p_object)
        {

            foreach (var runner in p_object.Element.Runners)
            {
                var t = Clone();
                t.Parent = runner;
                t.IsSystemTemplate = true;
                LevelMainController.current.Location.transformManager.Add(t);
            }
            foreach (var child in p_object.Childs)
            {
                RunTransformOnObjectRunner(child);
            }
        }

        public TransformSystem Clone()
        {
            var t = new TransformSystem();
            t.Name = Name;
            t.IsPaused = IsPaused;
            foreach (var transform in _Storage)
            {
                t.Storage.Add(transform.Clone());
            }
            return t;
        }
    }
}
