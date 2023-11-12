using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction<MoveAction<Node, Edge, Unit>>,
        IMoveAction<Node, Edge, Unit>
    {
        [SerializeField] private SFXData moveSfx;
        [SerializeField] private SFXList reactionsSfx;

        private IDJ dj;

        public event Action MoveAnimationEnded;


        public bool CanMoveTo(Node target, bool ignoreActionPointsCondition = false) =>
            Action.CanMoveTo(target, ignoreActionPointsCondition);

        public override void Initialize()
        {
            base.Initialize();
            Unit.MovementLogic.MovementIsOver += MovementLogicOnMovementIsOver;
            dj = new RandomDJ(0.5f);
        }

        public void MoveTo(Node target)
        {
            Action.MoveTo(target);
            Unit.MovementLogic.MoveTo(target.transform);
            SfxManager.Instance.Play(moveSfx);
            SfxManager.Instance.Play(dj.GetSound(reactionsSfx));
            Player.LocalPlayer.RecalculateVisibility();
        }

        private void MovementLogicOnMovementIsOver(Transform obj) => MoveAnimationEnded?.Invoke();

        private void OnDestroy()
        {
            Unit.MovementLogic.MovementIsOver -= MovementLogicOnMovementIsOver;
        }

        protected override MoveAction<Node, Edge, Unit> GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit>(Unit);
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