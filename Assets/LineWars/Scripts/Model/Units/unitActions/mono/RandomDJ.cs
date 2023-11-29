using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class RandomDJ : IDJ
    {
        private float proximity;

        public RandomDJ(float proximity)
        {
            this.proximity = Mathf.Clamp01(proximity);
        }
        
        public SFXData GetSound(SFXList list)
        {
            var sounds = list.ToArray();
            if (UnityEngine.Random.Range(0f, 1f) > proximity)
                return null;
            var counter = UnityEngine.Random.Range(0, sounds.Length);
            return sounds[counter];
        }
    }
}