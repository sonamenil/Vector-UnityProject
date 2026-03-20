using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nekki.Vector.Core.Controllers
{
	public class ControllerCatching
	{
		private ModelHuman _Model;

		private List<string> _Mureders;

		private bool _IsPlay;

		private Dictionary<ModelHuman, int> _Targets = new Dictionary<ModelHuman, int>();

		public static float DistanceFactor = 250;

		public static float Timeout = 0.35f;

		public static float HeightFactor = 2;

		public ControllerCatching(ModelHuman model)
		{
			_Model = model;
			_Mureders = _Model.Murders;
		}

		public void Play()
		{
			SoundsManager.Instance.PlaySounds(SoundType.enemy_charge);
			_Model.controllerModelEffect.RunTaserEffect();
			_IsPlay = true;
		}

		public void Stop()
		{
            _Model.controllerModelEffect.StopTaserEffect();
            _IsPlay = false;
        }

		public void Catch(ModelHuman modelHuman)
		{
			Stop();
			modelHuman.StartPhysics();
			if (modelHuman.IsSelf)
			{
				LevelMainController.current.Murder(modelHuman);
			}
			else
			{
				if (LevelMainController.IsHunterMode)
				{
					LevelMainController.current.Win(modelHuman, 1);
				}
			}
			SoundsManager.Instance.PlaySounds(SoundType.enemy_discharge);
			GamepadController.Instance.Rumble(0.7f, 0.7f,0.5f);
			modelHuman.controllerModelEffect.RunParalyzeEffect();
			_Model.controllerModelEffect.RunTaserExplosion();
		}

		public void Render(ModelHuman modelHuman)
		{
			if (modelHuman == null)
			{
				return;
			}
			if (_Mureders.Contains(modelHuman.Name))
			{
				if (IsActive(modelHuman))
				{
                    if (_Targets.ContainsKey(modelHuman))
                    {
                        if (_Targets[modelHuman] != -1)
                        {
                            _Targets[modelHuman] -= 1;
                            if (_Targets[modelHuman] == 0)
                            {
                                Catch(modelHuman);
                                return;
                            }
							return;
                        }
                    }
                    _Targets[modelHuman] = (int)(Timeout * 60);
                    if (_IsPlay)
                    {
                        return;
                    }
                    Play();
                    return;
                }
				else
				{
					if (!_Targets.ContainsKey(modelHuman))
					{
						return;
					}
					_Targets.Remove(modelHuman);
					Stop();
				}
            }
		}

		public bool IsActive(ModelHuman modelHuman)
		{
            if (!_Model.IsEnabled || !modelHuman.IsEnabled || _Model.IsPhysics || modelHuman.IsPhysics || _Model.LockInterval)
			{
				return false;
			}
			return Distance(_Model.ModelObject.CenterOfMassNode, modelHuman.ModelObject.CenterOfMassNode);
		}

		public bool Distance(ModelNode node1, ModelNode node2)
		{
            var vector = new Vector3f();
			vector.X = Mathf.Abs((float)node2.Start.X - (float)node1.Start.X);
			vector.Y = Mathf.Abs((float)node2.Start.Y - (float)node1.Start.Y) * HeightFactor;

			return Vector3f.Distance(vector, Vector3f.Zero) < DistanceFactor;
		}

		public void Reset()
		{
			_Model.controllerModelEffect.StopTaserEffect();
			_IsPlay = false;
		}
	}
}
