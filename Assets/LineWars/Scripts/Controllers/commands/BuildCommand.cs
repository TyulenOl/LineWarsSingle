using System;
using JetBrains.Annotations;


namespace LineWars.Model
{
    public class BuildCommand<TNode, TEdge, TUnit, TOwned, TPlayer>:
        ICommand
    
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer> 
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TPlayer: class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer>
        #endregion 
    {
        private readonly IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> buildAction;
        private readonly TUnit engineer;
        private readonly TEdge edge;

        public BuildCommand([NotNull] TUnit engineer, [NotNull] TEdge edge)
        {
            this.engineer = engineer ?? throw new ArgumentNullException(nameof(engineer));
            this.edge = edge ?? throw new ArgumentNullException(nameof(edge));

            buildAction = engineer.TryGetUnitAction<IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>>(out var action)
                ? action
                : throw new ArgumentException($"{nameof(TUnit)} does not contain {nameof(IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>)}");
        }

        public BuildCommand(
            [NotNull] IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> buildAction,
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
    }
}
