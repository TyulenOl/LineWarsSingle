﻿using System;
using UnityEngine;

namespace LineWars.Model
{
    public class LonelinessEffectInitializer : EffectInitializer
    {
        [SerializeField] private int powerBonus;
        public override Effect<Node, Edge, Unit> GetEffect(Unit unit)
        {
            return new LonelinessEffect<Node, Edge, Unit>(unit, powerBonus);
        }
    }
}
