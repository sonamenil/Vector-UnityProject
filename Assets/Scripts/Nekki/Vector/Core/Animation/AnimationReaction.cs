using Nekki.Vector.Core.Detector;
using Nekki.Vector.Core.Location;
using Nekki.Vector.Core.Models;
using Nekki.Vector.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using static Nekki.Vector.Core.Node.NodeName;

namespace Nekki.Vector.Core.Animation
{
    public class AnimationReaction
    {
        public const string NameDeath = "Death";

        private AnimationInfo _info;

        private readonly List<int> _slopeH = new List<int>();

        private readonly List<int> _slopeV = new List<int>();

        private readonly int _triggerNameHash;

        private static Random _Random;

        public static List<AnimationReaction> Reactions
        {
            get;
        }

        public string Name
        {
            get;
            set;
        }

        public int FirstFrame
        {
            get;
            set;
        }

        public int Priority
        {
            get;
            set;
        }

        public List<int> Sides
        {
            get;
            set;
        }

        public AnimationDeltas DeltasH
        {
            get;
        }

        public AnimationDeltas DeltasV
        {
            get;
        }

        public AnimationDeltas DeltasC
        {
            get;
        }

        public bool CornerCling
        {
            get;
            set;
        }

        public int CornerClingV
        {
            get;
            set;
        }

        public int CornerClingH
        {
            get;
            set;
        }

        public string PivotNode
        {
            get;
            set;
        }

        public bool SafeHorizontal
        {
            get;
            set;
        }

        public bool SafeVertical
        {
            get;
            set;
        }

        public bool Mirror
        {
            get;
        }

        public bool Reverse
        {
            get;
            set;
        }

        public float InsideH
        {
            get;
            set;
        }

        public float InsideV
        {
            get;
            set;
        }

        public string TriggerName
        {
            get;
        }

        public List<string> NodesWi
        {
            get;
            set;
        }

        public bool OnEndTrigger
        {
            get;
        }

        private AnimationInfo Info
        {
            get
            {
                if (_info == null)
                {
                    _info = Animations.Animation[Name];
                }
                return _info;
            }
        }

