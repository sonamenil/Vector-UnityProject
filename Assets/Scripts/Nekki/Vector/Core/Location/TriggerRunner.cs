using System.Collections.Generic;
using System.Xml;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Trigger;
using Nekki.Vector.Core.Trigger.Events;
using UnityEngine;
using Xml2Prefab;

namespace Nekki.Vector.Core.Location
{
    public class TriggerRunner : QuadRunner
    {
        public enum TriggerColisionType
        {
            OneNode = 0,
            MultiNode = 1
        }

        private TriggerTimer _timer;

        private Variable _AIvar;

        private Variable _nodeVar;

        private Variable _modelVar;

        private Variable _activeVar;

        private ModelHuman _checkedModel;

        private Dictionary<string, Variable> vars = new Dictionary<string, Variable>();

        private List<TriggerLoop> _loops = new List<TriggerLoop>();

        private List<TriggerLine> _lines;

        private List<TE_ChangeVar> _renderEvents = new List<TE_ChangeVar>();

        private TriggerColider _colider;

        private XmlNode _xmlNode;

        private string _statistic;

        private XmlNode _rawNode;

        private float _w;

        private float _h;

        private string _CollisionNodeName;

        public List<string> _NodesName;

        private TriggerColisionType _CollisionType = TriggerColisionType.OneNode;

        public string CollisionNodeName
        {
            get => _CollisionNodeName;
            set => _CollisionNodeName = value;
        }

        public List<string> TriggerNodesName => _NodesName;

        public List<TriggerLine> Lines => _lines;

        public Variable AIVar => _AIvar;

        public Variable ModelVar => _modelVar;

        public string NodeName => _nodeVar == null ? "COM" : _nodeVar.ValueString;

        public bool IsActive => _activeVar.ValueInt == 1;

        public string statistic => _statistic;

        public TriggerColisionType CollisionType => _CollisionType;

        public TriggerRunner(float p_x, float p_y, float p_width, float p_height, XmlNode p_node)
            : base(p_x, p_y, p_width, p_height, sticky: false, 0, p_node.Attributes["Name"].ParseString(string.Empty))
        {
            _TypeClass = RunnerType.Trigger;
            _timer = new TriggerTimer(this);
            _w = p_width;
            _h = p_height;
            _xmlNode = p_node["Content"];
            _statistic = p_node.Attributes["Statistic"].ParseString();
            _rawNode = p_node;
        }

        public void Init()
        {
            parseVariable(_xmlNode["Init"]);
            SetTriggerCollisionType();
            XmlNode xmlNode = _xmlNode["Template"];
            if (xmlNode != null)
            {
                string value = xmlNode.Attributes["Name"].Value;
                parseTemplate(TemplateModule.getTemplateXmlNode(value));
            }
            parseLoops(_xmlNode);
            InitEvents();
        }

        private void SetTriggerCollisionType()
        {
            string[] array = _nodeVar.ValueString.Split('|');
            if (array.Length == 1)
            {
                _CollisionType = TriggerColisionType.OneNode;
                _CollisionNodeName = array[0];
            }
            else
            {
                _CollisionType = TriggerColisionType.MultiNode;
                _NodesName = new List<string>(array);
            }
        }

        protected override void SerializeData()
        {
            if (_UnityObject == null)
            {
                CreateObject();
            }
            _UnityObject.AddComponent<Xml2PrefabTriggerContainer>().Init(_rawNode.OuterXml, _h, _w, Choice);
            _CachedTransform = _UnityObject.transform;
        }

        private void InitEvents()
        {
            foreach (TriggerLoop loop in _loops)
            {
                foreach (TriggerEvent @event in loop.Events)
                {
                    if (@event.Type == TriggerEvent.EventType.TET_ON_SHOW || @event.Type == TriggerEvent.EventType.TET_ON_HIDE)
                    {
                        CreateCollider();
                    }
                    if (@event.Type == TriggerEvent.EventType.TET_VAR_CHANGE)
                    {
                        _renderEvents.Add((TE_ChangeVar)@event);
                    }
                }
            }
        }

        private void CreateCollider()
        {
            if (_colider == null)
            {
                CreateObject();
                var controller = UnityObject.AddComponent<TriggerColider>();
                controller.Init(rectangle);
                controller.OnBecameVisibleAction.AddListener(OnBecameVisible);
                controller.OnBecameUnvisibleAction.AddListener(OnBecameUnvisible);
            }
        }

        private void OnBecameVisible()
        {
            CheckEvent(new TE_OnShow(), null);
        }

        private void OnBecameUnvisible()
        {
            CheckEvent(new TE_OnHide(), null);
        }

        public void parseTemplate(XmlNode p_node)
        {
            parseLoops(p_node);
        }

