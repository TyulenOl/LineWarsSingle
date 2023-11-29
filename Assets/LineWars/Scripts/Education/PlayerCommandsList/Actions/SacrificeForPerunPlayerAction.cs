using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public class SacrificeForPerunPlayerAction: PlayerAction
    {
        [Header("")]
        [SerializeField] private PointerToUnit unit;
        [SerializeField] private PointerToUnit enemy;
        
        public override bool IsMyCommandType(CommandType commandType)
        {
            return commandType == CommandType.SacrificePerun;
        }

        public override bool CanSelectExecutor(IMonoExecutor executor)
        {
            return executor.Equals(unit.GetUnit());
        }

        public override bool CanSelectTarget(int targetId, IMonoTarget target)
        {
            return targetId == 0 && target.Equals(enemy.GetUnit());
        }

        public override bool CanSelectCurrentCommand()
        {
            return true;
        }
    }
}