        public bool IsAnimationArrest
        {
            get
            {
                if (Animations.Animation.ContainsKey(Name) && Animations.Animation[Name].Type == 5)
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsTrick
        {
            get
            {
                if (Animations.Animation.ContainsKey(Name) && Animations.Animation[Name].Type == 3)
                {
                    return true;
                }
                return false;
            }
        }

        static AnimationReaction()
        {
            Reactions = new List<AnimationReaction>();
            _Random = new Random();
        }

        public AnimationReaction(string name, int frame)
        {
            Sides = new List<int>();
            NodesWi = new List<string>();

            Name = name;
            FirstFrame = frame;
        }

        public AnimationReaction(XmlNode node)
        {
            Sides = new List<int>();
            NodesWi = new List<string>();
            Name = node.Name;
            FirstFrame = XmlUtils.ParseInt(node.Attributes["FirstFrame"]);
            Priority = XmlUtils.ParseInt(node.Attributes["Priority"]);
            CornerCling = XmlUtils.ParseBool(node.Attributes["CornerCling"]);
            CornerClingV = XmlUtils.ParseInt(node.Attributes["CornerClingV"], -1);
            CornerClingH = XmlUtils.ParseInt(node.Attributes["CornerClingH"], -1);
            SafeHorizontal = XmlUtils.ParseBool(node.Attributes["SafeH"]);
            SafeVertical = XmlUtils.ParseBool(node.Attributes["SafeV"]);
            Reverse = XmlUtils.ParseBool(node.Attributes["Reverse"]);
            Mirror = XmlUtils.ParseBool(node.Attributes["Mirror"]);
            OnEndTrigger = XmlUtils.ParseBool(node.Attributes["OnEndTrigger"]);
            PivotNode = XmlUtils.ParseString(node.Attributes["PivotNode"]);
            if (node.Attributes["TriggerName"] != null)
            {
                TriggerName = node.Attributes["TriggerName"].Value;
                _triggerNameHash = TriggerName.GetHashCode();
            }
            if (node.Attributes["AreaName"] != null)
            {
                TriggerName = node.Attributes["AreaName"].Value;
                _triggerNameHash = TriggerName.GetHashCode();
            }
            if (node.Attributes["Sides"] != null)
            {
                string[] sides = node.Attributes["Sides"].Value.Split('|');
                foreach (string side in sides)
                {
                    Sides.Add(int.Parse(side));
                }
            }
            if (node.Attributes["SlopeH"] != null)
            {
                string[] array2 = node.Attributes["SlopeH"].Value.Split('|');
                foreach (string s2 in array2)
                {
                    _slopeH.Add(int.Parse(s2));
                }
            }
            if (node.Attributes["SlopeV"] != null)
            {
                string[] array3 = node.Attributes["SlopeV"].Value.Split('|');
                foreach (string s3 in array3)
                {
                    _slopeV.Add(int.Parse(s3));
                }
            }
            if (node.Attributes["NodesWI"] != null)
            {
                NodesWi = new List<string>(node.Attributes["NodesWI"].Value.Split('|'));
            }
            InsideH = XmlUtils.ParseFloat(node.Attributes["InsideH"], float.NaN);
            InsideV = XmlUtils.ParseFloat(node.Attributes["InsideV"], float.NaN);
            DeltasH = new AnimationDeltas(node, AnimationDeltaType.Horizontal);
            DeltasV = new AnimationDeltas(node, AnimationDeltaType.Vertical);
            DeltasC = new AnimationDeltas(node, AnimationDeltaType.Collision);
            Reactions.Add(this);
            _info = null;
        }

        public bool CheckNameHash(int hash)
        {
            return _triggerNameHash == hash;
        }

        public static bool IsTriggerType(AnimationReaction reaction, AreaRunner area, ModelHuman model)
        {
            if (area.IsTrick)
            {
                if ((area as TrickAreaRunner).isActive && model.IsTrick)
                {
                    return reaction.CheckNameHash(area.NameHash);
                }
                return false;
            }
            if (area.IsCatch)
            {
                if (area.IsEnabled)
                {
                    return (area as ArrestAreaRunner).IsArrest && reaction.CheckNameHash(area.NameHash);
                }
                return false;
            }
            return reaction.CheckNameHash(area.NameHash);
        }

        public static AnimationReaction GetReaction(List<List<AnimationReaction>> Reactions, AnimationDeltaData DeltaH, AnimationDeltaData DeltaV, AnimationDeltaData DeltaC, List<AreaRunner> Areas, ModelHuman Model, Vector3d Velocity)
        {
            var sort = ToList(Reactions);
            sort = Sort(sort, Model);
            sort = Sort(sort, Areas, Model);
            sort = Sort(sort, DeltaH, DeltaV, DeltaC, Model);
            sort = Sort(sort, DeltaH, DeltaV, DeltaC, Model.Sign, Velocity);
            sort = Sort(sort);
            return Random(sort);
        }

        public static List<AnimationReaction> ToList(List<List<AnimationReaction>> Animations)
        {
            List<AnimationReaction> list = new List<AnimationReaction>();
            foreach (var animation in Animations)
            {
                if (animation != null)
                    list.AddRange(animation);
            }
            return list;
        }

        public static List<AnimationReaction> Sort(List<AnimationReaction> Reactions, List<AreaRunner> Areas, ModelHuman Model)
        {
            for (int num = Reactions.Count - 1; num >= 0; num--)
            {
                if (Reactions[num].TriggerName != null)
                {
                    bool flag = false;
                    foreach (AreaRunner p_area in Areas)
                    {
                        if (IsTriggerType(Reactions[num], p_area, Model))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        Reactions.Remove(Reactions[num]);
                    }
                }
            }
            return Reactions;
        }

        public static List<AnimationReaction> Sort(List<AnimationReaction> reactions, ModelHuman model)
        {
            DetectorLine detectorHorizontalLine = model.ModelObject.DetectorHorizontalLine;
            DetectorLine detectorVerticalLine = model.ModelObject.DetectorVerticalLine;
            for (int num = reactions.Count - 1; num >= 0; num--)
            {
                if (!reactions[num].IsInside(detectorHorizontalLine, detectorVerticalLine, model.Sign))
                {
                    reactions.RemoveAt(num);
                }
            }
            return reactions;
        }

        public static List<AnimationReaction> Sort(List<AnimationReaction> Reactions, AnimationDeltaData DeltaH, AnimationDeltaData DeltaV, AnimationDeltaData DeltaС, ModelHuman Model)
        {
            for (int i = Reactions.Count - 1; i >= 0; i--)
            {
                var reaction = Reactions[i];
                if (!reaction.IsSlope(DeltaH, DeltaV, DeltaС, Model.Sign))
                {
                    Reactions.RemoveAt(i);
                }
            }
            return Reactions;
        }

        public static List<AnimationReaction> Sort(List<AnimationReaction> Reactions, AnimationDeltaData DeltaH, AnimationDeltaData DeltaV, AnimationDeltaData DeltaС, int Sign, Vector3d Velocity)
        {
            for (int i = Reactions.Count - 1; i >= 0; i--)
            {
                var reaction = Reactions[i];
                if (!reaction.IsDeltaCheck(DeltaH, DeltaV, DeltaС, Sign, Velocity))
                {
                    Reactions.RemoveAt(i);
                }
            }
            return Reactions;
        }

        public static List<AnimationReaction> Sort(List<AnimationReaction> Reactions)
        {
            var maxPriority = MaxPriority(Reactions);
            for (int i = Reactions.Count - 1; i >= 0; i--)
            {
                var reaction = Reactions[i];
                if (reaction.Priority != maxPriority)
                {
                    Reactions.RemoveAt(i);
                }
            }
            return Reactions;
        }

        public static AnimationReaction Random(List<AnimationReaction> Reactions)
        {
            return (Reactions.Count != 0) ? Reactions[_Random.Next(0, Reactions.Count - 1)] : null;
        }

        public static int MaxPriority(List<AnimationReaction> Reactions)
        {
            int num = 0;
            for (int i = 0; i < Reactions.Count; i++)
            {
                num = Math.Max(Reactions[i].Priority, num);
            }
            return num;
        }

        public bool IsSide(int Value, int Sign)
        {
            if (Sides.Count == 0 || Value == -1)
            {
                return true;
            }
            for (int i = 0; i < Sides.Count; i++)
            {
                int num = Sides[i];
                if (Sign == -1)
                {
                    switch (num)
                    {
                        case 1:
                            num = 3;
                            break;
                        case 3:
                            num = 1;
                            break;
                    }
                }
                if (num == Value)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsSlope(AnimationDeltaData DeltaH, AnimationDeltaData DeltaV, AnimationDeltaData DeltaC, int Sign)
        {
            return IsDeltaSlope(DeltaH, _slopeH, Sign) && IsDeltaSlope(DeltaV, _slopeV, Sign);
        }

        public bool IsDeltaSlope(AnimationDeltaData Delta, List<int> Slopes, int Sign)
        {
            if (Delta == null || Slopes == null || Slopes.Count == 0)
            {
                return true;
            }
            int type = Delta.Platform.Type;
            foreach (int p_slope in Slopes)
            {
                int num;
                if (Sign == 1)
                {
                    num = p_slope;
                }
                else
                {
                    switch (p_slope)
                    {
                        case 1:
                            num = 2;
                            break;
                        case 2:
                            num = 1;
                            break;
                        default:
                            num = p_slope;
                            break;
                    }
                }
                if (num == type)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsInside(DetectorLine DetectorH, DetectorLine DetectorV, int Sign)
        {
            return Inside(InsideH, DetectorH) && Inside(InsideV, DetectorV);
        }

        public bool Inside(float Inside, DetectorLine Detector)
        {
            if (float.IsNaN(Inside))
            {
                return true;
            }
            if (Detector.Platform == null)
            {
                return Inside == 0f;
            }
            return Inside == 1f;
        }

        public bool IsDeltaCheck(AnimationDeltaData deltaH, AnimationDeltaData deltaV, AnimationDeltaData deltaС, int Sign, Vector3d Velocity)
        {
            bool flag = DeltasH.IsCheck(deltaH, Sign, Velocity);
            bool flag2 = DeltasV.IsCheck(deltaV, Sign, Velocity);
            bool flag3 = DeltasC.IsCheck(deltaС, Sign, Velocity);
            return flag && flag2 && flag3;
        }
    }
}
