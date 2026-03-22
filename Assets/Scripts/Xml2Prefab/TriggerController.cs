using System;
using UnityEngine;

namespace Xml2Prefab
{
	[ExecuteInEditMode]
	public class TriggerController : MonoBehaviour
	{
		public Xml2PrefabTriggerContainer Container;

        private static Sprite whiteSprite;
        
        SpriteRenderer  spriteRender;

        public Action OnBecameVisibleEvent = delegate
        {
        };

        public Action OnBecameInvisibleEvent = delegate
        {
        };

        private bool _IsVisible;

        private void Awake()
        {
            if (whiteSprite == null)
            {
                whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero, 1, 0, SpriteMeshType.FullRect);
            }
        }

        private void Start()
        {
            if (Container == null)
            {
                Container = GetComponent<Xml2PrefabTriggerContainer>();
            }
            
            spriteRender = GetComponent<SpriteRenderer>();
            if (spriteRender == null)
            {
                spriteRender = gameObject.AddComponent<SpriteRenderer>();
                spriteRender.sortingOrder = 1;
            }
            if (spriteRender.sprite == null)
            {
                spriteRender.sprite = whiteSprite;
            }
            transform.localScale = new Vector3(Container.W, Container.H);
            
            Color color = new Color(0.5f, 1, 1, 0.2f);
            if (Container.Choice == null || string.IsNullOrEmpty(Container.Choice.Variant) || Container.Choice.Variant == "CommonMode")
            {
                color = new Color(1, 0, 0, 0.2f);
            }

            var a = !Application.isPlaying || Game.Instance == null || Game.Instance.SnailSett.ShowTriggers ? 0.2f : 0f;
            color.a = a;

            spriteRender.color = color;
        }

        private void OnBecameVisible()
        {
            if (LevelMainController.current != null && LevelMainController.current.Location != null && !_IsVisible)
            {
                _IsVisible = true;
                OnBecameVisibleEvent?.Invoke();
            }
        }

        private void OnBecameInvisible()
        {
            if (LevelMainController.current != null && LevelMainController.current.Location != null && _IsVisible)
            {
                _IsVisible = false;
                OnBecameInvisibleEvent?.Invoke();
            }
        }

    }
}
