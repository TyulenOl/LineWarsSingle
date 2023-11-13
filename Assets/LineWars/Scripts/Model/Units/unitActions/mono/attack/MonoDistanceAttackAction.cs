using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction :
        MonoAttackAction<DistanceAttackAction<Node, Edge, Unit>>,
        IDistanceAttackAction<Node, Edge, Unit>
    {
        [field: SerializeField, Min(0)] public int InitialDistance { get; private set; }
        public uint Distance => Action.Distance;
        
        
        protected override DistanceAttackAction<Node, Edge, Unit> GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit>(
                Unit,
                InitialDamage,
                InitialIsPenetratingDamage,
                (uint)InitialDistance,
                MonoGraph.Instance);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}