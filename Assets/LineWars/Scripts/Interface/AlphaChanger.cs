using System;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class AlphaChanger : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] protected float alphaDecreaseModifier;
        [SerializeField] protected float minAlpha;

        private void FixedUpdate()
        {
            if(image != null && image.gameObject.activeInHierarchy)
                ChangeAlpha();
        }

        protected virtual void ChangeAlpha()
        {
            var currentAlpha = image.color.a * 255;
            if (currentAlpha >= 255)
            {
                alphaDecreaseModifier = -Math.Abs(alphaDecreaseModifier);
            }
            else if (currentAlpha <= minAlpha)
            {
                alphaDecreaseModifier = Math.Abs(alphaDecreaseModifier);
            }

            var resultAlpha = currentAlpha + alphaDecreaseModifier;
            image.color = new Color(image.color.r, image.color.g, image.color.b,
                resultAlpha / 255f);
        }
    }
}