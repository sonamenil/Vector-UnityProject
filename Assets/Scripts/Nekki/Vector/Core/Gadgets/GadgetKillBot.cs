using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;

namespace Nekki.Vector.Core.Gadgets
{
	public class GadgetKillBot : Gadget
	{
		private const int Distance = 300;

		private ModelHuman _selfModel;

		private ModelHuman _botModel;

		private ModelNode _COMnode;

		public GadgetKillBot()
			: base(GadgetType.KillBot)
		{
			_selfModel = LevelMainController.current.Location.GetUserModel();
            _botModel = LevelMainController.current.Location.GetBotModel();
			_COMnode = _selfModel.GetNode("COM");
        }

        public override bool IsCanUse()
		{
			if (!LevelMainController.current.IsDeath && !_selfModel.IsDeath)
			{
				var botModels = LevelMainController.current.Location.GetAllBotModels();
				foreach (var botModel in botModels)
				{
					_botModel = botModel;
					if (_botModel != null && !_botModel.IsDeath && _botModel.IsDelayEnd)
					{
						return ModelHuman.Distance(_selfModel, _botModel) < Distance;
					}
				}
			}
			return false;
		}

		public override void Play()
		{
			base.Play();
			KillBot();
			base.Stop();
		}

		private void KillBot()
		{
			_botModel.StartPhysics();
			_botModel.Death(GameEndType.GE_MURDER);
			foreach (var edge in _botModel.ModelObject.EdgesAll)
			{
				var vector = new Vector3d(edge.Center);
				vector.Subtract(_COMnode.Start);
				vector.Normalize();
				vector.Multiply(20);
				_botModel.Strike(edge, edge.Center, vector);
			}
			_botModel.ModelObject.RenderMacroNode();
			_botModel.ControllerPhysics.Render();
			_botModel.ModelObject.RenderMacroNode();
			_selfModel.controllerModelEffect.RunAntibotEffect();
		}
	}
}
