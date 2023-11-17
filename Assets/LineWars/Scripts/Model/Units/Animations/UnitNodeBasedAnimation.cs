namespace LineWars.Model
{
    public abstract class UnitNodeBasedAnimation : UnitAnimation
    {
        public abstract void Execute(Unit unit, Node node);      
    }
}
