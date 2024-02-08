using System;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoArsonAction :
        MonoUnitAction<ArsonAction<Node, Edge, Unit>>,
        IArsonAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;
        [SerializeField] private int fireEffectRounds;

        protected override bool NeedAutoComplete => false;
        public int FireEffectRounds => fireEffectRounds;
        protected override ArsonAction<Node, Edge, Unit> GetAction()
        {
            return new ArsonAction<Node, Edge, Unit>(Executor, fireEffectRounds);
        }

        public bool IsAvailable(Node target)
        {
            return Action.IsAvailable(target);
        }

        public void Execute(Node target)
        {
            if(unitAnimation == null)
            {
                ExecuteInstant(target);
            }
            else
            {
                ExecuteWithAnimation(target);
            }
        }
        
        private void ExecuteWithAnimation(Node target)
        {
            unitAnimation.Ended.AddListener(OnAnimEnd);
            var context = new AnimationContext()
            {
                TargetNode = target,
                TargetPosition = target.Position
            };
            unitAnimation.Execute(context);
            void OnAnimEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        private void ExecuteInstant(Node target)
        {
            Action.Execute(target);
            Complete();
        }

        public IActionCommand GenerateCommand(Node target)
        {
            return new TargetedUniversalCommand<Unit, MonoArsonAction, Node>(Executor, target);
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
