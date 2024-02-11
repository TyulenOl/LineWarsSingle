using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class TargetedNodePlayerAction: PlayerAction
    {
        [Header("")]
        [SerializeField] private CommandType commandType;
        [SerializeField] private PointerToUnit unit;
        [SerializeField] private Node targetedNode;
        
        public override bool IsMyCommandType(CommandType commandType)
        {
            return commandType == this.commandType;
        }

        public override bool CanSelectExecutor(IMonoExecutor executor)
        {
            return executor.Equals(unit.GetUnit());
        }

        public override bool CanSelectTarget(int targetId, IMonoTarget target)
        {
            return targetId == 0 && target.Equals(targetedNode);
        }
    }
}