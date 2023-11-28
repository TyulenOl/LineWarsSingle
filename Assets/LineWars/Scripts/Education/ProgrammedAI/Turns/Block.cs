using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class Block: AITurn
    {
        [SerializeField] private PointerToUnit unit;
        public override void Execute()
        {
            unit.GetUnit().GetAction<MonoRLBlockAction>().EnableBlock();
        }
    }
}