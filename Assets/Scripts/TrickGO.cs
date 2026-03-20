using System;
using System.IO;
using Nekki.Vector.Core.Scripts;
using UnityEngine;
using UnityEngine.U2D;

public class TrickGO : MonoBehaviour
{
    [SerializeField]
    private AnimationSprite _animation;

    [SerializeField]
    private SpriteRenderer _animationSprite;

    [SerializeField]
    private SpriteRenderer _trickIcon;

    [SerializeField]
    private SpriteAtlas _idleAnimation;

    [SerializeField]
    private SpriteAtlas _activateAnimation;

    public void Init(string itemName, bool isActive, float w, float h)
    {
        if (_animation != null)
        {
            _animation.gameObject.SetActive(true);
            _trickIcon.gameObject.SetActive(true);
            _animation.Init(_idleAnimation, _animationSprite);
            if (isActive)
            {
                InitActiveState(itemName);
            }
            else
            {
                InitDisableState();
            }
            //if (Xml2PrefabRoot.UseOnlyXML)
            {
                UpdatePosition(w, h);
                UpdateScale(w, h);
            }

        }
    }

    public void UpdatePosition(float w, float h)
    {
        var vector = new Vector3(w * 0.5f / w, -(h * -0.5f / h));
        _animation.transform.localPosition = vector;

        var vector2 = new Vector3(w * 0.5f / w, -((h * -0.5f + -23.28f) / h));
        _trickIcon.transform.localPosition = vector2;
    }

    public void UpdateScale(float w, float h)
    {
        var animScale = _animation.transform.localScale;
        animScale.x = 1 / w;
        animScale.y = 1 / h;
        _animation.transform.localScale = animScale;

        var iconScale = _trickIcon.transform.localScale;
        iconScale.x = 0.9f / w;
        iconScale.y = 0.9f / h;
        _trickIcon.transform.localScale = iconScale;

        _animationSprite.flipY = true;
        _trickIcon.flipY = true;
    }

    private void InitDisableState()
    {
        Sprite sprite = null;
        string path = Application.streamingAssetsPath + "/icons/tricks/lock.png";
        if (File.Exists(path))
        {
            sprite = ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 1);
        }
        else
        {
            sprite = Resources.Load<Sprite>("LevelContent/Tricks/Icons/lock");
        }
        _trickIcon.sprite = sprite;
    }

    private void InitActiveState(string itemName)
    {
        _animation.IsWork = true;
        _animation.Iterations = -1;

        Sprite sprite = null;
        string path = Application.streamingAssetsPath + "/icons/tricks/" + itemName + ".png";
        if (File.Exists(path))
        {
            sprite = ResourceManager.LoadSpriteFromExternal(path, new Vector2(0.5f, 0.5f), 1);
        }
        else
        {
            sprite = Resources.Load<Sprite>("LevelContent/Tricks/Icons/TRACK_" + itemName);
        }
        _trickIcon.sprite = sprite;
    }

    public void RunActivate()
    {
        _animation.Init(_activateAnimation, _animationSprite);
        _animation.Iterations = 1;
        _animation.OnIterationsEnd = new Action(() =>
        {
            _animation.gameObject.SetActive(false);
        }
        );
        _trickIcon.gameObject.SetActive(false);

    }
}
