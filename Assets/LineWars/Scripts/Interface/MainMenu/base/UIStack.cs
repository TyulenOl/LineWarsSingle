using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace LineWars
{
    public class UIStack : Singleton<UIStack>
    {
        [SerializeField] private Transform initializeElement;

        private Stack<Transform> stackElements;

#if UNITY_EDITOR
        [SerializeField, ReadOnlyInspector] private List<Transform> stackElementsList;
#endif
        
        private void Start()
        {
            stackElements = new Stack<Transform>();
            PushElement(initializeElement);
        }

        public void PushElement(Transform uiStackElement)
        {
            if (uiStackElement == null) return;

            if (stackElements.Count != 0)
            {
                var previousElement = stackElements.Peek();
                previousElement.gameObject.SetActive(false);
            }
            
            stackElements.Push(uiStackElement);
#if UNITY_EDITOR
            stackElementsList.Add(uiStackElement);
#endif
            uiStackElement.gameObject.SetActive(true);
        }

        public void PopElement()
        {
            if (stackElements.Count == 0)
                return;

            var element = stackElements.Pop();
#if UNITY_EDITOR
            stackElementsList.Remove(element);
#endif
            element.gameObject.SetActive(false);

            if (stackElements.Count != 0)
            {
                var nextElement = stackElements.Peek();
                nextElement.gameObject.SetActive(true);
            }
        }
    }
}