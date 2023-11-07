using System;

namespace LineWars.Model
{
    [Flags]
    public enum ActionType
    {
        Simple = 1,
        Targeted = 2,
        MultiStage = 4,
        NeedAdditionalParameters = 8, // beta
        
        MultiTargeted = Targeted | MultiStage,
        MultiSimpleTargeted = Simple | Targeted | MultiStage
    }
}