using System;
using UnityEngine;

namespace LineWars.Controllers
{
    public class UnitBackgroundDrawer : MonoBehaviour
    {
        private class Save
        {
            public Sprite MaskSprite;
            public SpriteMaskInteraction BackgroundMaskInteraction;
            public SpriteMaskInteraction UnitMaskInteraction;

            public int BackgroundSortingOrder;
            public int UnitSortingOrder;

            public Vector3 BackgroundScale;
            public Vector3 UnitScale;

            public Vector3 UnitPosition;
        }


        [SerializeField] private Sprite nodeSprite;

        [SerializeField] private SpriteMask mask;

        [SerializeField] private SpriteRenderer unitSpriteRenderer;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        private Sprite leftPart;
        private Sprite rightPart;

        private Save save;

        private Sprite unitSprite => unitSpriteRenderer.sprite;

        public void Initialize()
        {
            CreateSave();
            Texture2D nodeTexture = nodeSprite.texture;
            float width = nodeTexture.width;
            float height = nodeTexture.height;

            //var leftTexture = new Texture2D(width, height, nodeTexture.format, true);
            //var rightTexture = new Texture2D(width, height, nodeTexture.format, true);

            //Graphics.CopyTexture(nodeTexture, 0, 0, 0, 0, width / 2, height, leftTexture, 0, 0, 0, 0);
            //Graphics.CopyTexture(nodeTexture, 0, 0, width / 2, 0, width / 2, height, rightTexture, 0, 0, width / 2, 0);

            leftPart = Sprite.Create(nodeTexture, new Rect(0, 0, width / 2, height), new Vector2(1, 0.5f));
            rightPart = Sprite.Create(nodeTexture, new Rect(width / 2, 0, width / 2, height), new Vector2(0, 0.5f));

            backgroundSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            backgroundSpriteRenderer.sortingOrder = 0;

            unitSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            unitSpriteRenderer.sortingOrder = 1;
        }

        public void Disable()
        {
            mask.sprite = save.MaskSprite;

            backgroundSpriteRenderer.maskInteraction = save.BackgroundMaskInteraction;
            backgroundSpriteRenderer.sortingOrder = save.BackgroundSortingOrder;

            unitSpriteRenderer.maskInteraction = save.UnitMaskInteraction;
            unitSpriteRenderer.sortingOrder = save.UnitSortingOrder;

            backgroundSpriteRenderer.transform.localScale = save.BackgroundScale;
            unitSpriteRenderer.transform.localScale = save.UnitScale;

            unitSpriteRenderer.transform.position = save.UnitPosition;
        }

        private void CreateSave()
        {
            save = new Save()
            {
                MaskSprite = mask.sprite,

                BackgroundMaskInteraction = backgroundSpriteRenderer.maskInteraction,
                BackgroundSortingOrder = backgroundSpriteRenderer.sortingOrder,

                UnitMaskInteraction = unitSpriteRenderer.maskInteraction,
                UnitSortingOrder = unitSpriteRenderer.sortingOrder,

                BackgroundScale = backgroundSpriteRenderer.transform.localScale,
                UnitScale = unitSpriteRenderer.transform.localScale,
                
                UnitPosition = unitSpriteRenderer.transform.position
            };
        }


        private void ScaleForNode(float width, float height, SpriteRenderer renderer)
        {
            var rect = renderer.sprite.rect;
            var localScale = renderer.transform.localScale;

            var scaleFactorForWidth = width / (rect.width);
            var scaleFactorForHeight = height / (rect.height);

            var scaleFactor = Mathf.Min(scaleFactorForWidth, scaleFactorForHeight);

            localScale = Vector3.one * scaleFactor;
            renderer.transform.localScale = localScale;
        }


        public void DrawLeft()
        {
            ScaleForNode(nodeSprite.rect.width / 2, nodeSprite.rect.height, unitSpriteRenderer);
            mask.sprite = leftPart;
            
            unitSpriteRenderer.transform.localPosition =
                Vector3.left * unitSprite.rect.width / 4 / unitSprite.pixelsPerUnit;
        }

        public void DrawCenter()
        {
            ScaleForNode(nodeSprite.rect.width, nodeSprite.rect.height, unitSpriteRenderer);
            
            mask.sprite = nodeSprite;
            unitSpriteRenderer.transform.localPosition = Vector3.zero;
        }

        public void DrawRight()
        {
            ScaleForNode(nodeSprite.rect.width / 2, nodeSprite.rect.height, unitSpriteRenderer);

            mask.sprite = rightPart;
            unitSpriteRenderer.transform.localPosition =
                Vector3.right * unitSprite.rect.width / 4 / unitSprite.pixelsPerUnit;
        }
    }
}