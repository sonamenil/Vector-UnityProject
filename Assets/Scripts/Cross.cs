namespace Nekki.Vector.Core.Result
{
    public struct Cross
    {
        private int _Index;

        private Vector3d _Point;

        public int Index => _Index;

        public Vector3d Point => _Point;

        public Cross(Vector3d point, int index)
        {
            _Index = index;
            _Point = point;
        }
    }
}
