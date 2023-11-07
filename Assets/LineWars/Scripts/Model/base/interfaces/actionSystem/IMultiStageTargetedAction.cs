using System;

namespace LineWars.Model
{
    public interface IMultiStageTargetedAction: ITargetedAction
    {
        public Type[] AdditionalTargets { get; }
    }
}