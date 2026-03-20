using DG.Tweening;
using Nekki.Vector.Core.Scripts;
using UnityEngine;
using UnityEngine.U2D;

public class ItemGO : MonoBehaviour
{
    [SerializeField]
    private SpriteAtlas _atlas;

    [SerializeField]
    private SpriteAtlas _atlasEnd;

    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private AnimationSprite _animationSprite;

    [SerializeField]
    private TextMesh _textMesh;

    private Vector3 _defaultTextPos;

    public Color color
    {
        set
        {
            _spriteRenderer.color = value;
        }
    }

    public void Init()
    {
        _defaultTextPos = _textMesh.transform.localPosition;
        Reset();
    }

    public void PlayEnd(int scope)
    {
        _textMesh.text = "+" + scope;
        _animationSprite.Init(_atlasEnd, _spriteRenderer);
        _animationSprite.Iterations = 1;
        _textMesh.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        var tween = ShortcutExtensions.DOLocalMoveY(_textMesh.transform, _textMesh.transform.localPosition.y + -200, 1, false);
        TweenSettingsExtensions.Append(sequence, tween);
        var callback = new TweenCallback(() => _textMesh.gameObject.SetActive(false));
        TweenSettingsExtensions.AppendCallback(sequence, callback);
        TweenExtensions.Play(sequence);
    }

    public void Reset()
    {
        _animationSprite.Init(_atlas, _spriteRenderer);
        _animationSprite.IsWork = true;
        _animationSprite.Iterations = -1;
        _textMesh.gameObject.SetActive(false);
        _textMesh.transform.localPosition = _defaultTextPos;
    }
}
