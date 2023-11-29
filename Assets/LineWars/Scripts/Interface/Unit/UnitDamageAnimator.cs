using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace LineWars.Interface
{
    public class UnitDamageAnimator : MonoBehaviour
    {
        [HideInInspector] public Vector2 offset;
        [SerializeField] private float animationThreshold = 0.5f;

        [Header("Reference")] [SerializeField] private TMP_Text damageText;

        private int currentCoroutineIndex = -1;
        private float globalProgress = 1;

        private bool CanStartAnimation => globalProgress > animationThreshold;

        public float AnimationThreshold
        {
            get => animationThreshold;
            set => animationThreshold = Math.Min(Math.Max(0, value), 1);
        }

        private void Awake()
        {
            damageText.gameObject.SetActive(false);
        }

        public void AnimateDamageText(string text, Color textColor)
        {
            StartCoroutine(WaitAnimateDamageCoroutine(text, textColor));
        }

        private IEnumerator WaitAnimateDamageCoroutine(string text, Color textColor)
        {
            while (!CanStartAnimation)
            {
                yield return null;
            }

            StartCoroutine(AnimateDamageTextCoroutine(text, textColor));
        }

        private IEnumerator AnimateDamageTextCoroutine(string text, Color textColor)
        {
            var myIndex = ++currentCoroutineIndex;
            float localProgress = globalProgress = 0;

            var instantiate = Instantiate(damageText, transform, true);
            instantiate.text = text;
            instantiate.color = textColor;
            instantiate.gameObject.SetActive(true);
            Vector2 startPos = instantiate.transform.position;

            while (localProgress < 1)
            {
                localProgress += Time.deltaTime;
                instantiate.gameObject.transform.position = startPos + offset * localProgress;
                instantiate.color = instantiate.color.WithAlpha(Mathf.Pow(1 - localProgress, 0.5f));

                if (currentCoroutineIndex == myIndex)
                    globalProgress = localProgress;

                yield return null;
            }

            Destroy(instantiate.gameObject);
        }
    }
}