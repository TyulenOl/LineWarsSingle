using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveCommand<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>:
        ICommand
    
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TOwned : class, IOwned<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TPlayer : class, IBasePlayer<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
        where TNation : class, INation<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>
    {
        private readonly IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> moveAction;
        private readonly TUnit unit;
        private readonly TNode start;
        private readonly TNode end;
        
        public MoveCommand([NotNull] TUnit unit, [NotNull] TNode end)
        {
            this.unit = unit ?? throw new ArgumentNullException(nameof(unit));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            this.start = unit.Node;
            
            moveAction = unit.TryGetUnitAction<IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>>(out var action) 
                ? action 
                : throw new ArgumentException($"{unit} does not contain {nameof(IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation>)}");
        }

        public MoveCommand(
            [NotNull] IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer, TNation> moveAction, 
            [NotNull] TNode end)
        {
            this.moveAction = moveAction ?? throw new ArgumentNullException(nameof(moveAction));
            this.end = end ?? throw new ArgumentNullException(nameof(end));
            
            unit = this.moveAction.MyUnit;
            start = unit.Node;
        }

        public void Execute()
        {
            moveAction.MoveTo(end);
        }

        public bool CanExecute()
        {
            return moveAction.CanMoveTo(end);
        }

        public string GetLog()
        {
            return $"Юнит {unit} переместился из {start} в {end}";
        }
    }
}