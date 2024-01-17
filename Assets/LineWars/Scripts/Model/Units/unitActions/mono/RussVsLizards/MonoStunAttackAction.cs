using UnityEngine;

namespace LineWars.Model
{
    public class MonoStunAttackAction : 
        MonoUnitAction<StunAttackAction<Node, Edge, Unit>>,
        ITargetedAction<Unit>,
        IStunAttackAction<Node, Edge, Unit>
    {
        [SerializeField] private ActionUnitAnimation attackAnimation;
        protected override bool NeedAutoComplete => false;
        protected override StunAttackAction<Node, Edge, Unit> GetAction()
        {
            return new StunAttackAction<Node, Edge, Unit>(Executor);
        }

        public bool IsAvailable(Unit target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Unit target)
        {
            if(attackAnimation != null)
                AnimationAttack(target);
            else
                AttackInstant(target);
        }

        private void AttackInstant(Unit target)
        {
            Action.Execute(target);
            Complete();
        }

        private void AnimationAttack(Unit target)
        {
            var context = new AnimationContext()
            {
                TargetUnit = target
            };
            attackAnimation.SetAction(() => Action.Execute(target));
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            attackAnimation.Execute(context);
            
            void OnAnimationEnd(UnitAnimation animation)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Complete();
            }
        }

        public IActionCommand GenerateCommand(Unit target)
        {
            return new TargetedUniversalCommand<Unit, MonoStunAttackAction, Unit>(this, target);
        }
        public override void Accept(IMonoUnitActionVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
