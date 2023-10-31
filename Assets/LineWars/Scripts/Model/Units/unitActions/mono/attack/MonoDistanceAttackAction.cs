using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoDistanceAttackAction :
        MonoAttackAction<DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IDistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [field: SerializeField, Min(0)] public int InitialDistance { get; private set; }
        public uint Distance => Action.Distance;
        
        
        protected override DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer>(
                Unit,
                InitialDamage,
                InitialIsPenetratingDamage,
                (uint)InitialDistance,
                MonoGraph.Instance);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}