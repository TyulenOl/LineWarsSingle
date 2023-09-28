using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction: MonoAttackAction
    {
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        protected override UnitAction GetAction(ComponentUnit unit) => new DistanceAttackAction(unit, this);
    }
}