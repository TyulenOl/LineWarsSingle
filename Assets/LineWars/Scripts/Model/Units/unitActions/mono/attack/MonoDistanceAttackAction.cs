using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction : MonoAttackAction,
        IDistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        protected DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation> DistanceAttack
            => (DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        
        [field: SerializeField, Min(0)] public int InitialDistance { get; private set; }
        public uint Distance => DistanceAttack.Distance;
        
        
        protected override ExecutorAction GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this, MonoGraph.Instance);
        }
    }
}