using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public static class UnitExtension
    {
        
        public static int GetMaxDamage(this Unit unit) => GetMaxDamage<Node, Edge, Unit, Owned, BasePlayer>(unit);
        public static int GetMaxDamage<TNode, TEdge, TUnit, TOwned, TPlayer>(this TUnit unit)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TOwned, TPlayer>
            #endregion

        {
            var actions = GetDamages<TNode, TEdge, TUnit, TOwned, TPlayer>(unit)
                .ToArray();
            return actions.Length != 0
                ? actions.Max(x => x.Item2)
                : 0;
        }

        public static IEnumerable<(CommandType, int)> GetDamages(this Unit unit) => GetDamages<Node, Edge, Unit, Owned, BasePlayer>(unit);
        public static IEnumerable<(CommandType, int)> GetDamages<TNode, TEdge, TUnit, TOwned, TPlayer>(this TUnit unit)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TOwned, TPlayer>
            #endregion
        {
            var actions = unit.Actions
                .OfType<IActionWithDamage>()
                .ToArray();
            foreach (var action in actions)
            {
                if (action is IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> unitAction)
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