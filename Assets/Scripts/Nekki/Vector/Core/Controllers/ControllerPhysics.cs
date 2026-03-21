using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;
using System.Collections.Generic;

namespace Nekki.Vector.Core.Controllers
{
	public class ControllerPhysics
	{
		private int _Iterative = 1;

		private ModelObject _ModelObject;

		private bool _IsPhysics;

		private double _SlowMode = 1;

		private bool[] SArray;

		private bool[] EArray;

		private List<ModelNode> _Nodes;

		private List<ModelLine> _Edges;

		private List<ModelLine> _EdgesPhysic;

        private static float _Gravity = 0.4000000059604645f;


        public ModelObject ModelObject => _ModelObject;

		public bool IsPhysics => _IsPhysics;

		public double SlowMode => _SlowMode;

		public static double Gravity => _Gravity / LevelMainController.current.slowModeFrames;

		public ControllerPhysics(ModelObject Object)
		{
			_ModelObject = Object;
			_Nodes = Object.NodesAll;
			_Edges = Object.EdgesAll;
			_EdgesPhysic = Object.EdgesPhysic;
		}

		public void Render()
		{
			TimeStep();
			Iterative();
			NodeReset();
		}

		public void Start()
		{
			_IsPhysics = true;
		}

		public void Stop()
		{
            _IsPhysics = false;
        }

        public void TimeStep()
		{
			ModelNode modelNode = null;
			for (int i = 0; i < _Nodes.Count; i++)
			{
				modelNode = _Nodes[i];
				if (!modelNode.IsFixed && (_IsPhysics || modelNode.IsPhysics))
				{
					modelNode.TimeStep(Gravity);
				}
			}
		}

		public void Iterative()
		{
			if (!_IsPhysics)
			{
				foreach (var edge in _EdgesPhysic)
				{
					IterativeLine(edge, _IsPhysics);
				}
			}
			else
			{
				foreach (var edge in _Edges)
				{
					IterativeLine(edge, _IsPhysics);
				}
			}
		}

		public void IterativeLine(ModelLine line, bool isPhysics)
		{
            if (isPhysics || line.Start.IsPhysics || line.End.IsPhysics)
			{
                Vector3d p_vector = line.Iterative(line.Start.Start);
                Vector3d p_vector2 = line.Iterative(line.End.Start);
                IterativeNode(p_vector, line.Start, isPhysics);
				IterativeNode(p_vector2, line.End, isPhysics);
			}

        }

		public void IterativeNode(Vector3d vector, ModelNode node, bool isPhysics)
		{
            if (!node.IsType || node.IsFixed || (!isPhysics && !node.IsPhysics))
            {
				return;
            }
            node.PositionStart(vector);
        }

		public void NodeReset()
		{
			if (!_IsPhysics)
			{
				return;
			}
			_ModelObject.PositionToPivot(_ModelObject.DetectorHorizontalNode);
			_ModelObject.PositionToPivot(_ModelObject.DetectorVerticalNode);
			_ModelObject.PositionToPivot(_ModelObject.CameraNode.Node);
			_ModelObject.PositionToPivot(_ModelObject.CenterOfMassNode);
		}
	}
}
