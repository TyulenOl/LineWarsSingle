using UnityEngine;

namespace LineWars.Model
{
    public class BlockProgrammedAIAction: ProgrammedAIAction
    {
        [SerializeField] private PointerToUnit unit;
        public override void Execute()
        {
            unit.GetUnit().GetAction<MonoRLBlockAction>().EnableBlock();
        }
    }
}