using System.Numerics;

namespace LineWars.Model
{
    public class SpawnPresetCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        ICommand
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
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