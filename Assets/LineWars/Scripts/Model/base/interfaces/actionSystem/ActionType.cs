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
    }

    public static class ActionTypeHelper
    {
        public static bool IsMultiStage(this ActionType me)
        {
            return me.Is(ActionType.MultiStage);
        }

        public static bool Is(this ActionType me, ActionType other)
        {
            return (me & other) == other;
        }
    }
}