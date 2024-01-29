using System;
using UnityEngine;

namespace LineWars.Interface
{
    public class SpriteRendererAlphaChanger : AlphaChanger
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void FixedUpdate()
        {
            if(spriteRenderer != null && spriteRenderer.gameObject.activeInHierarchy)
                ChangeAlpha();
        }

        protected override void ChangeAlpha()
        {
            var currentAlpha = spriteRenderer.color.a * 255;
            if (currentAlpha >= 255)
            {
                alphaDecreaseModifier = -Math.Abs(alphaDecreaseModifier);
            }
            else if (currentAlpha <= minAlpha)
            {
                alphaDecreaseModifier = Math.Abs(alphaDecreaseModifier);
            }

            var resultAlpha = currentAlpha + alphaDecreaseModifier;
            spriteRenderer.color = spriteRenderer.color.WithAlpha(resultAlpha / 255f);
        }
    }
}