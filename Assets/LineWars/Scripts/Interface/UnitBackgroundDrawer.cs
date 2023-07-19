using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Controllers
{
    [RequireComponent(typeof(Unit))]
    public class UnitBackgroundDrawer : MonoBehaviour
    {

        [SerializeField] private Sprite nodeSprite;

        [SerializeField] private SpriteMask mask;

        [SerializeField] private SpriteRenderer unitSpriteRenderer;
        [SerializeField] private SpriteRenderer backgroundSpriteRenderer;

        private Sprite leftPart;
        private Sprite rightPart;

        private Unit unit;

        public SpriteMask Mask => mask;
        public SpriteRenderer UnitSpriteRenderer => unitSpriteRenderer;
        public SpriteRenderer BackgroundSpriteRenderer => backgroundSpriteRenderer;

        private Sprite unitSprite => unitSpriteRenderer.sprite;

        private void Awake()
        {
            Initialize();
            unit = GetComponent<Unit>();
        }

        private void OnEnable()
        {
            unit.UnitDirectionChance.AddListener(OnUnitDirectionChance);
        }

        private void OnDisable()
        {
            unit.UnitDirectionChance.RemoveListener(OnUnitDirectionChance);
        }

        private void OnUnitDirectionChance(UnitSize size, UnitDirection direction)
        {
            if (size == UnitSize.Little && direction == UnitDirection.Left)
                DrawLeft();
            else if (size == UnitSize.Little && direction == UnitDirection.Right)
                DrawRight();
            else
                DrawCenter();
        }

        public void Initialize()
        {
            Texture2D nodeTexture = nodeSprite.texture;
            float width = nodeTexture.width;
            float height = nodeTexture.height;
            
            leftPart = Sprite.Create(nodeTexture, new Rect(0, 0, width / 2, height), new Vector2(1, 0.5f));
            rightPart = Sprite.Create(nodeTexture, new Rect(width / 2, 0, width / 2, height), new Vector2(0, 0.5f));

            mask.isCustomRangeActive = true;
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
            
            backgroundSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            unitSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }


        public void DrawLeft()
        {
            ScaleForNode(nodeSprite.rect.width / 2, nodeSprite.rect.height, unitSpriteRenderer);
            mask.sprite = leftPart;
            
            backgroundSpriteRenderer.sortingOrder = 1;
            unitSpriteRenderer.sortingOrder = 2;
            mask.frontSortingOrder = 2;
            mask.backSortingOrder = 0;
            
            unitSpriteRenderer.transform.localPosition =
                Vector3.left * unitSprite.rect.width / 2 / unitSprite.pixelsPerUnit;
        }

        public void DrawCenter()
        {
            ScaleForNode(nodeSprite.rect.width, nodeSprite.rect.height, unitSpriteRenderer);
            mask.sprite = nodeSprite;
            
            backgroundSpriteRenderer.sortingOrder = 0;
            unitSpriteRenderer.sortingOrder = 1;
            
            unitSpriteRenderer.transform.localPosition = Vector3.zero;
        }

        public void DrawRight()
        {
            ScaleForNode(nodeSprite.rect.width / 2, nodeSprite.rect.height, unitSpriteRenderer);

            mask.sprite = rightPart;

            backgroundSpriteRenderer.sortingOrder = 3;
            unitSpriteRenderer.sortingOrder = 4;
            mask.frontSortingOrder = 5;
            mask.backSortingOrder = 2;
            
            unitSpriteRenderer.transform.localPosition =
                Vector3.right * unitSprite.rect.width / 2 / unitSprite.pixelsPerUnit;
        }
    }
}