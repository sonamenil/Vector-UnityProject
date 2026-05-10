using System.Collections.Generic;
using Core._Common;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Node;
using Nekki.Vector.Core.Scripts;
using UnityEngine;

namespace Nekki.Vector.Core.Controllers
{
    public class ControllerModelEffect
    {
        private ModelHuman _modelHuman;

        private AnimationSprite _antibotEffect;

        private AnimationSprite _taserExplosionEffect;

        private Dictionary<ModelNode, AnimationSprite> _paralyzeEffect = new Dictionary<ModelNode, AnimationSprite>();

        private bool _taserEffectActive;

        private List<(ModelNode, AnimationSprite)> _taserEffect = new List<(ModelNode, AnimationSprite)>();

        private GameObject _layer;

        public GameObject Layer
        {
            set
            {
                _layer = value;
                if (_antibotEffect != null)
                    _antibotEffect.transform.SetParent(_layer.transform, true);
                _taserExplosionEffect.transform.SetParent(_layer.transform, true);
                foreach (var item in _taserEffect)
                {
                    item.Item2.transform.SetParent(_layer.transform, true);
                }
            }
        }

        public ControllerModelEffect(ModelHuman modelHuman)
        {
            _modelHuman = modelHuman;
            if (_modelHuman.IsSelf)
            {
                CreateAntibotEffect();
            }
            else
            {
                CreateTaserEffect();
            }
            CreateTaserExplosionEffect();
        }

        private void CreateAntibotEffect()
        {
            _antibotEffect = CreateGO("AntibotEffect", VectorPaths.AnimatedTextures + "/antibot");
            _antibotEffect.FPS = 100;
            _antibotEffect.Iterations = 1;
            _antibotEffect.OnIterationsEnd = () =>
            {
                _antibotEffect.gameObject.SetActive(false);
            };
        }

        public void RunAntibotEffect()
        {
            if (_antibotEffect == null)
            {
                Debug.LogError("Run antibot but effect null");
                return;
            }
            var node = _modelHuman.GetNode("COM");
            _antibotEffect.transform.localPosition = new Vector3((float)node.Start.X, (float)node.Start.Y, -20);
            _antibotEffect.gameObject.SetActive(true);
            _antibotEffect.Reset();
            _antibotEffect.IsWork = true;
        }

        private void CreateTaserExplosionEffect()
        {
            _taserExplosionEffect = CreateGO("Taser Explosion", VectorPaths.AnimatedTextures + "/lightning_expl_v2");
            _taserExplosionEffect.transform.localScale = new Vector3(2, 2, 2);
            _taserExplosionEffect.FPS = 100;
            _taserExplosionEffect.Iterations = 1;
            _taserExplosionEffect.OnIterationsEnd = () =>
            {
                _taserExplosionEffect.gameObject.SetActive(false);
            };
        }

        public void RunTaserExplosion()
        {
            if (_taserExplosionEffect == null)
            {
                Debug.LogError("Run TaserExplosion but effect null");
                return;
            }
            var node = _modelHuman.GetNode("COM");
            _taserExplosionEffect.transform.localPosition = new Vector3((float)node.Start.X, (float)node.Start.Y, -20);
            _taserExplosionEffect.gameObject.SetActive(true);
            _taserExplosionEffect.IsWork = true;
        }

        private void CreateTaserEffect()
        {
            string path = VectorPaths.AnimatedTextures + "/lightning_hands";
            _taserEffect.Add((_modelHuman.GetNode("NKnuckles_1"), CreateGO("Taser 1", path)));
            _taserEffect.Add((_modelHuman.GetNode("NKnuckles_2"), CreateGO("Taser 2", path)));
        }

        public void RunTaserEffect()
        {
            _taserEffectActive = true;
            foreach (var item in _taserEffect)
            {
                item.Item2.gameObject.SetActive(true);
                item.Item2.IsWork = true;
            }
        }

        public void StopTaserEffect()
        {
            _taserEffectActive = false;
            foreach (var item in _taserEffect)
            {
                item.Item2.gameObject.SetActive(false);
            }
        }

        private void RenderTasetEffect()
        {
            if (!_taserEffectActive)
            {
                return;
            }
            foreach (var item in _taserEffect)
            {
                item.Item2.transform.localPosition = new Vector3((float)item.Item1.Start.X, (float)item.Item1.Start.Y, -20);
                item.Item2.IsWork = true;
            }
        }

        public void RunParalyzeEffect()
        {
            string[] nodes = {
                "NNeck",
                "NPivot",
                "NKnee_1",
                "NKnee_2",
                "NElbow_1",
                "NElbow_2"
            };
            _paralyzeEffect.Clear();
            foreach (var nodeName in nodes)
            {
                var node = _modelHuman.GetNode(nodeName);
                var animation = CreateGO("Taser Paralyze " + nodeName, VectorPaths.AnimatedTextures + "/lightning_paraliz_v2", true);
                animation.IsWork = true;
                animation.transform.parent = _layer.transform;
                animation.transform.localPosition = new Vector3((float)node.Start.X, (float)node.Start.Y, -20);
                animation.transform.Rotate(0, 0, Random.Range(0, 360));
                _paralyzeEffect.Add(node, animation);
            }
        }

        public void ParalyzeRender()
        {
            foreach (var node in _paralyzeEffect.Keys)
            {
                _paralyzeEffect[node].transform.localPosition = new Vector3((float)node.Start.X, (float)node.Start.Y, -20);
            }
        }

        public void StopParalyzeEffect()
        {
            foreach (var effect in _paralyzeEffect.Values)
            {
                Object.Destroy(effect.gameObject);
            }
            _paralyzeEffect.Clear();
        }

        public void Render()
        {
            ParalyzeRender();
            RenderTasetEffect();
        }

        public void Reset()
        {
            if (_antibotEffect != null)
                _antibotEffect.gameObject.SetActive(false);
            _taserExplosionEffect.gameObject.SetActive(false);
            StopParalyzeEffect();
            StopTaserEffect();
        }

        private AnimationSprite CreateGO(string goName, string animationName, bool isActive = false)
        {
            var animationSprite = Object.Instantiate(Resources.Load<GameObject>("LevelContent/Prefabs/AnimationSprite")).GetComponent<AnimationSprite>();
            animationSprite.Init(animationName, null, 0.5f, 0.5f);
            animationSprite.gameObject.SetActive(isActive);
            animationSprite.name = goName;
            return animationSprite;
        }
    }
}
