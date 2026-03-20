using Nekki.Vector.Core.Location.Animation;
using Nekki.Vector.Core.Location.LevelCreation;
using Nekki.Vector.Core.Utilites;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using UnityEngine;
using Xml2Prefab;


namespace Nekki.Vector.Core.Location
{
    public class Elements : BaseElements
    {

        public Elements(BaseObjectRunner parent, uint index)
            : base(parent, index)
        {
        }

        public uint Parse(XmlNode mainNode, Dictionary<string, string> choices)
        {
            if (mainNode == null)
            {
                return Index;
            }
            uint num = 0;
            for (int i = 0; i < mainNode.ChildNodes.Count; i++)
            {
                XmlNode node = mainNode.ChildNodes[i];
                ChoiceContainer choice = null;
                Runner runner = null;
                if (node["Properties"] != null && node["Properties"]["Static"] != null && node["Properties"]["Static"]["Selection"] != null)
                {
                    var selection = node["Properties"]["Static"]["Selection"];
                    choice = new ChoiceContainer(selection.Attributes["Choice"].Value, selection.Attributes["Variant"].Value);
                    if (!Xml2PrefabRoot.Serialize)
                    {
                        if (choices[choice.Name] != choice.Variant)
                        {
                            continue;
                        }
                    }
                }

                switch (node.Name)
                {
                    case "Object":
                        num += (_Parent as ObjectRunner).CreateChild(node, num + Index, choices) + 1;
                        continue;
                    case "Image":
                        runner = CreateVisual(node);
                        if (runner != null)
                            _Visuals.Add((VisualRunner)runner);
                        break;
                    case "Trigger":
                        runner = CreateTrigger(node);
                        if (runner != null)
                            _Triggers.Add((TriggerRunner)runner);
                        break;
                    case "Area":
                        runner = CreateArea(node);
                        if (runner != null)
                            _Areas.Add((AreaRunner)runner);
                        break;
                    case "Spawn":
                        runner = CreateSpawn(node);
                        if (runner != null)
                            _Spawns.Add((SpawnRunner)runner);
                        break;
                    case "Camera":
                        runner = CreateCamera(node);
                        if (runner != null)
                            _Cameras.Add((CameraRunner)runner);
                        break;
                    case "Platform":
                        runner = CreatePlatform(node);
                        if (runner != null)
                            _Platforms.Add((PlatformRunner)runner);
                        break;
                    case "Trapezoid":
                        runner = CreateTrapezoid(node);
                        if (runner != null)
                            _Trapezoids.Add((TrapezoidRunner)runner);
                        break;
                    case "Item":
                        runner = CreateItem(node);
                        if (runner != null)
                            _Items.Add((ItemRunner)runner);
                        break;
                    case "Model":
                        runner = CreatePrimitive(node);
                        if (runner != null)
                            _Primitives.Add((PrimitiveRunner)runner);
                        break;
                    case "Animation":
                        runner = CreateAnimation(node);
                        if (runner != null)
                            _Animations.Add((AnimationRunner)runner);
                        break;
                    case "Particle":
                        runner = CreateParticle(node);
                        if (runner != null)
                            _Particles.Add((ParticleRunner)runner);
                        break;
                }

                if (runner != null)
                {
                    num++;
                    runner.SetVariant(choice);
                    _runners.Add(runner);
                    runner.Generate();
                    _Parent.LayerOrder.Add(runner);
                    runner.Index = num + Index;
                }
            }
            Init();
            return num;
        }

