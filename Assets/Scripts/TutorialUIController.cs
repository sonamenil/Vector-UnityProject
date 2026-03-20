using DG.Tweening;
using Nekki.Vector.Core.Controllers;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private RectTransform _hand;

    [SerializeField]
    private Image _track;

    [SerializeField]
    private GameObject _GB;

    [SerializeField]
    private GameObject _root;

    private Vector2 _defaultHandPosition;

    private Sequence _sequence;

    private void Awake()
    {
        if (_hand != null)
        {
            _defaultHandPosition = _hand.anchoredPosition;
        }
    }

    public void SetVisible(bool value)
    {
        if (_root != null)
        {
            _root.SetActive(value);
            if (_GB != null)
            {
                _GB.SetActive(value);
            }
        }
    }

    public void ShowKey(KeyVariables key, string description)
    {
        if (_hand != null)
        {
            _hand.anchoredPosition = _defaultHandPosition;
            _track.color = Color.white;

            if (key != null)
            {
                switch (key.Key)
                {
                    case Key.Up:
                        string upText = LocalizationManager.Instance.GetTranslation("help_up_txt");
                        string upDesc = GetDescription(description);

                        _track.transform.localEulerAngles = new Vector3(0, 0, 0);

                        _text.text = upDesc;

                        break;
                    case Key.Down:

                        string downText = LocalizationManager.Instance.GetTranslation("help_down_txt");
                        string downDesc = GetDescription(description);

                        _track.transform.localEulerAngles = new Vector3(0, 0, 180);

                        _text.text = downDesc;

                        break;
                    case Key.Left:
                        string leftText = LocalizationManager.Instance.GetTranslation("help_left_txt");
                        string leftDesc = GetDescription(description);

                        _track.transform.localEulerAngles = new Vector3(0, 0, 90);

                        _text.text = leftDesc;

                        break;
                    case Key.Right:
                        string rightText = LocalizationManager.Instance.GetTranslation("help_right_txt");
                        string rightDesc = GetDescription(description);

                        _track.transform.localEulerAngles = new Vector3(0, 0, -90);

                        _text.text = rightDesc;

                        break;
                }
                //_sequence = DOTween.Sequence();
                //TweenSettingsExtensions.AppendInterval(_sequence, 0.2f);
                //var t = DOTweenModuleUI.DOAnchorPos(_hand, endValue, 0.5f);
                //TweenSettingsExtensions.Append(_sequence, t);
                //var t1 = DOTweenModuleUI.DOFade(_track, 0, 0.5f);
                //TweenSettingsExtensions.Join(_sequence, t1);
                //TweenSettingsExtensions.AppendInterval(_sequence, 0.8f);
                //TweenSettingsExtensions.SetLoops(_sequence, -1);
                //TweenExtensions.Play(_sequence);
                SetVisible(true);
            }
        }
    }

    private string GetDescription(string description)
    {
        return LocalizationManager.Instance.GetTranslation(description);
    }

    public void Reset()
    {
        if (_sequence != null)
        {
            TweenExtensions.Kill(_sequence, false);
            _sequence = null;
        }
    }
}
