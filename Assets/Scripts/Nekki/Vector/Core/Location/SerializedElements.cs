using Nekki.Vector.Core.Location.Animation;
using Nekki.Vector.Core.Location.LevelCreation;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using Xml2Prefab;
using static Core._Common.Settings;
using static Core._Common.Settings.Visual;

namespace Nekki.Vector.Core.Location
{
    public class SerializedElements : BaseElements
    {
        private readonly Dictionary<System.Type, Func<Component, uint, uint>> _actions = new Dictionary<System.Type, Func<Component, uint, uint>>();

        private Dictionary<string, string> _choices = new Dictionary<string, string>();

        public SerializedElements(BaseObjectRunner parent, uint index)
            : base(parent, index)
        {
            _actions.Add(typeof(Xml2PrefabObjectRunnerContainer), ObjectRunner);
            _actions.Add(typeof(Xml2PrefabVisualRunnerContainer), VisualRunner);
            _actions.Add(typeof(Xml2PrefabPlatformContainer), PlatformRunner);
            _actions.Add(typeof(Xml2PrefabTriggerContainer), TriggerRunner);
            _actions.Add(typeof(Xml2PrefabAreaContainer), AreaRunner);
            _actions.Add(typeof(Xml2PrefabArrestAreaContainer), AreaRunner);
            _actions.Add(typeof(Xml2PrefabTrickAreaContainer), AreaRunner);
            _actions.Add(typeof(Xml2PrefabTutorialAreaContainer), AreaRunner);
            _actions.Add(typeof(Xml2PrefabTrapezoidContainer), TrapezoidRunner);
            _actions.Add(typeof(Xml2PrefabItemContainer), ItemRunner);
            _actions.Add(typeof(Xml2PrefabItemScoreContainer), ItemRunner);
            _actions.Add(typeof(Xml2PrefabCoinContainer), ItemRunner);
            _actions.Add(typeof(Xml2PrefabPrimitiveContainer), PrimitiveRunner);
            _actions.Add(typeof(Xml2PrefabParticleContainer), ParticleRunner);
            _actions.Add(typeof(Xml2PrefabAnimationContainer), AnimationRunnerFunc);
            _actions.Add(typeof(Xml2PrefabAnimationVectorContainer), AnimationRunnerFunc);
            _actions.Add(typeof(Xml2PrefabCameraContainer), CameraRunner);
            _actions.Add(typeof(Xml2PrefabSpawnContainer), SpawnRunnerFunc);
        }

        private bool VariantAllowed(ChoiceContainer choice)
        {
            if (choice == null) return true;
            if (!_choices.ContainsKey(choice.Name))
            {
                return true;
            }
            return _choices[choice.Name] == choice.Variant;
        }

