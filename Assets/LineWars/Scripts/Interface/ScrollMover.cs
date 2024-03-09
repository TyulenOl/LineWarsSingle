using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Scripts.Interface
{
    public class ScrollMover : MonoBehaviour
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private int timeInSeconds;

        public void MoveRight()
        {
            StopAllCoroutines();
            StartCoroutine(MoveCoroutine(1, false));
        }

        private IEnumerator MoveCoroutine(int targetValue, bool isVertical)
        {
            var currentScrollbar = isVertical ? scrollRect.verticalScrollbar : scrollRect.horizontalScrollbar;
            var currentValue = currentScrollbar.value;
            var difference = targetValue - currentValue;
            
            float completionPercentage = 0;

            while (completionPercentage < 1)
            {
                completionPercentage += Time.deltaTime / timeInSeconds;
                currentScrollbar.value = currentValue + difference * completionPercentage;
                yield return null;
            }
        }
    }
}