using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class UIStackElement : MonoBehaviour
    {
        private bool isOpen;

        protected virtual void Awake()
        {
            if (!isOpen)
                gameObject.SetActive(false);
        }

        public virtual void OnOpen()
        {
            isOpen = true;
            gameObject.SetActive(true);
        }

        public virtual void OnClose()
        {
            isOpen = false;
            gameObject.SetActive(false);
        }
    }
}