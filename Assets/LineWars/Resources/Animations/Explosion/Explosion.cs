using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars
{
    public class Explosion : MonoBehaviour
    {
        public event Action ExplosionEnded;
        public void OnExplosionEnd()
        {
            ExplosionEnded?.Invoke();
            Destroy(gameObject);
        }
    }
}

