using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction<MoveAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IMoveAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] private SFXData moveSfx;
        public event Action MoveAnimationEnded;

        private MoveAction<Node, Edge, Unit, Owned, BasePlayer> MoveAction =>
            (MoveAction<Node, Edge, Unit, Owned, BasePlayer>) Action;

        public bool CanMoveTo(Node target, bool ignoreActionPointsCondition = false) =>
            MoveAction.CanMoveTo(target, ignoreActionPointsCondition);

        public override void Initialize()
        {
            base.Initialize();
            Unit.MovementLogic.MovementIsOver += MovementLogicOnMovementIsOver;
        }

        public void MoveTo(Node target)
        {
            MoveAction.MoveTo(target);
            Unit.MovementLogic.MoveTo(target.transform);
            SfxManager.Instance.Play(moveSfx);
        }

        private void MovementLogicOnMovementIsOver(Transform obj) => MoveAnimationEnded?.Invoke();

        public Type TargetType => typeof(Node);
        public bool IsMyTarget(ITarget target) => target is Node;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new MoveCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Node) target);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void OnDestroy()
        {
            Unit.MovementLogic.MovementIsOver -= MovementLogicOnMovementIsOver;
        }

        protected override MoveAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
            return action;
        }
    }
}