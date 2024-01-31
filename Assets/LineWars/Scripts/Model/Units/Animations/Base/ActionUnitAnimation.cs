using System;

namespace LineWars.Model
{
    public abstract class ActionUnitAnimation : UnitAnimation
    {
        protected Action givenMethod;

        public void SetAction(Action action)
        {
            givenMethod = action;
        }
    }
}
