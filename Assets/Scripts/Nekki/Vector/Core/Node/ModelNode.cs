using Nekki.Vector.Core.Location;
using UnityEngine;

namespace Nekki.Vector.Core.Node
{
    public class ModelNode : Vector3dLine
    {
        private string _name;

        private int _bothIndex;

        private string _bothName;

        private readonly Vector3d _defaultPosition;

        private QuadRunner _Data;

        private Vector3d _timeStepVector = new Vector3d();

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                string[] array = _name.Split('_');
                _bothName = _name.Split('_')[0];
                if (array.Length == 2 && int.TryParse(array[1], out int val))
                {
                    _bothIndex = val;
                }
                else
                {
                    _bothIndex = 0;
                }
            }
        }

        public int Id
        {
            get;
            set;
        }

        public bool IsFixed
        {
            get;
            set;
        }

        public bool IsNodeFixed
        {
            get
            {
                return !IsFixed && IsType;
            }
        }

        public int Radius
        {
            get;
            set;
        }

        public bool IsPhysics
        {
            get;
            set;
        }

        public bool IsNodePhysics
        {
            get
            {
                return IsPhysics && IsType;
            }
        }

        public double Weight
        {
            get;
            set;
        }

        public double Attenuation
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool IsType => Type == "Node";

        public MacroNode MacroNode
        {
            get;
            set;
        }

        public bool IsCollisible
        {
            get;
            set;
        }

        public bool IsDetector
        {
            get;
            set;
        }

        public int BothIndex => _bothIndex;

        public string BothName => _bothName;

        public QuadRunner Data
        {
            get
            {
                return _Data;
            }
            set
            {
                _Data = value;
            }
        }

        public ModelNode(Vector3d position, MacroNode macroNode = null) : base(position, position)
        {
            _defaultPosition = new Vector3d(position);
            MacroNode = macroNode;
            _Data = null;
        }

        public void Reset()
        {
            Start.Set(_defaultPosition);
            End.Set(_defaultPosition);
            _Data = null;
        }

        public void Position(Vector3d end, Vector3d start)
        {
            End.Set(end);
            Start.Set(start);
        }

        public void PositionStart(Vector3d vector)
        {
            Start.Set(vector);
        }

        public void PositionStart(Vector3 vector)
        {
            Start.Set(vector);
        }

        public void PositionStart(double x, double y, double z)
        {
            Start.Set(x, y, z);
        }

        public void PositionStartNoRound(double x, double y, double z)
        {
            Start.Set(x, y, z);
        }

        public void PositionEnd(Vector3d vector)
        {
            End.Set(vector);
        }

        public void PositionEnd(double x, double y, double z)
        {
            End.Set(x, y, z);
        }

        public void EndAssignStart()
        {
            End.X = Start.X;
            End.Y = Start.Y;
            End.Z = Start.Z;
        }

        public void MacroNodeCompute()
        {
            if (MacroNode != null)
            {
                Start.Reset();
                End.Reset();
                for (int i = 0; i < MacroNode.ChildNode.Count; i++)
                {
                    Start.Add(MacroNode.ChildNode[i].Start, MacroNode.LCC[i]);
                    End.Add(MacroNode.ChildNode[i].End, MacroNode.LCC[i]);
                }
            }
        }

        public void TimeStep(double gravity)
        {
            _timeStepVector.Set(Start.X - End.X, Start.Y - End.Y, Start.Z - End.Z);
            if (IsPhysics)
            {
                _timeStepVector *= 1f - Attenuation;
            }
            _timeStepVector.Add(Start);
            _timeStepVector.Y += gravity;
            EndAssignStart();
            PositionStart(_timeStepVector);
        }
    }
}
