using System.Collections.Generic;

namespace Nekki.Vector.Core.Transformation
{
    public class TransformManager
    {
        private List<TransformSystem> _Storage = new List<TransformSystem>();

        private List<TransformSystem> _FlushStorage = new List<TransformSystem>();

        public void Clear()
        {
            foreach (var t in _Storage)
            {
                t.Reset();
            }
            _Storage.Clear();
            _FlushStorage.Clear();
        }

        public void Add(TransformSystem system)
        {
            if (!_Storage.Contains(system))
            {
                foreach (var t in system.Storage)
                {
                    if (t.Type == Type.Move)
                    {
                        t.Runner.AddMoveSystem((MoveTransform)t);
                        t.Runner.TransformationStart();
                    }
                    if (t.Type == Type.Scale)
                    {
                        t.Runner.TransformationStart();
                    }
                }
                _Storage.Add(system);
            }

        }

        public void Remove(TransformSystem system)
        {
            foreach (var t in system.Storage)
            {
                if (t.Type == Type.Move)
                {
                    t.Runner.RemoveMoveSystem((MoveTransform)t);
                }
            }
            _FlushStorage.Add(system);
        }

        public List<TransformSystem> Find(string name)
        {
            List<TransformSystem> list = new List<TransformSystem>();
            for (int i = 0; i < _Storage.Count; i++)
            {
                var t = _Storage[i];
                if (t.Name == name)
                {
                    list.Add(t);
                }
            }
            return list;
        }

        public void Update()
        {
            for (int i = 0; i < _Storage.Count; i++)
            {
                if (!_Storage[i].Update())
                {
                    Remove(_Storage[i]);
                }
            }
            if (_FlushStorage.Count == 0)
            {
                return;
            }
            for (int i = 0; i < _FlushStorage.Count; i++)
            {
                _FlushStorage[i].Parent.TransformationEnd();
                _Storage.Remove(_FlushStorage[i]);
            }
            _FlushStorage.Clear();
        }

        public void RemoveEndedTransformation()
        {
            if (_FlushStorage.Count == 0)
            {
                return;
            }
            for (int i = 0; i < _FlushStorage.Count; i++)
            {
                if (_FlushStorage[i].Parent != null)
                {
                    _FlushStorage[i].Parent.TransformationEnd();

                }
                _Storage.Remove(_FlushStorage[i]);
            }
            _FlushStorage.Clear();
        }
    }
}
