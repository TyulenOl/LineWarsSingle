using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction : MonoAttackAction,
        IDistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer> DistanceAttack
            => (DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        
        [field: SerializeField, Min(0)] public int InitialDistance { get; private set; }
        public uint Distance => DistanceAttack.Distance;
        
        
        protected override ExecutorAction GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>(GetComponent<Unit>(), this, MonoGraph.Instance);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}