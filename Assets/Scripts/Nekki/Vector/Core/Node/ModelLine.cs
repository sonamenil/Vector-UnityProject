using UnityEngine;

namespace Nekki.Vector.Core.Node
{
	public class ModelLine
	{
		private string _name;

		public ModelNode Start
		{
			get;
		}

		public ModelNode End
		{
			get;
		}

		public Vector3dLine LineCurrent
		{
			get;
		}

		public Vector3dLine LinePrevious
		{
			get;
		}

		public virtual string Name
		{
			get => _name;
			set => _name = value;
		}

		public Color Color
		{
			get;
			set;
		}

		public double Length
		{
			get;
			set;
		}

		private double RealLength => Vector3d.Distance(Start.Start, End.Start);

		private double ScaleLength => Length / RealLength;

		public int SubType
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public double Mass
		{
			get;
		}

		public double Margin1
		{
			get;
			set;
		}

		public double Margin2
		{
			get;
			set;
		}

		public bool Collisible
		{
			get;
			set;
		}

		public double Stroke
		{
			get;
			set;
		}

		public Vector3d Center => Vector3d.Middle(Start.Start, End.Start);

		public ModelLine(ModelNode node1, ModelNode node2)
		{
            LineCurrent = new Vector3dLine(node1.Start, node2.Start);
            LinePrevious = new Vector3dLine(node1.End, node2.End);
            Start = node1;
            End = node2;
            Mass = 1f / (Start.Weight + End.Weight);
        }

		public ModelLine(ModelLine modelLine)
		{
            Name = modelLine.Name;
            Start = modelLine.Start;
            End = modelLine.End;
            Stroke = modelLine.Stroke;
            Color = modelLine.Color;
            Length = modelLine.Length;
            SubType = modelLine.SubType;
            Type = modelLine.Type;
            Margin1 = modelLine.Margin1;
            Margin2 = modelLine.Margin2;
        }

		public Vector3d Iterative(Vector3d vector)
		{
			Vector3d vector3f = (Start.Start * Start.Weight + End.Start * End.Weight) * Mass;
			Vector3d vector3f2 = (vector - vector3f) * ScaleLength;
			return vector3f + vector3f2;

        }
	}
}
