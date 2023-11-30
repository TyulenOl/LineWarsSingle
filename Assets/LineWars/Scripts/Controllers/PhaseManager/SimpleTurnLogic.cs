
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class SimpleTurnLogic : ITurnLogic
    {
        private Action action;

        public SimpleTurnLogic(Action action)
        {
            this.action = action;
        }

        public event Action<ITurnLogic> Ended;

        public void Start()
        {
            action();
            Ended?.Invoke(this);
        }
    }
}
