using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoBlowWithSwingAction :
        MonoUnitAction<BlowWithSwingAction<Node, Edge, Unit>>,
        IBlowWithSwingAction<Node, Edge, Unit>
    {
        [SerializeField] private SFXList attackReactionSounds;
        [SerializeField] private SFXData attackSound;
        [SerializeField] private UnitAnimation swingAnimation;

        private IDJ dj;
        
        public int Damage => Action.Damage;
        public event Action<int> DamageChanged
        {
            add => Action.DamageChanged += value;
            remove => Action.DamageChanged -= value;
        }

        private void Awake()
        {
            dj = new RandomDJ(1);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            Executor.PlaySfx(attackSound);

            if (swingAnimation != null)
            {
                var context = new AnimationContext()
                {
                    TargetUnit = target
                };

                swingAnimation.Ended.AddListener(ExecuteOnAnimationEnd);
                swingAnimation.Execute(context);
            }
            else
            {
                AttackWithResponse(target);
            }

            void ExecuteOnAnimationEnd(UnitAnimation animation)
            {
                animation.Ended.RemoveListener(ExecuteOnAnimationEnd);
                AttackWithResponse(target);
            }
        }

        private void AttackWithResponse(Unit target)
        {
            foreach (var neighbor in Executor.Node.GetNeighbors())
            {
                if (neighbor.AllIsFree)
                    continue;
                if (neighbor.OwnerId == Executor.OwnerId)
                    continue;
                foreach (var unit in neighbor.Units)
                if (unit.TryGetComponent<AnimationResponses>(out var responses))
                    responses.TrySetDeathAnimation(AnimationResponseType.SwingDied);
                    
            }
            Action.Execute(target);
            foreach (var neighbor in Executor.Node.GetNeighbors())
            {
                if (neighbor.AllIsFree)
                    continue;
                if (neighbor.OwnerId == Executor.OwnerId)
                    continue;
                foreach (var unit in neighbor.Units)
                if(unit.TryGetComponent<AnimationResponses>(out var responses))
                {
                    var context = new AnimationContext()
                    {
                        TargetUnit = Executor,
                        TargetNode = Executor.Node
                    };

                    responses.Respond(AnimationResponseType.SwingDamaged, context);
                    responses.SetDefaultDeathAnimation();
                }
            }
            Executor.PlaySfx(dj.GetSound(attackReactionSounds));
        }

        protected override BlowWithSwingAction<Node, Edge, Unit> GetAction()
        {
            return new BlowWithSwingAction<Node, Edge, Unit>(Executor);
        }

        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}