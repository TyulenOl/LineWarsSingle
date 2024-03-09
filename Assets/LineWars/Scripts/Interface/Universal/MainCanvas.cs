using System;
using System.Collections;
using System.Collections.Generic;
using DataStructures;
using UnityEngine;

namespace LineWars
{
    public class MainCanvas : Singleton<MainCanvas>
    {
        private Canvas canvas;

        public Canvas Canvas => canvas;

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
        }
    }
}
