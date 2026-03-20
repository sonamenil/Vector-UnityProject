using System.Collections.Generic;

public class BufferFrames : KeyFrames
{
	public bool IsBufferEmpty => _Size == _ActiveFrame;

	public BufferFrames() : base()
	{

	}
	public new void Reset()
	{
		_Size = _ActiveFrame;
	}

	public void InitBuffer(int p_frames)
	{
		_Size = p_frames;
		_ActiveFrame = 0;
		if (_Data.Count < p_frames)
		{
			ResizeData(p_frames - _Data.Count, 46);
		}
	}

	public void VelocityBuffer(Vector3d p_point)
	{
        for (int i = _ActiveFrame; i < _Size; i++)
        {
            List<Vector3d> list = _Data[i];
            for (int j = 0; j < list.Count; j++)
            {
                list[j].Add(p_point);
            }
        }
    }
}
