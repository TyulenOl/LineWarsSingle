using System;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoFogEraseAction :
        MonoUnitAction<FogEraseAction<Node, Edge, Unit>>,
        IFogEraseAction<Node, Edge, Unit>
    {
        [SerializeField] private UnitAnimation unitAnimation;

        protected override bool NeedAutoComplete => false;
        protected override FogEraseAction<Node, Edge, Unit> GetAction()
        {
            return new FogEraseAction<Node, Edge, Unit>(Executor);
        }

        public void Execute(Node target)
        {
            if(unitAnimation == null)
            {
                ExecuteInstant(target);
            }
            else
            {
                unitAnimation.Ended.AddListener(OnAnimEnd);
                var context = new AnimationContext()
                {
                    TargetNode = target
                };
                unitAnimation.Execute(context);
            }

            void OnAnimEnd(UnitAnimation _)
            {
                unitAnimation.Ended.RemoveListener(OnAnimEnd);
                ExecuteInstant(target);
            }
        }

        private void ExecuteInstant(Node target)
        {
            Action.Execute(target);
            Player.LocalPlayer.SetAdditionalVisibleNodeForRound(target, 1);
            Player.LocalPlayer.RecalculateVisibility();
            Complete();
        }

        public bool IsAvailable(Node target)
        {
            return Action.IsAvailable(target);
        }

        public IActionCommand GenerateCommand(Node target)
        {
            return new TargetedUniversalCommand<
                Unit,
                MonoFogEraseAction,
                Node>(this, target);
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
