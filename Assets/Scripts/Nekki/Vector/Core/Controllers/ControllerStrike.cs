using System;
using System.Collections.Generic;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;

namespace Nekki.Vector.Core.Controllers
{
	public class ControllerStrike
	{
		private Model _Model;

		private ModelObject _ModelObject;

		private List<ModelNode> _Nodes;

		private List<ModelLine> _Edges;

		public ControllerStrike(Model model)
		{
			_Model = model;
			_ModelObject = _Model.ModelObject;
			_Nodes = _ModelObject.CollisibleNodes;
			_Edges = _ModelObject.CollisibleEdges;
		}

		public void Striking(ModelLine line, Vector3d point, Vector3d impulse)
		{
            Decrease();
            ModelNode start = line.Start;
            ModelNode end = line.End;
            if (!start.IsFixed || !end.IsFixed)
            {
                Vector3d start2 = start.Start;
                Vector3d start3 = end.Start;
                double num = Vector3d.Factor(point, start2, start3);
                Vector3d vector3d = new Vector3d(1f - num, 1f - num, 0f);
                Vector3d vector3d2 = new Vector3d(num, num, 0f);
                double slowModeValue = LevelMainController.current.slowModeFrames;
                if (!start.IsFixed)
                {
                    double p_x = start2.X + vector3d.X * impulse.X / Math.Sqrt(slowModeValue) / start.Weight;
                    double p_y = start2.Y + vector3d.Y * impulse.Y / Math.Sqrt(slowModeValue) / start.Weight;
                    start2.Set(p_x, p_y, start2.Z);
                }
                if (!end.IsFixed)
                {
                    double p_x2 = start3.X + vector3d2.X * impulse.X / Math.Sqrt(slowModeValue) / end.Weight;
                    double p_y2 = start3.Y + vector3d2.Y * impulse.Y / Math.Sqrt(slowModeValue) / end.Weight;
                    start3.Set(p_x2, p_y2, start3.Z);
                }
            }
        }
		

		public void Decrease()
		{
            foreach (ModelNode node in _Nodes)
            {
                node.End.X = (node.End.X + node.Start.X) * 0.5f;
                node.End.Y = (node.End.Y + node.Start.Y) * 0.5f;
                node.End.Z = (node.End.Z + node.Start.Z) * 0.5f;
            }
        }
	}
}