        private TriggerRunner CreateTrigger(XmlNode Node)
        {
            var x = XmlUtils.ParseFloat(Node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(Node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(Node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(Node.Attributes["Height"]);
            var trigger = new TriggerRunner(x, y, width, height, Node);
            trigger.Layer = _Parent.Layer;
            trigger.SetXmlList(Xml2PrefabUtils.GetTransformationNode(Node));
            return trigger;
        }

        private AreaRunner CreateArea(XmlNode node)
        {
            AreaRunner area = null;
            var name = XmlUtils.ParseString(node.Attributes["Name"]);
            string type = XmlUtils.ParseString(node.Attributes["Type"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"]);
            switch (type)
            {
                case "Trick":
                    string itemName = XmlUtils.ParseString(node.Attributes["ItemName"]);
                    int score = XmlUtils.ParseInt(node.Attributes["Score"]);
                    area = new TrickAreaRunner(x, y, width, height, type, name, itemName, score);
                    break;
                case "Catch":
                    float distance = XmlUtils.ParseFloat(node.Attributes["Distance"], 200);
                    area = new ArrestAreaRunner(x, y, width, height, type, name, distance);
                    break;
                case "Help":
                    string key = XmlUtils.ParseString(node.Attributes["Key"]);
                    string description = XmlUtils.ParseString(node.Attributes["Description"]);
                    area = new TutorialAreaRunner(x, y, width, height, type, name, key, description);
                    break;
                default:
                    area = new AreaRunner(AreaRunner.AreaType.None, x, y, width, height, type, name);
                    break;
            }
            area.Layer = _Parent.Layer;
            area.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return area;
        }

        private VisualRunner CreateVisual(XmlNode node)
        {
            var name = XmlUtils.ParseString(node.Attributes["ClassName"]);
            int type = XmlUtils.ParseInt(node.Attributes["Type"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"], float.NaN);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"], float.NaN);
            if (node.Attributes["TrMatrix"] != null)
            {
                width = XmlUtils.ParseFloat(node.Attributes["NativeX"], float.NaN);
                height = XmlUtils.ParseFloat(node.Attributes["NativeY"], float.NaN);
            }
            Color color = Color.white;
            if (node.Attributes["Color"] != null)
            {
                color = ColorUtils.FromHex(node.Attributes["Color"].Value);
            }
            if (node["Properties"] != null && node["Properties"]["Static"] != null && node["Properties"]["Static"]["StartColor"] != null)
            {
                color = ColorUtils.FromHex(node["Properties"]["Static"]["StartColor"].Attributes["Color"].Value);
            }
            int depth = XmlUtils.ParseInt(node.Attributes["Depth"], -1);
            XmlNode matrixNode = null;
            if (node["Properties"] != null && node["Properties"]["Static"] != null && node["Properties"]["Static"]["Matrix"] != null)
            {
                matrixNode = node["Properties"]["Static"]["Matrix"];
                var tX = XmlUtils.ParseFloat(matrixNode.Attributes["Tx"], 0);
                var tY = XmlUtils.ParseFloat(matrixNode.Attributes["Ty"], 0);
                x += tX;
                y += tY;
            }
            var visual = new VisualRunner(type, name, new Pointd(x, y), width, height, color, depth, matrixNode);
            visual.Layer = _Parent.Layer;
            visual.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return visual;
        }

        private PlatformRunner CreatePlatform(XmlNode node)
        {
            var name = XmlUtils.ParseString(node.Attributes["Name"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"]);
            var sticky = XmlUtils.ParseBool(node.Attributes["Sticky"], true);
            var platform = new PlatformRunner(name, x, y, width, height, sticky);
            platform.Layer = _Parent.Layer;
            platform.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return platform;
        }

        private TrapezoidRunner CreateTrapezoid(XmlNode node)
        {
            var name = XmlUtils.ParseString(node.Attributes["Name"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            int type = XmlUtils.ParseInt(node.Attributes["Type"], 1);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"]);
            var height1 = XmlUtils.ParseFloat(node.Attributes["Height1"]);
            var sticky = XmlUtils.ParseBool(node.Attributes["Sticky"], true);
            var trapezoid = new TrapezoidRunner(name, type, x, y, width, height, height1, sticky);
            trapezoid.Layer = _Parent.Layer;
            trapezoid.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return trapezoid;
        }

        private ItemRunner CreateItem(XmlNode node)
        {
            ItemRunner item = null;
            int type = XmlUtils.ParseInt(node.Attributes["Type"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            if (type == 0)
            {
                int score = XmlUtils.ParseInt(node.Attributes["Score"]);
                item = new ItemScoreRunner(type, "ScoreItem", score, x, y);
            }
            else
            {
                int groupId = XmlUtils.ParseInt(node.Attributes["GroupId"]);
                int score = XmlUtils.ParseInt(node.Attributes["Score"]);
                item = new CoinRunner(type, "CoinItem", groupId, score, x, y);
            }
            item.Layer = _Parent.Layer;
            item.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return item;
        }

        private PrimitiveRunner CreatePrimitive(XmlNode node)
        {
            PrimitiveRunner primitive = null;
            var name = XmlUtils.ParseString(node.Attributes["ClassName"]);
            int type = XmlUtils.ParseInt(node.Attributes["Type"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var impulse = XmlUtils.ParseFloat(node.Attributes["Impuls"], 30);
            List<string> sounds = XmlUtils.ParseString(node.Attributes["Sounds"], string.Empty).Split('|').ToList();
            Vector3f deltaPosition = new Vector3f(x, y);
            if (type == 0)
            {
                primitive = new PrimitiveRunner(0, name, Color.black, deltaPosition, impulse, sounds);
            }
            else
            {
                primitive = new PrimitiveAnimatedRunner(type, name, Color.black, deltaPosition, impulse, sounds);
                primitive.Type = Core.Models.ModelType.PrimitiveAnimated;
            }
            primitive.Layer = _Parent.Layer;
            primitive.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return primitive;
        }

        private ParticleRunner CreateParticle(XmlNode node)
        {
            var name = XmlUtils.ParseString(node.Attributes["ClassName"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"]);
            var particle = new ParticleRunner(x, y, width, height, name);
            particle.Layer = _Parent.Layer;
            particle.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return particle;
        }

        private AnimationRunner CreateAnimation(XmlNode node)
        {
            AnimationRunner animation = null;
            var name = XmlUtils.ParseString(node.Attributes["ClassName"]);
            if (name == "p_dust2")
            {
                return null;
            }
            int type = XmlUtils.ParseInt(node.Attributes["Type"]);
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var width = XmlUtils.ParseFloat(node.Attributes["Width"]);
            var height = XmlUtils.ParseFloat(node.Attributes["Height"]);
            var scaleX = XmlUtils.ParseFloat(node.Attributes["ScaleX"], 1);
            var scaleY = XmlUtils.ParseFloat(node.Attributes["ScaleY"], 1);
            if (type == 1)
            {
                Vector2 direction = Vector2.zero;
                Vector2 acceleration = Vector2.zero;
                int life = XmlUtils.ParseInt(node.Attributes["Time"]);
                if (node.Attributes["Direction"] != null)
                {
                    string[] value = node.Attributes["Direction"].Value.Split('|');
                    direction = new Vector2(float.Parse(value[0]), float.Parse(value[1]));
                }
                if (node.Attributes["Acceleration"] != null)
                {
                    string[] value = node.Attributes["Acceleration"].Value.Split('|');
                    acceleration = new Vector2(float.Parse(value[0]), float.Parse(value[1]));
                }
                animation = new AnimationVectorRunner(x, y, width, height, name, 1, scaleX, scaleY, direction, acceleration, life * 60);
            }
            else
            {
                animation = new AnimationRunner(x, y, width, height, name, type, scaleX, scaleY, false, 2);
            }
            animation.Layer = _Parent.Layer;
            animation.SetXmlList(Xml2Prefab.Xml2PrefabUtils.GetTransformationNode(node));
            return animation;
        }

        private CameraRunner CreateCamera(XmlNode node)
        {
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            CameraRunner camera = new CameraRunner(x, y, "", "");
            camera.Layer = _Parent.Layer;
            camera.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return camera;
        }

        private SpawnRunner CreateSpawn(XmlNode node)
        {
            var x = XmlUtils.ParseFloat(node.Attributes["X"]);
            var y = XmlUtils.ParseFloat(node.Attributes["Y"]);
            var animation = XmlUtils.ParseString(node.Attributes["Animation"]);
            var name = XmlUtils.ParseString(node.Attributes["Name"]);
            SpawnRunner spawn = new SpawnRunner(x, y, name, animation);
            spawn.Layer = _Parent.Layer;
            spawn.SetXmlList(Xml2PrefabUtils.GetTransformationNode(node));
            return spawn;
        }
    }
}
