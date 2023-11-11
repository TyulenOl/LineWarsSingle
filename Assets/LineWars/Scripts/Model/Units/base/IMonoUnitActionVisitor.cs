namespace LineWars.Model
{
    public interface IMonoUnitActionVisitor
    {
        public void Visit(MonoBuildRoadAction action);
        public void Visit(MonoBlockAction action);
        public void Visit(MonoMoveAction action);
        public void Visit(MonoHealAction action);
        public void Visit(MonoDistanceAttackAction action);
        public void Visit(MonoArtilleryAttackAction action);
        public void Visit(MonoMeleeAttackAction action);

        public void Visit(MonoRLBlockAction action);
        public void Visit(MonoSacrificeForPerunAction action);
        public void Visit(MonoRamAction action);
        public void Visit(MonoBlowWithSwingAction action);
        public void Visit(MonoShotUnitAction action);
        public void Visit(MonoRLBuildAction action);
    }
}