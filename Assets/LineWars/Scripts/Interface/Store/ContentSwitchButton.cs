using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Interface
{
    public class ContentSwitchButton : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private int timeInSeconds;
        [SerializeField] private Button button;
        [SerializeField] private RectTransform movableRectTransform;

        private void Awake()
        {
            button.onClick.AddListener(StartMove);
        }

        private void StartMove()
        {
            StopAllCoroutines();
            StartCoroutine(MoveCoroutine());
        }

        private IEnumerator MoveCoroutine()
        {
            var startPoint = movableRectTransform.localPosition;
            var targetPosition = -targetTransform.localPosition;
            float completionPercentage = 0;

            while (completionPercentage < 1)
            {
                completionPercentage += Time.deltaTime / timeInSeconds;
                var easeOutQuart = 1 - Mathf.Pow(1 - completionPercentage, 4);
                movableRectTransform.localPosition = Vector2.Lerp(startPoint,new Vector2(movableRectTransform.localPosition.x, targetPosition.y), easeOutQuart);
                yield return null;
            }
        }
    }
}
