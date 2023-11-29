using LineWars.Controllers;
using LineWars.Model;
using UnityEngine;

namespace LineWars.Education
{
    public abstract class PlayerAction :
        MonoBehaviour,
        ICommandTypeConstrain,
        IExecutorConstrain,
        ITargetConstrain,
        ISimpleCommandConstrain,
        ICurrentCommandConstrain,
        IBuyConstrain
    {
       

        public virtual bool IsMyCommandType(CommandType commandType) => false;
        
        public virtual bool CanCancelExecutor => false;
        public virtual bool CanSelectExecutor(IMonoExecutor executor) => false;

        public virtual bool CanSelectTarget(int targetId, IMonoTarget target) => false;

        public virtual bool CanExecuteSimpleAction() => false;
        public virtual bool CanSelectCurrentCommand() => false;

        public virtual bool CanSelectNode(Node node) => false;
        public virtual bool CanSelectUnitBuyPreset(UnitBuyPreset preset) => false;
    }
}