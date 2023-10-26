using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public abstract class AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
        UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
        IAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public bool AttackLocked { get; protected set; }
        public int Damage { get; protected set; }
        public bool IsPenetratingDamage { get; protected set; }
        
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

        public Type TargetType => typeof(IAlive);
        public bool IsMyTarget(ITarget target) => target is IAlive;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new AttackCommand<TNode, TEdge, TUnit, TOwned, TPlayer>(this, (IAlive) target);
        }

        protected AttackAction(TUnit executor) : base(executor)
        {
        }
    }
}