        public void parseVariable(XmlNode p_node, bool isTemplate = false)
        {
            if (p_node == null)
            {
                return;
            }
            foreach (XmlNode childNode in p_node.ChildNodes)
            {
                if (!childNode.LocalName.Equals("SetVariable"))
                {
                    continue;
                }
                string text = childNode.Attributes["Name"].ParseString();
                string text2 = childNode.Attributes["Value"].ParseString(string.Empty);
                if (vars.ContainsKey("_" + text))
                {
                    switch (vars["_" + text].Type)
                    {
                        case VariableTypeE.VT_INT:
                            vars["_" + text].setValue(int.Parse(text2));
                            break;
                        case VariableTypeE.VT_DOUBLE:
                            vars["_" + text].setValue(float.Parse(text2));
                            break;
                        case VariableTypeE.VT_STRING:
                            vars["_" + text].setValue(text2);
                            break;
                    }
                }
                else
                {
                    vars["_" + text] = Variable.createVariable(text2, text, this);
                }
            }
            if (!isTemplate)
            {
                _AIvar = vars["_$AI"];
                _nodeVar = vars["_$Node"];
                _activeVar = vars["_$Active"];
                vars["_$ActionID"] = Variable.createVariable(" ", "$ActionID", this);
                if (!vars.ContainsKey("_$Model"))
                {
                    _modelVar = Variable.createVariable(" ", "$Model", this);
                    vars["_$Model"] = _modelVar;
                }
                else
                {
                    _modelVar = vars["_$Model"];
                }
                vars["_$Key"] = Variable.createVariable(" ", "$Key", this);
                if (_AIvar == null)
                {
                    _AIvar = Variable.createVariable("-1", "$AI");
                }
                if (_activeVar == null)
                {
                }
                if (_nodeVar != null)
                {
                }
            }
        }

        public void parseLoops(XmlNode p_node)
        {
            if (p_node == null)
            {
                return;
            }
            foreach (XmlNode childNode in p_node.ChildNodes)
            {
                if (string.Equals(childNode.LocalName, "Loop"))
                {
                    TriggerLoop triggerRunnerLoop;
                    if (childNode.Attributes["Template"] != null)
                    {
                        string value = childNode.Attributes["Template"].Value;
                        XmlNode templateLoopXML = TemplateModule.getTemplateLoopXML(value);
                        triggerRunnerLoop = TriggerLoop.createLoop(templateLoopXML, this);
                        _loops.Add(triggerRunnerLoop);
                    }
                    else
                    {
                        triggerRunnerLoop = TriggerLoop.createLoop(childNode, this);
                        _loops.Add(triggerRunnerLoop);
                    }
                }
            }
        }

        public void CheckEvent(TriggerEvent p_event, ModelHuman p_model)
        {
            if (!IsEnabled || (!IsActive && !p_event.IsTimeOutOrActivate()) || (p_model != null && p_model.IsPhysics && !p_event.IsCollision()))
            {
                return;
            }
            _checkedModel = p_model;
            List<List<TriggerAction>> list = new List<List<TriggerAction>>();
            foreach (var loop in _loops)
            {
                if (loop.CheckEvent(p_event))
                {
                    list.Add(loop.Actions);
                }
            }
            if (list.Count != 0)
            {
                foreach (var actions in list)
                {
                    TriggerActionsRenderer.Current.AddActions(actions);
                }
                if (p_model != null)
                {
                    if (p_model.UserData.IsSelf)
                    {
                        p_model.controllerStatistics.SetTrigger(this);
                    }
                }
                _modelVar.setValue("");
                _checkedModel = null;
            }
        }

        public void ResetRenderEvents()
        {
            foreach (var @event in _renderEvents)
            {
                @event.Reset();
            }
        }

        public void CheckRenderEvent(ModelHuman p_model)
        {
            if (_renderEvents == null)
            {
                return;
            }
            _modelVar.setValue(p_model.ModelName);
            foreach (TE_ChangeVar renderEvent in _renderEvents)
            {
                if (renderEvent.IsChange())
                {
                    CheckEvent(renderEvent, p_model);
                    _modelVar.setValue(p_model.ModelName);
                }
            }
            _modelVar.setValue(string.Empty);
        }

        public override bool Render()
        {
            return _timer.Render();
        }

        public override void Reset()
        {
            base.Reset();
            _timer.Reset();
            foreach (var vars in vars.Values)
            {
                vars.resetValues();
            }
            foreach (var loop in _loops)
            {
                loop.Reset();
            }
            if (_lines != null)
            {
                foreach (var line in _lines)
                {
                    line.Reset();
                }
            }
        }

        public Variable GetVar(string p_key)
        {
            if (!vars.ContainsKey(p_key))
            {
                DebugUtils.Dialog("No Var Name = " + p_key + " in trigger " + Name, true);
                return null;
            }
            return vars[p_key];
        }

        public SpawnRunner GetSpawnByName(string p_name)
        {
            foreach (var spawn in ParentElements.Spawns)
            {
                if (spawn.Name == p_name)
                {
                    return spawn;
                }
            }
            return null;
        }

        public void SetModelVar()
        {
            if (_checkedModel == null)
            {
                return;
            }
            _modelVar.setValue(_checkedModel.ModelName);
        }

        public void AddLine(TriggerLine p_line)
        {
            if (_lines == null)
            {
                _lines = new List<TriggerLine>();
            }
            _lines.Add(p_line);
        }

        public void SetKeyVar(string p_key)
        {
            vars["_$Key"].setValue(p_key);
        }

        public void SetTimer(int p_frames)
        {
            _timer.Start(p_frames);
        }

        public override string ToString()
        {
            return null;
        }
    }
}
