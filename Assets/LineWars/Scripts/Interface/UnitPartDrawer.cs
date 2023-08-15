using System;
using System.Collections;
using System.Collections.Generic;
using LineWars.Extensions;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class UnitPartDrawer : MonoBehaviour
    {
        [HideInInspector] public Vector2 offset;

        [Header("Reference")]
        [SerializeField] private TMP_Text damageText;

        private void Awake()
        {
            damageText.gameObject.SetActive(false);
        }

        public void AnimateDamageText(string text, Color textColor)
        {
            damageText.text = text;
            damageText.color = textColor;
            StartCoroutine(AnimateDamageTextCoroutine());
            Debug.Log(offset);
        }
        
        private IEnumerator AnimateDamageTextCoroutine()
        {
            float progress = 0;

            var text = Instantiate(damageText, transform, true);
            text.gameObject.SetActive(true);
            Vector2 startPos = text.transform.position;

            while (progress < 1)
            {
                progress += Time.deltaTime;
                text.gameObject.transform.position = startPos + offset * progress;
                text.color = text.color.WithAlpha(Mathf.Pow(1 - progress, 0.5f));
                yield return null;
            }
            
            Destroy(text.gameObject);
        }
    }
}