        private uint ObjectRunner(Component component, uint index)
        {
            var objectComponent = component as Xml2PrefabObjectRunnerContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(objectComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            return (_Parent as SerializedObjectRunner).CreateChild(objectComponent, Index + index, _choices) + 1;
        }

        private uint VisualRunner(Component component, uint index)
        {
            var VisualComponent = component as Xml2PrefabVisualRunnerContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(VisualComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Visual = CreateVisual(VisualComponent);
            if (Visual == null)
            {
                return index;
            }
            Visual.Index = Index + index;
            Visual.Generate(VisualComponent.gameObject);
            _Visuals.Add(Visual);
            _runners.Add(Visual);
            return 1;
        }

        private uint PlatformRunner(Component component, uint index)
        {
            var PlatformComponent = component as Xml2PrefabPlatformContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(PlatformComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Platform = CreatePlatform(PlatformComponent);
            if (Platform == null)
            {
                return index;
            }
            Platform.Index = Index + index;
            Platform.Generate(PlatformComponent.gameObject);
            _Platforms.Add(Platform);
            _runners.Add(Platform);
            return 1;
        }

        private uint TriggerRunner(Component component, uint index)
        {
            var TriggerComponent = component as Xml2PrefabTriggerContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(TriggerComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Trigger = CreateTrigger(TriggerComponent);
            if (Trigger == null)
            {
                return index;
            }
            Trigger.Index = Index + index;
            Trigger.Generate(TriggerComponent.gameObject);
            _Triggers.Add(Trigger);
            _runners.Add(Trigger);
            return 1;
        }

        private uint AreaRunner(Component component, uint index)
        {
            var areaComponent = component as Xml2PrefabAreaContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(areaComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var area = CreateArea(areaComponent);
            if (area == null)
            {
                return index;
            }
            area.Index = Index + index;
            area.Generate(areaComponent.gameObject);
            _Areas.Add(area);
            _runners.Add(area);
            return 1;
        }

        private uint TrapezoidRunner(Component component, uint index)
        {
            var TrapezoidComponent = component as Xml2PrefabTrapezoidContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(TrapezoidComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Trapezoid = CreateTrapezoid(TrapezoidComponent);
            if (Trapezoid == null)
            {
                return index;
            }
            Trapezoid.Index = Index + index;
            Trapezoid.Generate(TrapezoidComponent.gameObject);
            _Trapezoids.Add(Trapezoid);
            _runners.Add(Trapezoid);
            return 1;
        }

        private uint ItemRunner(Component component, uint index)
        {
            var ItemComponent = component as Xml2PrefabItemContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(ItemComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Item = CreateItem(ItemComponent);
            if (Item == null)
            {
                return index;
            }
            Item.Index = Index + index;
            Item.Generate(ItemComponent.gameObject);
            _Items.Add(Item);
            _runners.Add(Item);
            return 1;
        }

        private uint PrimitiveRunner(Component component, uint index)
        {
            var PrimitiveComponent = component as Xml2PrefabPrimitiveContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(PrimitiveComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Primitive = CreatePrimitive(PrimitiveComponent);
            if (Primitive == null)
            {
                return index;
            }
            Primitive.Index = Index + index;
            Primitive.Generate(PrimitiveComponent.gameObject);
            _Primitives.Add(Primitive);
            _runners.Add(Primitive);
            return 1;
        }

        private uint ParticleRunner(Component component, uint index)
        {
            var ParticleComponent = component as Xml2PrefabParticleContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(ParticleComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Particle = CreateParticle(ParticleComponent);
            if (Particle == null)
            {
                return index;
            }
            Particle.Index = Index + index;
            Particle.Generate(ParticleComponent.gameObject);
            _Particles.Add(Particle);
            _runners.Add(Particle);
            return 1;
        }

        private uint AnimationRunnerFunc(Component component, uint index)
        {
            var animationComponent = component as Xml2PrefabAnimationContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(animationComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var animation = CreateAnimation(animationComponent);
            if (animation == null)
            {
                return index;
            }
            animation.Index = Index + index;
            animation.Generate(animationComponent.gameObject);
            _Animations.Add(animation);
            _runners.Add(animation);
            return 1;
        }

        private uint CameraRunner(Component component, uint index)
        {
            var CameraComponent = component as Xml2PrefabCameraContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(CameraComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Camera = CreateCamera(CameraComponent);
            if (Camera == null)
            {
                return index;
            }
            Camera.Index = Index + index;
            Camera.Generate(CameraComponent.gameObject);
            _Cameras.Add(Camera);
            _runners.Add(Camera);
            return 1;
        }

        private uint SpawnRunnerFunc(Component component, uint index)
        {
            var SpawnComponent = component as Xml2PrefabSpawnContainer;
            if (!Xml2PrefabRoot.Serialize)
            {
                if (!VariantAllowed(SpawnComponent.Choice))
                {
                    component.gameObject.SetActive(false);
                    return 0xffffffff;
                }
            }
            var Spawn = CreateSpawn(SpawnComponent);
            if (Spawn == null)
            {
                return index;
            }
            Spawn.Index = Index + index;
            Spawn.Generate(SpawnComponent.gameObject);
            _Spawns.Add(Spawn);
            _runners.Add(Spawn);
            return 1;
        }

        public uint Parse(List<Component> components, Dictionary<string, string> choices)
        {
            if (components == null)
            {
                return Index;
            }
            _choices = choices;
            uint num = 0;
            for (int i = 0; i < components.Count; i++)
            {
                _actions.TryGetValue(components[i].GetType(), out var action);
                var index = (uint)(action?.Invoke(components[i], num));
                if (index != 0xffffffff)
                {
                    num += index;
                }
            }
            Init();
            return num;
        }

        private TriggerRunner CreateTrigger(Xml2PrefabTriggerContainer model)
        {
            var content = XmlUtils.OpenXMLElementFromString(model.Node);
            var x = model.X;
            var y = model.Y;
            var w = model.W;
            var h = model.H;
            var trigger = new TriggerRunner(x, y, w, h, content);
            trigger.Layer = _Parent.Layer;
            var node = Xml2PrefabUtils.GetTransformationNode(content);
            trigger.SetXmlListSerialized(node);
            return trigger;
        }

        private AreaRunner CreateArea(Xml2PrefabAreaContainer model)
        {
            AreaRunner area = null;

            var x = model.X;
            var y = model.Y;
            var w = model.W;
            var h = model.H;
            var name = model.Name;
            var type = model.Type;

            switch (model.Type)
            {
                case "Trick":
                    var trick = model as Xml2PrefabTrickAreaContainer;
                    var score = trick.Score;
                    var itemname = trick.ItemName;
                    area = new TrickAreaRunner(x, y, w, h, type, name, itemname, score);
                    break;
                case "Help":
                    var help = model as Xml2PrefabTutorialAreaContainer;
                    var key = help.Key;
                    var description = help.Description;
                    area = new TutorialAreaRunner(x, y, w, h, type, name, key, description);
                    break;
                case "Catch":
                    var cath = model as Xml2PrefabArrestAreaContainer;
                    var distance = cath.Distance;
                    area = new ArrestAreaRunner(x, y, w, h, type, name, distance);
                    break;
                default:
                    area = new AreaRunner(Core.Location.AreaRunner.AreaType.None, x, y, w, h, type, name);
                    break;
            }
            area.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transformations);
            area.SetXmlListSerialized(node);
            return area;
        }

        private VisualRunner CreateVisual(Xml2PrefabVisualRunnerContainer model)
        {
            var x = model.X;
            var y = model.Y;
            var w = model.ImageWidth;
            var h = model.ImageHeight;
            var type = model.Type;
            var color = model.DefaultColor;
            var name = model.Name;
            var depth = model.Depth;
            var matrix = XmlUtils.OpenXMLElementFromString(model.MatrixTransform);
            var visual = new VisualRunner(type, name, new Pointd(x, y), w, h, color, depth, matrix);
            visual.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transforms);
            visual.SetXmlListSerialized(node);
            return visual;
        }

        private PlatformRunner CreatePlatform(Xml2PrefabPlatformContainer model)
        {
            var x = model.X;
            var y = model.Y;
            var w = model.W;
            var h = model.H;
            var sticky = model.Sticky;
            var name = model.Name;
            var platform = new PlatformRunner(name, x, y, w, h, sticky);
            platform.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transformations);
            platform.SetXmlListSerialized(node);
            return platform;
        }

        private TrapezoidRunner CreateTrapezoid(Xml2PrefabTrapezoidContainer model)
        {
            var name = model.ClassName;
            var x = model.X;
            var y = model.Y;
            var w = model.Width;
            var h = model.Height;
            var h1 = model.Height1;
            var type = model.Type;
            var sticky = model.Sticky;
            var trapezoid = new TrapezoidRunner(name, type, x, y, w, h, h1, sticky);
            trapezoid.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transforms);
            trapezoid.SetXmlListSerialized(node);
            return trapezoid;
        }

        private ItemRunner CreateItem(Xml2PrefabItemContainer model)
        {
            ItemRunner item = null;
            var x = model.X;
            var y = model.Y;
            var type = model.Type;
            switch (type)
            {
                case 0:
                    var itemscore = model as Xml2PrefabItemScoreContainer;
                    var prefabname = itemscore.PrefabName;
                    var score = itemscore.Score;
                    item = new ItemScoreRunner(type, prefabname, score, x, y);
                    break;
                default:
                    var coin = model as Xml2PrefabCoinContainer;
                    var score1 = coin.Score;
                    var groupId = coin.GroupId;
                    item = new CoinRunner(type, "CoinItem", groupId, score1, x, y);
                    break;
            }
            item.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.TransformationData);
            item.SetXmlListSerialized(node);
            return item;
        }

        private PrimitiveRunner CreatePrimitive(Xml2PrefabPrimitiveContainer model)
        {
            var delta = new Vector3f(model.X, model.Y, 0);
            var type = model.Type;
            var name = model.ClassName;
            var impulse = model.Impulse;
            var sounds = model.Sounds;
            PrimitiveRunner primitive = null;

            if (type == 0)
            {
                primitive = new PrimitiveRunner(type, name, Color.black, delta, impulse, sounds);
            }
            else
            {
                primitive = new PrimitiveAnimatedRunner(type, name, Color.black, delta, impulse, sounds);
            }
            primitive.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transforms);
            primitive.SetXmlListSerialized(node);
            return primitive;
        }

        private ParticleRunner CreateParticle(Xml2PrefabParticleContainer model)
        {
            var x = model.X;
            var y = model.Y;
            var w = model.W;
            var h = model.H;
            var name = model.Name;
            var particle = new ParticleRunner(x, y, w, h, name);
            particle.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transforms);
            particle.SetXmlListSerialized(node);
            return particle;
        }

        private AnimationRunner CreateAnimation(Xml2PrefabAnimationContainer model)
        {
            if (model.Name == "p_dust2")
            {
                return null;
            }
            AnimationRunner animation = null;
            if (model.Type == 1)
            {
                var vector = model as Xml2PrefabAnimationVectorContainer;
                var x = vector.X;
                var y = vector.Y;
                var w = vector.W;
                var h = vector.H;
                var scalex = vector.ScaleX;
                var scaley = vector.ScaleY;
                var name = vector.Name;
                var type = vector.Type;
                var life = vector.Time;
                var direction = new Vector2(vector.DirectionX, vector.DirectionY);
                var acceleration = new Vector2(vector.AccelerationX, vector.AccelerationY);

                animation = new AnimationVectorRunner(x, y, w, h, name, type, scalex, scaley, direction, acceleration, life);
            }
            else
            {
                var x = model.X;
                var y = model.Y;
                var w = model.W;
                var h = model.H;
                var scalex = model.ScaleX;
                var scaley = model.ScaleY;
                var name = model.Name;
                var type = model.Type;

                animation = new AnimationRunner(x, y, w, h, name, type, scalex, scaley, false, 2);
            }
            animation.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transformations);
            animation.SetXmlListSerialized(node);
            return animation;
        }

        private CameraRunner CreateCamera(Xml2PrefabCameraContainer model)
        {
            var x = model.X;
            var y = model.Y;
            var animation = model.Animation;
            var name = model.Name;
            var camera = new CameraRunner(x, y, name, animation);
            camera.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transformations);
            camera.SetXmlListSerialized(node);
            return camera;
        }

        private SpawnRunner CreateSpawn(Xml2PrefabSpawnContainer model)
        {
            var x = model.X;
            var y = model.Y;
            var animation = model.Animation;
            var name = model.Name;
            var spawn = new SpawnRunner(x, y, name , animation);
            spawn.Layer = _Parent.Layer;
            var node = XmlUtils.OpenXMLElementFromString(model.Transformations);
            spawn.SetXmlListSerialized(node);
            return spawn;
        }
    }
}
