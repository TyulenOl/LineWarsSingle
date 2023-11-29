using UnityEngine;

namespace LineWars.Model
{
    public class BlockAIAction: AIAction
    {
        [SerializeField] private PointerToUnit unit;
        public override void Execute()
        {
            unit.GetUnit().GetAction<MonoRLBlockAction>().EnableBlock();
        }
    }
}