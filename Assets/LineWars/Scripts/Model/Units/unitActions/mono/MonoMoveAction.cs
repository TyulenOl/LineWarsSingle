using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    [DisallowMultipleComponent]
    public class MonoMoveAction : MonoUnitAction<MoveAction<Node, Edge, Unit>>,
        IMoveAction<Node, Edge, Unit>
    {
        [SerializeField] private SFXData moveSfx;
        [SerializeField] private SFXList reactionsSfx;

        private IDJ dj;

        public event Action MoveAnimationEnded;

        protected override bool NeedAutoComplete => false;

        public bool CanMoveTo(Node target) =>
            Action.CanMoveTo(target);

        public override void Initialize()
        {
            base.Initialize();
            Executor.MovementLogic.MovementIsOver += MovementLogicOnMovementIsOver;
            dj = new RandomDJ(0.5f);
        }

        public void MoveTo(Node target)
        {
            Action.MoveTo(target);
            Executor.MovementLogic.MoveTo(target.transform.position);
            Executor.MovementLogic.MovementIsOver += OnMoveEnd;
            Executor.PlaySfx(moveSfx);
            Executor.PlaySfx(dj.GetSound(reactionsSfx));
            Player.LocalPlayer.RecalculateVisibility();
        }

        private void OnMoveEnd()
        {
            Executor.MovementLogic.MovementIsOver -= OnMoveEnd;
            Complete();
        }

        private void MovementLogicOnMovementIsOver() => MoveAnimationEnded?.Invoke();

        private void OnDestroy()
        {
            Executor.MovementLogic.MovementIsOver -= MovementLogicOnMovementIsOver;
        }

        protected override MoveAction<Node, Edge, Unit> GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit>(Executor);
            return action;
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