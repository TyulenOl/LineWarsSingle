using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class UIStack : MonoBehaviour
    {
        public static UIStack Instance { get; private set; }
        [SerializeField] private UIStackElement initializeElement;
        [SerializeField] private List<UIStackElement> allElements;

        private Stack<UIStackElement> stackElements;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.Log($"Dublicadet {nameof(UIStack)}");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            foreach (var element in allElements)
            {
                if (element != null)
                {
                    element.gameObject.SetActive(true);
                    element.gameObject.SetActive(false);
                }
            }

            stackElements = new Stack<UIStackElement>();
            PushElement(initializeElement);
        }

        public void PushElement(UIStackElement uiStackElement)
        {
            if (uiStackElement == null) return;

            if (stackElements.Count != 0)
            {
                var previousElement = stackElements.Peek();
                previousElement.OnClose();
            }

            stackElements.Push(uiStackElement);
            uiStackElement.OnOpen();
        }

        public void PopElement()
        {
            if (stackElements.Count == 0)
                return;

            var element = stackElements.Pop();
            element.OnClose();

            if (stackElements.Count != 0)
            {
                var nextElement = stackElements.Peek();
                nextElement.OnOpen();
            }
        }
    }
}