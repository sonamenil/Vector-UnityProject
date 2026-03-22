using System.Collections.Generic;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;
using Nekki.Vector.Core.Result;
using Collision = Nekki.Vector.Core.Result.Collision;

namespace Nekki.Vector.Core.Controllers
{
	public class ControllerCollisions
	{
		private ModelType _Type;

		private Model _Model;

		private ModelObject _ModelObject;

		private List<ModelNode> _CollisionNodes;

		private List<ModelLine> _CollisionEdges;

		private ControllerPlatforms _controllerPlatforms;

		private ControllerAreas _controllerAreas;

		private ControllerCatching _modelControllerCatching;

		private List<Cross> cachedCrossList = new List<Cross>();

		public ControllerPlatforms ControllerPlatforms => _controllerPlatforms;

		public ControllerCatching ModelControllerCatching => _modelControllerCatching;

		public List<AreaRunner> ActiveAreas => _controllerAreas.ActiveAreas;

		public bool IsPlay => _Model.Type == ModelType.Human && ((ModelHuman)_Model).IsPlay;

        public ControllerCollisions(Model model)
		{
            _Model = model;
            _ModelObject = model.ModelObject;
            _CollisionEdges = _ModelObject.CollisibleEdges;
            _CollisionNodes = _ModelObject.CollisibleNodes;
			_Type = model.Type;
            if (model.Type == ModelType.Human)
            {
                ModelHuman modelHuman = (ModelHuman)model;
                _controllerPlatforms = new ControllerPlatforms(modelHuman);
                _controllerAreas = new ControllerAreas(modelHuman);
				_modelControllerCatching = new ControllerCatching(modelHuman);
            }
        }

		public void Reset()
		{
            if (_Type != ModelType.Human)
            {
                return;
            }
            _controllerPlatforms.Reset();
            _controllerAreas.Reset();
            _modelControllerCatching.Reset();
		}

		public void UpdateQuad(QuadRunner quad, int sign)
		{
            if (quad is TrickAreaRunner trick && IsCollide(trick))
            {
                trick.ExpandTrickAreaOnce(sign);
            }
            UpdateQuad(quad);
		}

		public void UpdateQuad(QuadRunner quad)
		{
            quad.IsRender = false;
            if (!IsCollide(quad.rectangle) || !quad.IsEnabled)
            {
                return;
            }
            ModelHuman modelHuman = (ModelHuman)_Model;
            switch (quad.TypeClass)
            {
                case RunnerType.Trigger:
                    modelHuman.ControllerTrigger.Check((TriggerRunner)quad);
                    return;
                case RunnerType.Area:
                    _controllerAreas.Check((AreaRunner)quad);
                    return;
            }
            if (modelHuman.IsPlay)
            {
                _controllerPlatforms.Render(quad);
                DispatchCollision(quad);
            }
            else
            {
                PushingNodes(quad);
            }
        }

		public void UpdatePlatform(QuadRunner platform)
		{
            if (IsCollide(platform.rectangle))
            {
                if (IsPlay)
                {
                    _controllerPlatforms.Render(platform);
                    DispatchCollision(platform);
                }
                else
                {
                    PushingNodes(platform);
                }
            }
        }

		public void UpdatePrimitive(PrimitiveRunner primitive)
		{
            if (IsCollide(primitive.Rectangle))
            {
                CheckPrimitive(primitive);
            }
		}

		public void CheckModel(ModelHuman modelHuman)
		{
			_modelControllerCatching.Render(modelHuman);
		}

		public void CheckPrimitive(PrimitiveRunner primitive)
		{
            if (primitive.Model.Type == ModelType.Primitive)
            {
                foreach (var edge in _CollisionEdges)
                {
                    var collision = CrossModel(primitive.Model, edge.Start.Start, edge.End.Start);
                    if (collision != null)
                    {
                        var node = _ModelObject.GetNode();
                        var vector = node.Start - node.End;
                        primitive.Model.Strike(collision.Edge, collision.Point, vector);
                        collision.Edge = edge;
                        collision.Model = primitive.Model;
                        _Model.OnCollisionModel(collision);
                        return;
                    }
                }
            }
		}

		public Collision CrossModel(Model model, Vector3d v1, Vector3d v2)
		{
            if (model == null || !model.IsEnabled)
            {
                return null;
            }
            foreach (ModelLine item in model.ModelObject.EdgesAll)
            {
                Vector3d vector3f = Vector3d.Cross(v1, v2, item.Start.Start, item.End.Start);
                if (vector3f == null)
                {
                    continue;
                }
                Collision collision = new Collision();
                collision.Point = vector3f;
                collision.Edge = item;
                return collision;
            }
            return null;
        }

		public void DispatchCollision(QuadRunner platform)
		{
            ModelHuman modelHuman = _Model as ModelHuman;
            if (modelHuman == null || !IsCollide(platform))
            {
                return;
            }
            ModelLine modelLine = null;
            for (int i = 0; i < _CollisionEdges.Count; i++)
            {
                cachedCrossList.Clear();
                modelLine = _CollisionEdges[i];
                platform.CrossByEdge(modelLine.Start.Start, modelLine.End.Start, cachedCrossList);
                if (cachedCrossList.Count != 0)
                {
                    Collision collision = new Collision();
                    collision.Model = _Model;
                    collision.Edge = modelLine;
                    collision.Point = cachedCrossList[0].Point;
                    collision.Platform = platform;
                    Collision p_collision = collision;
                    modelHuman.OnCollisionPlatform(new ModelEvent<Collision>(p_collision));
                    break;
                }
            }
        }

		public void PushingNodes(QuadRunner platform)
		{
            foreach (ModelNode collisionNode in _CollisionNodes)
            {
                cachedCrossList.Clear();
                Vector3dLine vector3fLine = platform.Friction(collisionNode.End, collisionNode.Start, cachedCrossList);
                if (vector3fLine != null)
                {
                    vector3fLine.Start.Z = collisionNode.Start.Z;
                    vector3fLine.End.Z = collisionNode.End.Z;
                    collisionNode.PositionStart(vector3fLine.Start);
                    collisionNode.PositionEnd(vector3fLine.End);
                }
            }
        }

        public static void PushingNode(ModelNode collisionNode, List<QuadRunner> platforms, double restitution)
        {
            foreach (var platform in platforms)
            {
                var crossList = new List<Cross>();
                
                Vector3d velocity = collisionNode.Start - collisionNode.End;
                
                Vector3dLine pushed = platform.Friction(collisionNode.End, collisionNode.Start, crossList);
                if (pushed == null)
                {
                    continue;
                }

                pushed.Start.Z = collisionNode.Start.Z;
                pushed.End.Z = collisionNode.End.Z;
                
                collisionNode.PositionStart(pushed.Start);
                collisionNode.PositionEnd(pushed.End);
                
                int side = crossList.Count > 0
                    ? crossList[0].Index
                    : platform.NearestEdge(collisionNode.Start);
                
                switch (side)
                {
                    case 0:
                    case 2:
                        velocity.Y = -velocity.Y * restitution;
                        break;

                    case 1:
                    case 3:
                        velocity.X = -velocity.X * restitution;
                        break;
                }
                
                collisionNode.PositionEnd(collisionNode.Start - velocity);

                break;
            }
        }

		public bool IsCollide(QuadRunner quad)
		{
            return ((ModelHuman)_Model).CollisionBox.Intersect(quad.rectangle);
        }

        private bool IsCollide(Rectangle rectangle)
		{
            return rectangle.Intersect(_Model.Rectangle);
        }
    }
}
