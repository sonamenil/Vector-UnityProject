using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xml2Prefab
{
	[ExecuteAlways]
	public class Xml2PrefabObjectRunnerContainer : MonoBehaviour
	{
		[SerializeField]
		private string _name;

		[SerializeField]
		private float _factor;

		[SerializeField]
		private uint _index;

		[SerializeField, TextArea(10, 25)]
		private string _local;

		[SerializeField]
		private string _external;

		[SerializeField]
		private List<Component> _runners;

		[SerializeField]
		private ChoiceContainer _choice;

		private readonly List<Type> _containers = new List<Type>();

		public string Name => _name;

		public float Factor => _factor;

		public string LocalDynamicTransform => _local;

		public string ExternalDynamicTransform => _external;

		public List<Component> Runners => _runners;

		public ChoiceContainer Choice => _choice;

		public Xml2PrefabObjectRunnerContainer()
		{
			_containers.Add(typeof(Xml2PrefabAnimationVectorContainer));
			_containers.Add(typeof(Xml2PrefabAnimationContainer));
			_containers.Add(typeof(Xml2PrefabAreaContainer));
			_containers.Add(typeof(Xml2PrefabArrestAreaContainer));
			_containers.Add(typeof(Xml2PrefabTrickAreaContainer));
			_containers.Add(typeof(Xml2PrefabTutorialAreaContainer));
			_containers.Add(typeof(Xml2PrefabCameraContainer));
			_containers.Add(typeof(Xml2PrefabCoinContainer));
			_containers.Add(typeof(Xml2PrefabItemContainer));
			_containers.Add(typeof(Xml2PrefabItemScoreContainer));
			_containers.Add(typeof(Xml2PrefabObjectRunnerContainer));
			_containers.Add(typeof(Xml2PrefabParticleContainer));
			_containers.Add(typeof(Xml2PrefabPlatformContainer));
			_containers.Add(typeof(Xml2PrefabPrimitiveContainer));
			_containers.Add(typeof(Xml2PrefabSpawnContainer));
			_containers.Add(typeof(Xml2PrefabTrapezoidContainer));
			_containers.Add(typeof(Xml2PrefabTriggerContainer));
            _containers.Add(typeof(Xml2PrefabVisualRunnerContainer));
        }

		public void Init(string n, float x, float y, float factor, uint index, string localTransform, string externalTransform, List<Component> references, ChoiceContainer choice)
		{
            _name = n;
            _factor = factor;
            _index = index;
            _local = localTransform;
            _external = externalTransform;
            _runners = references;
            _choice = choice;
        }

		public void SetChoices(ChoiceContainer choiceContainer)
		{
			_choice = choiceContainer;
		}

		public void UpdateHierarchy()
		{
			if (_runners == null)
			{
				_runners = new List<Component>();
			}
			_runners.Clear();
			foreach (Transform child in transform)
			{
				foreach (var type in _containers)
				{
                    foreach (var component in child.GetComponents(type))
					{
						if (!_runners.Contains(component))
						{
                            _runners.Add(component);
                        }

					}

                }

			}
		}

#if UNITY_EDITOR
        private void Update()
        {
			if (Application.isPlaying)
			{
				return;
			}
            UpdateHierarchy();
        }
#endif
    }
}
