using System.Numerics;

namespace LineWars.Model
{
    public class SpawnPresetCommand: ICommand
    {
        private readonly IBasePlayer player;
        private readonly UnitBuyPreset unitPreset;
        
        public SpawnPresetCommand(IBasePlayer player, UnitBuyPreset unitPreset)
        {
            this.player = player;
            this.unitPreset = unitPreset; 
        }

        public void Execute()
        {
            player.SpawnPreset(unitPreset);
        }

        public bool CanExecute()
        {
            return player.CanSpawnPreset(unitPreset);
        }

        public string GetLog()
        {
            return $"Игрок {player} заспавнил на базе пресет юнитов {unitPreset.Name}";
        }
    }
}