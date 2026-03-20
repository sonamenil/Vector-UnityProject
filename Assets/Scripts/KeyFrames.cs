using System.Collections.Generic;
using UnityEngine;

public class KeyFrames
{
    protected List<List<Vector3d>> _Data = new List<List<Vector3d>>();

    protected int _Size;

    protected int _ActiveFrame;

    public int Size => _Size;

    public int ActiveFrame => _ActiveFrame;

    public int SizeMinusActiveFrame => _Size - _ActiveFrame;

    public KeyFrames()
    {
        ResizeData(2, 46);
    }

    public void NextActiveFrame()
    {
        _ActiveFrame++;
    }

    public void InterruptFramesSeted()
    {
        _Size = 2;
    }

    public List<Vector3d> GetFrame(int p_index)
    {
        if (_Size <= p_index)
        {
            return null;
        }
        return _Data[p_index];
    }

    public List<Vector3d> GetActiveFrame(int p_shift)
    {
        return _Data[_ActiveFrame + p_shift];
    }

    public void Reset()
    {
        _Size = 0;
        _ActiveFrame = 0;
    }

    public void SetFrames(int p_from, int p_to, Vector3[][] p_data)
    {
        ResizeIfNeed(p_data[p_from].Length);
        for (int i = p_from; i <= p_to; i++)
        {
            SetFrame(p_data[i]);
        }
    }

    public void ResizeIfNeed(int size)
    {
        for (int i = 0; i < _Data.Count; i++)
        {
            int num = size - _Data[i].Count;
            if (num > 0)
            {
                for (int j = num; j > 0; j--)
                {
                    _Data[i].Add(new Vector3d());
                }
            }
            if (num < 0)
            {
                for (int k = -num; k > 0; k--)
                {
                    _Data[i].RemoveAt(_Data[i].Count - 1);
                }
            }
        }
    }

    public void SetFrame(Vector3[] p_data)
    {
        _Size++;
        if (_Data.Count < _Size)
        {
            ResizeData(1, p_data.Length);
        }
        var data = _Data[_Size - 1];
        if (data.Count != p_data.Length)
        {
            Debug.LogError("Input and data size don't match");
            return;
        }
        if (data.Count > 0)
        {
            for (int i = 0; i < data.Count; i++)
            {
                data[i].Set(p_data[i]);
            }
        }
    }

    protected void ResizeData(int p_count, int p_size)
    {
        for (int i = 0; i < p_count; i++)
        {
            var list = new List<Vector3d>(p_size);
            _Data.Add(list);
            if (p_size > 0)
            {
                for (int j = 0; j < p_size; j++)
                {
                    list.Add(new Vector3d());
                }
            }
        }
    }
}
