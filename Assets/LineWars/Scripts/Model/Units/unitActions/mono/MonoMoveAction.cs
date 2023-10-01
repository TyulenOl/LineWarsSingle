using System;
using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoMoveAction : MonoUnitAction, IMoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        private MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation> MoveAction =>
            (MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>) ExecutorAction;
        
        public bool CanMoveTo(Node target, bool ignoreActionPointsCondition = false) =>
            MoveAction.CanMoveTo(target, ignoreActionPointsCondition);

        public void MoveTo(Node target) => MoveAction.MoveTo(target);
        protected override ExecutorAction GetAction()
        {
            var action = new MoveAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this);
            return action;
        }
    }
}