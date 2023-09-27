using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MeleeUnitAttackAction : BaseUnitAttackAction
    {
        [SerializeField] private UnitBlockerSelector blockerSelector;

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [SerializeField] private bool onslaught = true;

        public UnitBlockerSelector BlockerSelector => blockerSelector;
        public bool Onslaught => onslaught;

        public override UnitAction GetAction(ComponentUnit unit) => new MeleeAttackAction(unit, this);
    }
}