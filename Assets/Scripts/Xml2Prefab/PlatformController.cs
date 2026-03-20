using System;
using UnityEngine;

namespace Xml2Prefab
{
	[ExecuteInEditMode]
	public class PlatformController : MonoBehaviour
	{
		public Xml2PrefabPlatformContainer Container;

        private static Sprite whiteSprite;
        
        SpriteRenderer spriteRender;


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
                Container = GetComponent<Xml2PrefabPlatformContainer>();
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
            
            var a = (!Application.isPlaying || Game.Instance == null || Game.Instance.SnailSett.ShowPlatforms) ? 0.2f : 0f;
            spriteRender.color = new Color(0, 0, 1, a);
        }

	}
}
