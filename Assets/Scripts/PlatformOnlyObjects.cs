using UnityEngine;

public class PlatformOnlyObjects : MonoBehaviour
{
    enum Platform
    {
        PC,
        Mobile
    }

    [SerializeField]
    private GameObject[] _gameObjects;

    [SerializeField]
    private Platform _platform;

    private void OnEnable()
    {
        var currentPlatform = Application.isMobilePlatform ? Platform.Mobile : Platform.PC;

        foreach (GameObject go in _gameObjects)
        {
            if (currentPlatform == _platform)
            {
                go.SetActive(true);
            }
            else
            {
                go.SetActive(false);
            }
        }
    }
}
