using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class UIStack : MonoBehaviour
    {
        [SerializeField] private UIStackElement initializeElement;
        
        private Stack<UIStackElement> stackElements;

        private void Awake()
        {
            stackElements = new Stack<UIStackElement>();
        }

        private void Start()
        {
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