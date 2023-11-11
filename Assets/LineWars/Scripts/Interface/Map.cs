using UnityEngine;

namespace LineWars.Interface
{
    public class Map : MonoBehaviour
    {
        private static Map Instance { get; set; }
        private static SpriteRenderer mapSpriteRenderer;

        public static SpriteRenderer MapSpriteRenderer => Instance != null ? mapSpriteRenderer : null;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                mapSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}