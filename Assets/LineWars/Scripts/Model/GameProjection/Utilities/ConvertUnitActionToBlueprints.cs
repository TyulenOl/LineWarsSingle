using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LineWars.Model
{
    public class ConvertUnitActionToBlueprints<TNode, TEdge, TUnit, TOwned, TPlayer> 
        : IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion 
    {
        public IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> Graph {  get; private set; }
        public List<ICommandBlueprint> BlueprintList { get; private set; }


        public ConvertUnitActionToBlueprints(IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph,
            List<ICommandBlueprint> list)
        {
            Graph = graph;
            BlueprintList = list;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            foreach(var edge in action.MyUnit.Node.Edges)
            {
                if(action.CanUpRoad(edge))
                {
                    var command = new BuildCommandBlueprint(action.MyUnit.Id, edge.Id);
                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            if(action.CanBlock())
            {
                var command = new BlockCommandBlueprint(action.MyUnit.Id);

                BlueprintList.Add(command);
            }
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.MyUnit.Node, 1))
            {
                
                if(action.CanMoveTo(node) && (!node.IsBase || action.MyUnit.Owner == node.Owner))
                {

                    var command = new MoveCommandBlueprint(action.MyUnit.Id, node.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(HealAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.MyUnit.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (action.CanHeal(unit))
                {
                    var unitId = action.MyUnit.Id;
                    var command = new HealCommandBlueprint(unitId, unit.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            ProcessAttackAction(action, (uint)action.MyUnit.MaxActionPoints);
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            ProcessAttackAction(action, (uint)action.MyUnit.MaxActionPoints);
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            ProcessAttackAction(action, 2);
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            if(action.CanBlock())
            {
                var unitId = action.MyUnit.Id;
                var command = new RlBlockCommandBlueprint(unitId);
                BlueprintList.Add(command);
            }
        }

        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            
        }

        public void Visit(RamAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.MyUnit.Node, 2)) //TODO: 2 -> 1
            {
                if(action.CanRam(node))
                {
                    var unitId = action.MyUnit.Id;
                    var nodeId = node.Id;
                    var command = new RamCommandBlueprint(unitId, nodeId);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
          
        }

        public void Visit(ShotUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
           
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> action)
        {
            
        }


        private void ProcessAttackAction(AttackAction<TNode, TEdge, TUnit, TOwned, TPlayer> action, uint range)
        {
            foreach (var node in Graph.GetNodesInRange(action.MyUnit.Node, range))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (action.CanAttackFrom(action.MyUnit.Node, unit))
                {
                    var unitId = action.MyUnit.Id;
                    var command = new AttackCommandBlueprint(unitId, unit.Id, AttackCommandBlueprint.Target.Unit);

                    BlueprintList.Add(command);
                }
            }
        }
    }

    public static class ConvertUnitActionToBlueprints
    {
        public static ConvertUnitActionToBlueprints<TNode, TEdge, TUnit, TOwned, TPlayer>
            Create<TNode, TEdge, TUnit, TOwned, TPlayer>
            (IGraphForGame<TNode, TEdge, TUnit, TOwned, TPlayer> graph,
            List<ICommandBlueprint> list)
            #region Сonstraints
            where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
            where TOwned : class, IOwned<TOwned, TPlayer>
            where TPlayer : class, IBasePlayer<TOwned, TPlayer>
            #endregion
        {
            return new ConvertUnitActionToBlueprints
                <TNode, TEdge, TUnit, TOwned, TPlayer> 
                (graph, list);
        }
    }
}
