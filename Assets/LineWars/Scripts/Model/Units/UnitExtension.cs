using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class UnitExtension
    {
        public static int GetMaxDamage(this Unit unit) => GetMaxDamage<Node, Edge, Unit>(unit);

        public static int GetMaxDamage<TNode, TEdge, TUnit>(this TUnit unit)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
        {
            var actions = GetDamages<TNode, TEdge, TUnit>(unit)
                .ToArray();
            return actions.Length != 0
                ? actions.Max(x => x.Item2)
                : 0;
        }

        public static IEnumerable<(CommandType, int)> GetDamages(this Unit unit) => GetDamages<Node, Edge, Unit>(unit);

        public static IEnumerable<(CommandType, int)> GetDamages<TNode, TEdge, TUnit>(this TUnit unit)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
        {
            var actions = unit.Actions
                .OfType<IActionWithDamage>()
                .ToArray();
            foreach (var action in actions)
            {
                if (action is IUnitAction<TNode, TEdge, TUnit> unitAction)
                {
                    yield return (unitAction.CommandType, action.Damage);
                }
            }
        }

        // public static IEnumerable<ICommandWithCommandType> GetCommandsForNode(this Unit unit, Node node)
        // {
        //     return node.Targets
        //         .Where(target => unit.TargetTypeActionsDictionary.ContainsKey(target.GetType()))
        //         .Select(target => unit.TargetTypeActionsDictionary[target.GetType()]
        //             .Select(action => action.GenerateCommand(target)))
        //         .SelectMany(x => x);
        // }
    }
}