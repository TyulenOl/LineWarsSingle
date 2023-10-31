using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class CommandContext
    {
        public ICommandWithCommandType Command;
        public IExecutor Executor;
        public ITarget Target;
    }

    public class AllCommandsVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> :
            IIUnitActionVisitor<IEnumerable<CommandContext>, TNode, TEdge, TUnit, TOwned, TPlayer>

        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> GraphForGame { get; }

        public AllCommandsVisitor(IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graphForGame)
        {
            GraphForGame = graphForGame;
        }

        public IEnumerable<CommandContext> Visit(IBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return action.MyUnit.Node.Edges
                .Select(edge => new CommandContext()
                {
                    Command = action.GenerateCommand(edge),
                    Target = edge,
                    Executor = action.MyUnit
                });
        }

        public IEnumerable<CommandContext> Visit(IBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForSimpleAction(action);
        }

        public IEnumerable<CommandContext> Visit(IMoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForNodesInRange(action, 1);
        }

        public IEnumerable<CommandContext> Visit(IHealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForUnitsInRange(action, 1);
        }

        public IEnumerable<CommandContext> Visit(IDistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForUnitsInRange(action, action.Distance);
        }

        public IEnumerable<CommandContext> Visit(IArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForUnitsInRange(action, action.Distance);
        }

        public IEnumerable<CommandContext> Visit(IMeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForUnitsInRange(action, 1);
        }

        public IEnumerable<CommandContext> Visit(IRLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForSimpleAction(action);
        }

        public IEnumerable<CommandContext> Visit(ISacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CommandContext> Visit(IRamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForNodesInRange(action, 1);
        }

        public IEnumerable<CommandContext> Visit(IBlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            return GetCommandsForSimpleAction(action);
        }

        public IEnumerable<CommandContext> Visit(IShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CommandContext> Visit(IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Генерирует все команды для всех НОД в радиусе distance.
        /// Будте бдительны! Если action не работает с НОДАМИ, то этот код вызовет исключение.
        /// </summary>
        private IEnumerable<CommandContext> GetCommandsForNodesInRange<TAction>(TAction action, uint distance)
            where TAction : IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, ITargetedAction
        {
            return GraphForGame.GetNodesInRange(action.MyUnit.Node, distance)
                .Select(node => new CommandContext()
                {
                    Command = action.GenerateCommand(node),
                    Executor = action.MyUnit,
                    Target = node
                });
        }

        /// <summary>
        /// Генерирует все команды для всех ЮНИТОВ в радиусе distance.
        /// Будте бдительны! Если action не работает с ЮНИТАМИ, то этот код вызовет исключение.
        /// </summary>
        private IEnumerable<CommandContext> GetCommandsForUnitsInRange<TAction>(TAction action, uint distance)
            where TAction : IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, ITargetedAction
        {
            return GraphForGame.GetNodesInRange(action.MyUnit.Node, distance)
                .SelectMany(node => node.Units)
                .Select(unit => new CommandContext()
                {
                    Command = action.GenerateCommand(unit),
                    Executor = action.MyUnit,
                    Target = unit
                });
        }

        private IEnumerable<CommandContext> GetCommandsForSimpleAction<TAction>(TAction action)
            where TAction : IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>, ISimpleAction
        {
            return new[]
            {
                new CommandContext()
                {
                    Command = action.GenerateCommand(),
                    Executor = action.MyUnit
                }
            };
        }
    }
}