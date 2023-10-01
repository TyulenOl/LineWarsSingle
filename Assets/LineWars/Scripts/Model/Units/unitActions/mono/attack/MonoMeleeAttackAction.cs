using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMeleeAttackAction : MonoAttackAction,
        IMeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        [SerializeField] private UnitBlockerSelector initialBlockerSelector;

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [SerializeField] private bool initialOnslaught = true;

        public UnitBlockerSelector InitialBlockerSelector => initialBlockerSelector;
        public bool InitialOnslaught => initialOnslaught;

        protected override ExecutorAction GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this);
        }
    }
}