namespace LineWars.Model
{
    public interface IMonoUnitVisitor
    {
        public void Visit(MonoBuildRoadAction action);
        public void Visit(MonoBlockAction action);
        public void Visit(MonoMoveAction action);
        public void Visit(MonoHealAction action);
        public void Visit(MonoDistanceAttackAction action);
        public void Visit(MonoArtilleryAttackAction action);
        public void Visit(MonoMeleeAttackAction action);
    }
}
