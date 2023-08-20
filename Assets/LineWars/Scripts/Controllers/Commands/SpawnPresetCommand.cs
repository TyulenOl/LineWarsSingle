namespace LineWars.Model
{
    public class SpawnPresetCommand: ICommand
    {
        private readonly BasePlayer player;
        private readonly Node node;
        private readonly UnitBuyPreset unitPreset;
        
        public SpawnPresetCommand(BasePlayer player, Node node, UnitBuyPreset unitPreset)
        {
            this.player = player;
            this.node = node;
            this.unitPreset = unitPreset;
        }
        public void Execute()
        {
            player.SpawnPreset(node, unitPreset);
        }

        public bool CanExecute()
        {
            return player.CanSpawnUnit(node, unitPreset);
        }

        public string GetLog()
        {
            return $"Игрок {player.gameObject.name} заспавнил в ноде {node.gameObject.name} пресет юнитов {unitPreset.Name}";
        }
    }
}