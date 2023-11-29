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
        [SerializeField] private DistanceAttackAnimation distanceAttackAnimation;
        protected override bool NeedAutoComplete => false;
        
        protected override DistanceAttackAction<Node, Edge, Unit> GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit>(
                Executor,
                InitialDamage,
                InitialIsPenetratingDamage,
                (uint)InitialDistance,
                MonoGraph.Instance);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Attack(ITargetedAlive enemy)
        {
            if(enemy is Unit targetUnit && distanceAttackAnimation != null)
            {
                var context = new AnimationContext()
                {
                    TargetNode = targetUnit.Node,
                    TargetUnit = targetUnit
                };

                distanceAttackAnimation.Ended.AddListener(AttackOnEvent);
                distanceAttackAnimation.Execute(context);
            }
            else
            {
                ExecuteAttack(enemy);
            }

            void AttackOnEvent(UnitAnimation _)
            {
                distanceAttackAnimation.Ended.RemoveListener(AttackOnEvent);
                ExecuteAttack(enemy);
            }
        }

        private void ExecuteAttack(ITargetedAlive enemy)
        {
            base.Attack(enemy);
            if (enemy == null || enemy.CurrentHp <= 0)
            {
                Complete();
                return;
            }
            if (enemy is Unit unit && unit.TryGetComponent(out AnimationResponses responses))
            {
                var animContext = new AnimationContext()
                {
                    TargetNode = Executor.Node,
                    TargetUnit = Executor
                };
                var respond = responses.Respond(AnimationResponseType.DistanceDamaged, animContext);
                if (respond != null)
                    respond.Ended.AddListener(CompleteOnRespondEnd);
                else
                    Complete();
            }
            else
            {
                Complete();
            }

            void CompleteOnRespondEnd(UnitAnimation respond)
            {
                respond.Ended.RemoveListener(CompleteOnRespondEnd);
                Complete();
            }
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}