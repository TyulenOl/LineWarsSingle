using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>,
        IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        public bool AttackLocked { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsPenetratingDamage { get; protected set; }

        protected AttackAction([NotNull] TUnit unit, MonoAttackAction data) : base(unit, data)
        {
            Damage = data.InitialDamage;
            IsPenetratingDamage = data.InitialIsPenetratingDamage;
        }

        public bool CanAttack(IAlive enemy, bool ignoreActionPointsCondition = false) =>
            CanAttackFrom(MyUnit.Node, enemy, ignoreActionPointsCondition);

        public bool CanAttackFrom(TNode node, IAlive enemy, bool ignoreActionPointsCondition = false)
        {
            return enemy switch
            {
                TEdge edge => CanAttackFrom(node, edge, ignoreActionPointsCondition),
                TUnit unit => CanAttackFrom(node, unit, ignoreActionPointsCondition),
                _ => false
            };
        }

        public void Attack(IAlive enemy)
        {
            switch (enemy)
            {
                case TEdge edge:
                    Attack(edge);
                    break;
                case TUnit unit:
                    Attack(unit);
                    break;
            }
        }


        public virtual bool CanAttackFrom(TNode node, TUnit enemy, bool ignoreActionPointsCondition = false) =>
            false;

        public virtual bool CanAttackFrom(TNode node, TEdge edge, bool ignoreActionPointsCondition = false) =>
            false;

        public virtual void Attack(TUnit unit)
        {
        }

        public virtual void Attack(TEdge edge)
        {
        }
    }
}