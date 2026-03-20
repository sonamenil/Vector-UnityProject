using UnityEngine;

public class UIBlocker : MonoBehaviour
{
	private static UIBlocker current;

	[SerializeField]
	private RectTransform _rotator;

	private float _time;

	public static void Block()
	{
		if (current == null)
		{
			Debug.LogError("Can't Block current == null");
			return;
		}
		current.gameObject.SetActive(true);
	}

	public static void UnBlock()
	{
        if (current == null)
        {
            Debug.LogError("Can't UnBlock current == null");
            return;
        }
        current.gameObject.SetActive(false);
    }

	private void Awake()
	{
		current = this;
		gameObject.SetActive(false);
	}

	private void Update()
	{
		_time += Time.deltaTime;
		if (_time < 0.1)
		{
			return;
		}
		_time = 0;
		if (_rotator != null)
		{
			var angle = _rotator.eulerAngles;
			angle.z -= 30;
			_rotator.eulerAngles = angle;

		}
	}
}
