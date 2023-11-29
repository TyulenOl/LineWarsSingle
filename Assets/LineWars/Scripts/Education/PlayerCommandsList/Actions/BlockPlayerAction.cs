using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class BlockPlayerAction: ActionWithExecutor
    {
        [Header("")]
        [SerializeField] private PointerToUnit pointerToUnit;
        
        public override bool IsMyCommandType(CommandType commandType)
        {
            return commandType == CommandType.Block;
        }

        public override bool CanSelectExecutor(IMonoExecutor executor)
        {
            return executor.Equals(pointerToUnit.GetUnit());
        }
        
        public override bool CanExecuteSimpleAction()
        {
            return true;
        }
    }
}