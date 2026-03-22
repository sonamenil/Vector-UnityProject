using System.Collections.Generic;
using Nekki.Vector.Core.Models;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class BotIcon : MonoBehaviour
{
    [SerializeField]
    private RectTransform _botIcon;

    [SerializeField]
    private RectTransform _canvasRect;

    [SerializeField]
    private Camera _camera;

    private UIFlippable _botIconFlip;

    private ModelHuman _botModel;

    private int _sign;

    private bool _isVisible;

    public void Init(List<ModelHuman> botModels)
    {
        _botModel = null;
        SetBotIconVisible(true);
        _botIconFlip = _botIcon.GetComponent<UIFlippable>();
        foreach (var bot in botModels)
        {
            if (bot.UserData.isIcon)
            {
                _botModel = bot;
            }
        }
        if (_botModel == null)
        {
            SetBotIconVisible(false);
        }
    }

    public void Render()
    {
        if (_botModel == null)
            return;

        if (!_botModel.IsEnabled)
        {
            SetBotIconVisible(false);
            return;
        }
        Vector2 iconWorldPos = _botModel.GetPositionForIcon();
        Vector3 viewportPos = _camera.WorldToViewportPoint(new Vector3(iconWorldPos.x, iconWorldPos.y, 0));
        Vector2 viewportXY = new Vector2(viewportPos.x, viewportPos.y);

        if (!(viewportXY.x <= 0f || viewportXY.x >= 1f))
        {
            SetBotIconVisible(false);
            return;
        }

        SetBotIconVisible(true);

        int newSign = viewportXY.x < 0 ? 0 : 1;

        if (newSign != _sign)
        {
            float num = _sign > 0 ? 0 : 1;
            _sign = newSign;

            Vector2 anchor = new Vector2(num, 0.5f);
            _botIcon.anchorMin = anchor;
            _botIcon.anchorMax = anchor;
            _botIcon.pivot = anchor;

            _botIconFlip.horizontal = _sign > 0;
        }

        float clampedY = Mathf.Clamp01(viewportXY.y);

        Vector2 canvasSize = _canvasRect.sizeDelta;
        float anchoredY = clampedY * canvasSize.y - canvasSize.y * 0.5f;

        _botIcon.anchoredPosition = new Vector2(0f, anchoredY);
    }


    private void SetBotIconVisible(bool value)
    {
        _isVisible = value;
        _botIcon.gameObject.SetActive(_isVisible);
    }
}
