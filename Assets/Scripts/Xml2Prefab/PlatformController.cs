using UnityEngine;

namespace Xml2Prefab
{
    [ExecuteAlways]
    public class PlatformController : MonoBehaviour
    {
        public Xml2PrefabPlatformContainer Container;

        private static Sprite whiteSprite;
        private SpriteRenderer spriteRender;

        private void Start()
        {
            if (Container != null)
            {
                transform.localScale = new Vector3(Container.W, Container.H, 1f);

            }
        }
        private void OnEnable()
        {
            Refresh();
        }

        private void OnValidate()
        {
            Refresh();
        }

        [ContextMenu("Reload Sprite")]
        private void Refresh()
        {
            if (Container == null)
            {
                Container = GetComponent<Xml2PrefabPlatformContainer>();
            }

            if (Container == null)
            {
                return;
            }

            if (whiteSprite == null)
            {
                whiteSprite = Sprite.Create(
                    Texture2D.whiteTexture,
                    new Rect(0, 0, 1, 1),
                    Vector2.zero,
                    1,
                    0,
                    SpriteMeshType.FullRect
                );
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


            float alpha;

            if (!Application.isPlaying)
            {
                alpha = 0.2f;
            }
            else if (Game.Instance == null)
            {
                alpha = 0.2f;
            }
            else
            {
                alpha = Game.Instance.SnailSett.ShowPlatforms ? 0.2f : 0f;
            }

            spriteRender.color = new Color(0f, 0f, 1f, alpha);
            spriteRender.enabled = true;
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                Refresh();

                if (Container != null)
                {
                    Container.ChangeHW(transform.localScale.y, transform.localScale.x);
                }
            }
        }
    }
}