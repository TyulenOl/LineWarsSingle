namespace LineWars.Model
{
    public abstract class UnitOnUnitBasedAnimation : UnitAnimation
    {
        protected Unit currentTarget;
        public virtual void Execute(Unit targetUnit)
        {
            currentTarget = targetUnit;
        }
    }

}
