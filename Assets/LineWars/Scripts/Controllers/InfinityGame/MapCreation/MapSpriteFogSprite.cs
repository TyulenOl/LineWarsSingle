using System;
using UnityEngine;
using UnityEngine.U2D;

namespace LineWars.Controllers
{
    [Serializable]
    public class MapSpriteFogSprite
    {
        [SerializeField, ReadOnlyInspector] private string mapPath;
        [SerializeField, ReadOnlyInspector] private string fogPath;

#if UNITY_EDITOR
        [SerializeField] private Sprite mapSprite;
        [SerializeField] private Sprite fogSprite;

        public void OnValidate()
        {
            mapPath = mapSprite == null ? "" : EditorExtensions.GetAssetResourcePath(mapSprite);
            fogPath = fogSprite == null ? "" : EditorExtensions.GetAssetResourcePath(fogSprite);
        }
#endif
        public Sprite MapSprite => Resources.Load<Sprite>(mapPath);
        public Sprite FogSprite => Resources.Load<Sprite>(fogPath);
    }
}