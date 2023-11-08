namespace LineWars.Model
{
    public class SpawnPresetCommandBlueprint : ICommandBlueprint
    {
        public int ExecutorId
        {
            get => -1;
        }

        public int PlayerId { get; private set; }
        public UnitBuyPreset UnitBuyPreset { get; private set; }

        public SpawnPresetCommandBlueprint(int playerId, UnitBuyPreset unitBuyPreset)
        {
            PlayerId = playerId;
            UnitBuyPreset = unitBuyPreset;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var player = projection.PlayersIndexList[PlayerId];

            return new SpawnPresetCommand<NodeProjection, EdgeProjection, UnitProjection, BasePlayerProjection>(player,
                UnitBuyPreset);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var player = projection.PlayersIndexList[PlayerId].Original;

            return new SpawnPresetCommand<Node, Edge, Unit, BasePlayer>(player, UnitBuyPreset);
        }
    }
}