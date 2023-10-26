using System;

namespace LineWars.Model
{
    public interface IMultiStageTargetAction: ITargetedAction
    {
        public Type[] MyTargets { get; }
    }
}