// using System.Collections.Generic;
// using System.Linq;
//
// namespace LineWars.Model
// {
//     public class AllCommandsVisitor<TNode, TEdge, TUnit> :
//         IIUnitActionVisitor<IEnumerable<IActionCommand>, TNode, TEdge, TUnit>
//         where TNode : class, INodeForGame<TNode, TEdge, TUnit>
//         where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
//         where TUnit : class, IUnit<TNode, TEdge, TUnit>
//
//     {
//         public IGraphForGame<TNode, TEdge, TUnit> GraphForGame { get; }
//
//         public AllCommandsVisitor(IGraphForGame<TNode, TEdge, TUnit> graphForGame)
//         {
//             GraphForGame = graphForGame;
//         }
//
//         public IEnumerable<IActionCommand> Visit(IBuildAction<TNode, TEdge, TUnit> action)
//         {
//             return action.MyUnit.Node.Edges
//                 .Select(action.GenerateCommand);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IBlockAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForSimpleAction(action);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IMoveAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForNodesInRange(action, 1);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IHealAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForUnitsInRange(action, 1);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IDistanceAttackAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForUnitsInRange(action, action.Distance);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IArtilleryAttackAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForUnitsInRange(action, action.Distance);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IMeleeAttackAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForUnitsInRange(action, 1);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IRLBlockAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForSimpleAction(action);
//         }
//
//         public IEnumerable<IActionCommand> Visit(ISacrificeForPerunAction<TNode, TEdge, TUnit> action)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public IEnumerable<IActionCommand> Visit(IRamAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForNodesInRange(action, 1);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IBlowWithSwingAction<TNode, TEdge, TUnit> action)
//         {
//             return GetCommandsForSimpleAction(action);
//         }
//
//         public IEnumerable<IActionCommand> Visit(IShotUnitAction<TNode, TEdge, TUnit> action)
//         {
//             throw new System.NotImplementedException();
//         }
//
//         public IEnumerable<IActionCommand> Visit(IRLBuildAction<TNode, TEdge, TUnit> action)
//         {
//             throw new System.NotImplementedException();
//         }
//
//
//         /// <summary>
//         /// Генерирует все команды для всех НОД в радиусе distance.
//         /// </summary>
//         private IEnumerable<IActionCommand> GetCommandsForNodesInRange<TAction>(TAction action, uint distance)
//             where TAction : IUnitAction<TNode, TEdge, TUnit>, ITargetedAction<TNode>
//         {
//             return GraphForGame.GetNodesInRange(action.MyUnit.Node, distance)
//                 .Select(action.GenerateCommand);
//         }
//
//         /// <summary>
//         /// Генерирует все команды для всех ЮНИТОВ в радиусе distance.
//         /// </summary>
//         private IEnumerable<IActionCommand> GetCommandsForUnitsInRange<TAction>(TAction action, uint distance)
//             where TAction : IUnitAction<TNode, TEdge, TUnit>, ITargetedAction<TUnit>
//         {
//             return GraphForGame.GetNodesInRange(action.MyUnit.Node, distance)
//                 .SelectMany(node => node.Units)
//                 .Select(action.GenerateCommand);
//         }
//
//         private IEnumerable<IActionCommand> GetCommandsForSimpleAction<TAction>(TAction action)
//             where TAction : IUnitAction<TNode, TEdge, TUnit>, ISimpleAction
//         {
//             return new[]
//             {
//                 action.GenerateCommand()
//             };
//         }
//     }
// }