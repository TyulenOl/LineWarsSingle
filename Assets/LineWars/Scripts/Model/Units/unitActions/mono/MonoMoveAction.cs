using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction,
        IMoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        [SerializeField] private SFXData moveSfx;
        private MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation> MoveAction =>
            (MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        
        public bool CanMoveTo(Node target, bool ignoreActionPointsCondition = false) => 
            MoveAction.CanMoveTo(target, ignoreActionPointsCondition);

        public void MoveTo(Node target)
        {
            MoveAction.MoveTo(target);
            Unit.MovementLogic.MoveTo(target.transform);
            SfxManager.Instance.Play(moveSfx);
        }
        
        public bool IsMyTarget(ITarget target) => MoveAction.IsMyTarget(target);

        public ICommand GenerateCommand(ITarget target)
        {
            return new MoveCommand<Node, Edge, Unit, Owned, BasePlayer, Nation>(this, (Node) target);
        }
        
        protected override ExecutorAction GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(Unit, this);
            return action;
        }
    }
}