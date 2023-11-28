using System;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class Move : AITurn
    {
        [SerializeField] private PointerToUnit unit;
        [SerializeField] private Node toNode;
        
        public override void Execute()
        {
            unit.GetUnit().GetAction<MonoMoveAction>().MoveTo(toNode);
        }
    }
}