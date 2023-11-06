using System;
using JetBrains.Annotations;


namespace LineWars.Model
{
    public class BuildCommand<TNode, TEdge, TUnit> :
        ICommandWithCommandType
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        private readonly IBuildAction<TNode, TEdge, TUnit> buildAction;
        private readonly TUnit engineer;
        private readonly TEdge edge;

        public BuildCommand([NotNull] TUnit engineer, [NotNull] TEdge edge)
        {
            this.engineer = engineer ?? throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));

            buildAction = engineer.TryGetUnitAction<IBuildAction<TNode, TEdge, TUnit>>(out var action)
                ? action
                : throw new ArgumentException(
                    $"{nameof(TUnit)} does not contain {nameof(IBuildAction<TNode, TEdge, TUnit>)}");
        }

        public BuildCommand(
            [NotNull] IBuildAction<TNode, TEdge, TUnit> buildAction,
            [NotNull] TEdge edge)
        {
            this.buildAction = buildAction ?? throw new ArgumentNullException(nameof(buildAction));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));

            engineer = this.buildAction.MyUnit;
        }

        public void Execute()
        {
            buildAction.UpRoad(edge);
        }

        public bool CanExecute()
        {
            return buildAction.CanUpRoad(edge);
        }

        public string GetLog()
        {
            return $"Инженер {engineer} улучшил дорогу {edge}";
        }

        public CommandType CommandType => buildAction.CommandType;
    }
}