using System;
using UnityEngine;

namespace LineWars.Controllers
{
    public abstract class ActionInvokerBase: MonoBehaviour
    {
        public abstract void Invoke(Action action);
    }
}