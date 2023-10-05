
namespace LineWars.Model
{
    public class SpawnPresetCommand<TNode, TEdge, TUnit, TOwned, TPlayer>:
        ICommand
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
    {
        private readonly TPlayer player;
        private readonly UnitBuyPreset unitPreset;
        
        public SpawnPresetCommand(TPlayer player, UnitBuyPreset unitPreset)
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