namespace LineWars.Model
{
    public class SpawnUnitCommand: ICommand
    {
        private readonly BasePlayer player;
        private readonly Node node;
        private readonly UnitType unitType;
        
        public SpawnUnitCommand(BasePlayer player, Node node, UnitType unitType)
        {
            this.player = player;
            this.node = node;
            this.unitType = unitType;
        }
        public void Execute()
        {
            player.SpawnUnit(node, unitType);
        }

        public bool CanExecute()
        {
            return player.CanSpawnUnit(node, unitType);
        }
    }
}