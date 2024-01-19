namespace LineWars.Model
{
    public class TargetUnitUniversalCommandBlueprint<TMonoAction, TAction, TProjectionAction>
        : ICommandBlueprint
        where TProjectionAction : UnitAction<NodeProjection, EdgeProjection, UnitProjection>, 
        ITargetedAction<UnitProjection>
        where TAction : UnitAction<Node, Edge, Unit>, ITargetedAction<Unit>
        where TMonoAction : MonoUnitAction<TAction>, ITargetedAction<Unit>
    {
        private int executorId;
        private int targetId;
        public int ExecutorId => executorId;

        public TargetUnitUniversalCommandBlueprint(int executorId, int targetId)
        {
            this.executorId = executorId;
            this.targetId = targetId;
        }

        public ICommand GenerateCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[executorId];
            var target = projection.UnitsIndexList[targetId];
            return new TargetedUniversalCommand<
                UnitProjection,
                TProjectionAction,
                UnitProjection>(executor, target);
        }

        public ICommand GenerateMonoCommand(GameProjection projection)
        {
            var executor = projection.UnitsIndexList[executorId].Original;
            var target = projection.UnitsIndexList[targetId].Original;
            return new TargetedUniversalCommand<
                Unit,
                TMonoAction,
                Unit>(executor, target);
        }
    }
}
