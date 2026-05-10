using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Nekki.Vector.Core.Location.LevelCreation;
using UnityEngine;
using Xml2Prefab;

namespace Nekki.Vector.Core.Location
{
    public class ObjectRunner : BaseObjectRunner
    {
        private string _localTransform;

        private string _externalTransform;

        private ChoiceContainer _choices;

        public ObjectRunner(uint index, BaseObjectRunner parent)
            : base(index, parent)
        {

        }

        public uint Parse(XmlNode node, XmlNode data, Dictionary<string, string> choices)
        {
            _Name = node.Attributes["Name"].ParseString("");
            _Position = new Point(node.Attributes["X"].ParseFloat(), node.Attributes["Y"].ParseFloat());
            _LocalPosition = new Point(_Position);
            _Factor = node.Attributes["Factor"].ParseFloat();
            if (node["Properties"] != null && node["Properties"]["Static"] != null && node["Properties"]["Static"]["Selection"] != null)
            {
                var selectionNode = node["Properties"]["Static"]["Selection"];
                _choices = new ChoiceContainer(selectionNode.Attributes["Choice"].Value, selectionNode.Attributes["Variant"].Value);
            }
            if (_Parent != null)
            {
                Position.Add(_Parent.Position);
            }
            var localTransform = Xml2PrefabUtils.GetTransformationNode(node);
            ParseTransformationData(localTransform);
            if (localTransform != null)
            {
                _localTransform = localTransform.OuterXml;
            }
            Index++;
            var num = CreateElement(node["Content"], Index, choices);
            if (data == null)
            {
                return num;
            }
            var externalTransform = Xml2PrefabUtils.GetTransformationNode(data);
            ParseTransformationData(externalTransform);
            if (externalTransform != null)
                _externalTransform = externalTransform.OuterXml;
            return CreateElement(data["Content"], Index, choices);
        }

        public uint CreateElement(XmlNode node, uint index, Dictionary<string, string> choices)
        {
            var element = new Elements(this, index);
            uint num = element.Parse(node, choices);
            if (_Element == null)
            {
                _Element = element;
            }
            else
            {
                _Element.CopyFrom(element);
            }
            return num;
        }

        public uint CreateChild(XmlNode mainNode, uint index, Dictionary<string, string> choices)
        {
            uint num = 0;
            if (mainNode["Properties"] != null && mainNode["Properties"]["Static"] != null && mainNode["Properties"]["Static"]["Selection"] != null)
            {
                var selectionNode = mainNode["Properties"]["Static"]["Selection"];
                if (!Xml2PrefabRoot.Serialize && choices[selectionNode.Attributes["Choice"].Value] != selectionNode.Attributes["Variant"].Value)
                {
                    return index;
                }
            }
            string name = mainNode.Attributes["Name"].ParseString(string.Empty);
            if (!Xml2PrefabRoot.UseOnlyXML)
            {
                var obj = Xml2PrefabUtils.LoadPrefab(name);
                if (obj != null)
                {
                    var model = obj.GetComponent<Xml2PrefabObjectRunnerContainer>();
                    if (mainNode["Properties"] != null && mainNode["Properties"]["Static"] != null && mainNode["Properties"]["Static"]["Selection"] != null)
                    {
                        var selectionNode = mainNode["Properties"]["Static"]["Selection"];
                        model.SetChoices(new ChoiceContainer(selectionNode.Attributes["Choice"].Value, selectionNode.Attributes["Variant"].Value));
                    }
                    var objectRunner = new SerializedObjectRunner(index, this);
                    UpdatePrefabPosition(mainNode, model);
                    if (mainNode["content"] != null)
                    {
                        Debug.LogError("Prefab reference sort of. This is not allowed");
                    }
                    num = objectRunner.Parse(model, choices);
                    objectRunner.Init();
                    LayerOrder.Add(objectRunner);
                    _Childs.Add(objectRunner);
                }
                else
                {
                    var objectRunner1 = new ObjectRunner(index, this);
                    num = objectRunner1.Parse(mainNode, null, choices);
                    objectRunner1.Init();
                    LayerOrder.Add(objectRunner1);
                    _Childs.Add(objectRunner1);
                }
            }
            else
            {
                XmlNode data = null;

                if (!string.IsNullOrEmpty(name))
                {
                    data = Sets.ObjectNode(name);
                }

                var objectRunner1 = new ObjectRunner(index, this);
                num = objectRunner1.Parse(mainNode, data, choices);
                objectRunner1.Init();
                LayerOrder.Add(objectRunner1);
                _Childs.Add(objectRunner1);
            }
            return num;
        }

        private void UpdatePrefabPosition(XmlNode node, Xml2PrefabObjectRunnerContainer model)
        {
            model.transform.localPosition = new Vector3(node.Attributes["X"].ParseFloat(), node.Attributes["Y"].ParseFloat());
        }

        public void ParseMiscData(XmlNode Node)
        {
        }

        public override void Init()
        {
            base.Init();
            if (Xml2PrefabRoot.Serialize)
            {
                SerializeData();
            }
        }

        protected override void InitElements()
        {
            foreach (var runner in Element.Runners)
            {
                runner.InitRunner(Position, Xml2PrefabRoot.Serialize);
                BuildTranformationTable(runner.TransformationData);
            }
        }

        private void SerializeData()
        {
            _Element.InitSerializedData();
            List<Component> list = new List<Component>();
            list.AddRange(_Childs.Select(runner => { return runner.UnityObject.GetComponent<Xml2PrefabObjectRunnerContainer>(); }));
            list.AddRange(_Element.Models);
            List<Component> runners = new List<Component>();
            foreach (var obj in LayerOrder)
            {
                if (obj is BaseObjectRunner objectRunner)
                {
                    runners.Add(list.FirstOrDefault(x => x.gameObject == objectRunner.UnityObject));
                }
                if (obj is Runner runner)
                {
                    runners.Add(list.FirstOrDefault(x => x.gameObject == runner.ComponentHolder));
                }
            }
            var component = _UnityObject.AddComponent<Xml2PrefabObjectRunnerContainer>();
            component.Init(_Name, _Position.X, _Position.Y, _Factor, Index, _localTransform, _externalTransform, runners, _choices);
        }
    }
}
