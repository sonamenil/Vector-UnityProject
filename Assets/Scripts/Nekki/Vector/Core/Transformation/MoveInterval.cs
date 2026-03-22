using System;
using System.Collections.Generic;
using System.Xml;
using Nekki.Vector.Core.Location;

namespace Nekki.Vector.Core.Transformation
{
    public class MoveInterval
    {
        private int _Number;

        private int _Frames;

        private float _Delay;

        private float _CurrentDelay;

        private float _CurrentDelayFake;

        private List<Point> _Points = new List<Point>();

        private int _CurrentFrame;

        private int _CurrentFrameFake;

        public int Number
        {
            get => _Number;
            set => _Number = value;
        }

        public int Frames
        {
            get => _Frames;
            set => _Frames = value;
        }

        public float Delay
        {
            get => _Delay;
            set
            {
                _Delay = value;
                _CurrentDelay = value;
            }
        }

        public List<Point> Points => _Points;

        public int CurrentFrame => _CurrentFrame;

        public MoveInterval()
        {
        }

        public MoveInterval(int number, int frames, float delay)
        {
            _Number = number;
            _Frames = frames;
            _CurrentFrame = 0;
            _Delay = delay;
            _CurrentDelay = _Delay;
        }

        public void Reset()
        {
            _CurrentFrame = 0;
            _CurrentDelay = _Delay;
        }

        public void Init()
        {
        }

        public void CalculateExtraPoints()
        {
            Point point = _Points[0];
            Point point1 = _Points[_Points.Count - 1];
            List<Point> list = new List<Point>();
            for (int i = 1; i < _Points.Count - 1; i++)
            {
                list.Add(_Points[i]);
            }
            _Points.Clear();
            float num = 0f;
            for (int j = 0; j < _Frames; j++)
            {
                double num2 = Math.Pow(1f - num, list.Count + 1);
                double num3 = num2 * point.X;
                double num4 = num2 * point.Y;
                int num5 = 0;
                int num6 = 0;
                for (num5 = 0; num5 < list.Count; num5++)
                {
                    var n = Math.Pow(1 - num, list.Count + num6);
                    var n1 = Math.Pow(num, (double)num5 + 1);
                    var p = list[num5];
                    n1 = n * (list.Count + 1) * n1;
                    num3 = num3 + n1 * p.X;
                    num4 = num4 + n1 * p.Y;
                    num6--;
                }
                var n2 = Math.Pow(num, num5 + 1);
                num3 = num3 + n2 * point1.X;
                num4 = num4 + n2 * point1.Y;
                var p1 = new Point((float)num3, (float)num4);
                _Points.Add(p1);
                num = num + 1 / (float)_Frames;
            }
            _Points.Add(point1);

            for (int num2 = _Points.Count - 1; num2 > 0; num2--)
            {
                _Points[num2].Subtract(_Points[num2 - 1]);
                _Points[num2].Round(100);
            }
            _Points.RemoveAt(0);
        }


        public bool Iteration(Runner runner)
        {
            if (_CurrentDelay <= 0)
            {
                runner.UpdatePosition(_Points[_CurrentFrame]);

                _CurrentFrame++;
                if (_CurrentFrame < _Points.Count)
                {
                    return true;
                }
                _CurrentFrame = 0;
                _CurrentDelay = _Delay;
                return false;
            }
            runner.TweenPosition.Reset();
            _CurrentDelay--;
            return true;
        }

        public bool InerationFake(ref Point p_point, ref Vector3f p_velocity, bool p_isInit)
        {
            if (p_isInit)
            {
                _CurrentDelayFake = _CurrentDelay;
                _CurrentFrameFake = _CurrentFrame;
            }
            if (_CurrentDelayFake <= 0)
            {
                var currentPoint = _Points[_CurrentFrameFake];
                if (currentPoint != null && p_velocity != null)
                {
                    p_velocity.Set(currentPoint.X, currentPoint.Y);
                    p_point.Add(currentPoint);
                    _CurrentFrameFake++;
                    if (_CurrentFrameFake < _Points.Count)
                    {
                        return true;
                    }
                    _CurrentDelayFake = _Delay;
                    _CurrentFrameFake = 0;
                    return false;
                }
            }
            _CurrentDelayFake--;
            return true;
        }

        public static MoveInterval Create(XmlNode node)
        {
            var moveInterval = new MoveInterval();
            moveInterval._Number = node.Attributes["Number"].ParseInt();
            moveInterval._Frames = node.Attributes["FramesToMove"].ParseInt();
            var delay = node.Attributes["Delay"].ParseFloat();
            moveInterval._Delay = delay;
            moveInterval._CurrentDelay = delay;
            foreach (XmlNode node2 in node.ChildNodes)
            {
                var point = Point.Create(node2);
                if (point != null)
                {
                    moveInterval._Points.Add(point);
                }
            }
            moveInterval.CalculateExtraPoints();
            return moveInterval;
        }

        public int DeltaBySizeAndCurrentPoint()
        {
            return _Points.Count + ~_CurrentFrame;
        }

        public MoveInterval Clone()
        {
            var clone = new MoveInterval(_Number, _Frames, _Delay);
            foreach (var point in _Points)
            {
                clone._Points.Add(point.Clone());
            }
            return clone;
        }
    }
}
