using UnityEngine;

namespace LineWars.Model
{
    public class MoveProgrammedAIAction : ProgrammedAIAction
    {
        [SerializeField] private PointerToUnit unit;
        [SerializeField] private Node toNode;
        
        public override void Execute()
        {
            unit.GetUnit().GetAction<MonoMoveAction>().MoveTo(toNode);
        }
    }
}