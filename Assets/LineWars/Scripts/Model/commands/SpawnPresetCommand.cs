namespace LineWars.Model
{
    public class SpawnPresetCommand<TNode, TEdge, TUnit, TPlayer> :
        ICommand
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        where TPlayer : class, IBasePlayer
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
            player.BuyPreset(unitPreset);
        }

        public bool CanExecute()
        {
            return player.CanBuyPreset(unitPreset);
        }

        public string GetLog()
        {
            return $"Игрок {player} заспавнил на базе пресет юнитов {unitPreset.Name}";
        }
    }
}