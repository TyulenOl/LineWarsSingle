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
        [SerializeField] private bool isY;

        private void Awake()
        {
            button.onClick.AddListener(StartMove);
        }

        private void StartMove()
        {
            StopAllCoroutines();
            StartCoroutine(!isY ? MoveCoroutineUpDown() : MoveCoroutineLeftRight());
        }

        private IEnumerator MoveCoroutineUpDown()
        {
            var startPoint = movableRectTransform.localPosition;
            var targetPosition = -targetTransform.localPosition;
            float completionPercentage = 0;

            while (completionPercentage < 1)
            {
                completionPercentage += Time.deltaTime / timeInSeconds;
                var easeOutQuart = 1 - Mathf.Pow(1 - completionPercentage, 4);
                movableRectTransform.localPosition = Vector2.Lerp(startPoint,new Vector2(movableRectTransform.localPosition.y, targetPosition.x), easeOutQuart);
                yield return null;
            }
        }
        
        private IEnumerator MoveCoroutineLeftRight()
        {
            var startPoint = movableRectTransform.localPosition;
            var targetPosition = targetTransform.localPosition;
            float completionPercentage = 0;
            var targetPoint = new Vector2(startPoint.x-targetPosition.x, startPoint.y);

            while (completionPercentage < 1)
            {
                completionPercentage += Time.deltaTime / timeInSeconds;
                var easeOutQuart = 1 - Mathf.Pow(1 - completionPercentage, 4);
                movableRectTransform.localPosition = Vector2.Lerp(startPoint,targetPoint, easeOutQuart);
                yield return null;
            }
        }
    }
}
