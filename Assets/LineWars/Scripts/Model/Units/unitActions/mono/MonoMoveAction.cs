using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction<MoveAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IMoveAction<Node, Edge, Unit, Owned, BasePlayer>
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