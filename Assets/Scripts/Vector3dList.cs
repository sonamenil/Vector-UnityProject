using System.Collections.Generic;

public class Vector3dList
{
    private List<Vector3d> _list = new List<Vector3d>();

    private int _size;

    public int size => _size;

    public Vector3d this[int index]
    {
        get
        {
            if (_size <= index)
            {
                throw new System.IndexOutOfRangeException();
            }
            return _list[index];
        }
    }

    public void Clear()
    {
        _size = 0;
    }

    public void Add(Vector3d vector)
    {
        if (_size == _list.Count)
        {
            _list.Add(new Vector3d(vector));
        }
        else
        {
            _list[_size].Set(vector);
        }
        _size++;
    }
}
