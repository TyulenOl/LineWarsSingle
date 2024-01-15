using System;
using UnityEngine;

namespace LineWars
{
    public class SimpleEffect : MonoBehaviour
    {
        public event Action Ended;
        public void OnEnd()
        {
            Ended?.Invoke();
            Destroy(gameObject);
        }
    }
}

