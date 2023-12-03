using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class MovePlayerAction : ActionWithExecutor
    {
        [Header("")]
        [SerializeField] private PointerToUnit pointerToUnit;
        [SerializeField] private Node node;

        public override bool IsMyCommandType(CommandType commandType)
        {
            return commandType == CommandType.Move;
        }

        public override bool CanSelectExecutor(IMonoExecutor executor)
        {
            return executor.Equals(pointerToUnit.GetUnit());
        }

        public override bool CanSelectTarget(int targetId, IMonoTarget target)
        {
            return targetId == 0 && target.Equals(node);
        }
    }
}