using System;

namespace LineWars.Model
{
    public interface IManyTargetsAction
    {
        public Type[] MyTargets { get; }
    }
}