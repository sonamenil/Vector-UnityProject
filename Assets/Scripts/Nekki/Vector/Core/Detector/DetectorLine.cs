using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Node;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Nekki.Vector.Core.Detector
{
	public class DetectorLine
	{
		public enum DetectorType
		{
			Vertical,
			Horizontal
		}

		private GameObject _layer;

		private Vector3d lineDirection = new Vector3d();

		public GameObject Layer
		{
			get
			{
				return _layer;
			}
			set
			{
				_layer = value;
			}
		}

		public ModelNode Node
		{
			get;
		}

		public QuadRunner Platform
		{
			get
			{
				return Node.Data;
			}
			set
			{
				Node.Data = value;
			}
		}

		public string Name => Node.Name;

		public Vector3d Position => Node.Start;

		public DetectorType Type
		{
			get;
		}

		public Vector3d DeltaValue
		{
			get;
		}

		public bool Safe
		{
			get;
			set;
		}

		public int Side
		{
			get;
			set;
		}

		public Vector3dLine End
		{
			get;
		}

		public Vector3dLine Start
		{
			get;
		}

		public Vector3dLine Perpendicular
		{
			get;
		}

		public DetectorLine(ModelNode node, DetectorType type)
		{
            Node = node;
            Start = new Vector3dLine(node.Start, node.Start);
            End = new Vector3dLine(node.Start, node.Start);
            Perpendicular = new Vector3dLine(node.Start, node.Start);
            Type = type;
			DeltaValue = new Vector3d();
        }

		public void Reset()
		{
            Start.Set(Node.Start, Node.Start);
            End.Set(Node.Start, Node.Start);
            Perpendicular.Set(Node.Start, Node.Start);
        }

		public Vector3d Subtract()
		{
            return Node.Start - Node.End;
        }

        public void Delta(float value)
		{
            switch (Type)
            {
                case DetectorType.Horizontal:
                    DeltaValue.X = value; 
					break;
                case DetectorType.Vertical:
					DeltaValue.Y = value;
					break;
			}
		}

		public void Update()
		{
            End.Set(Start.Start, Start.End);
            Start.Set(Node.Start, Node.Start);
            Start.SetZerroOnZ();
            Start.Start.Subtract(DeltaValue);
            Start.End.Add(DeltaValue);
			lineDirection.Set(End.End);
			lineDirection.Subtract(End.Start);
            Perpendicular.Start = Vector3d.Closest(Start.Start, End.Start, lineDirection);
			lineDirection.Set(End.Start);
			lineDirection.Subtract(End.End);
            Perpendicular.End = Vector3d.Closest(Start.End, End.End, lineDirection);
        }

		public void DeltaPosition(Vector3d delta)
		{
            Node.Start.Add(delta);
            Node.EndAssignStart();
        }

		public string GetStringType()
		{
            return (Type != 0) ? "H" : "V";
        }
    }
}
