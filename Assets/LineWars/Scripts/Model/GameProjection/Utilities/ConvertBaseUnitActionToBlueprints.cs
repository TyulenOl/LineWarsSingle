using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace LineWars.Model
{
    public class ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit> 
        : IBaseUnitActionVisitor<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit> 
    {
        public IGraphForGame<TNode, TEdge, TUnit> Graph {  get; private set; }
        public List<ICommandBlueprint> BlueprintList { get; private set; }


        public ConvertBaseUnitActionToBlueprints(IGraphForGame<TNode, TEdge, TUnit> graph,
            List<ICommandBlueprint> list)
        {
            Graph = graph;
            BlueprintList = list;
        }

        public void Visit(BuildAction<TNode, TEdge, TUnit> action)
        {
            foreach(var edge in action.Executor.Node.Edges)
            {
                if(action.CanUpRoad(edge))
                {
                    var command = new BuildCommandBlueprint(action.Executor.Id, edge.Id);
                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlockAction<TNode, TEdge, TUnit> action)
        {
            if(action.CanBlock())
            {
                var command = new BlockCommandBlueprint(action.Executor.Id);

                BlueprintList.Add(command);
            }
        }

        public void Visit(MoveAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                
                if(action.CanMoveTo(node) && (!node.IsBase || action.Executor.OwnerId == node.OwnerId))
                {

                    var command = new MoveCommandBlueprint(action.Executor.Id, node.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(HealAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
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
                    var unitId = action.Executor.Id;
                    var command = new HealCommandBlueprint(unitId, unit.Id);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(DistanceAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, (uint)action.Executor.MaxActionPoints);
        }

        public void Visit(ArtilleryAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, (uint)action.Executor.MaxActionPoints);
        }

        public void Visit(MeleeAttackAction<TNode, TEdge, TUnit> action)
        {
            ProcessAttackAction(action, 2);
        }

        public void Visit(RLBlockAction<TNode, TEdge, TUnit> action)
        {
            if(action.CanBlock())
            {
                var unitId = action.Executor.Id;
                var command = new RlBlockCommandBlueprint(unitId);
                BlueprintList.Add(command);
            }
        }
        
        public void Visit(RamAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if(action.CanRam(node))
                {
                    var unitId = action.Executor.Id;
                    var nodeId = node.Id;
                    var command = new RamCommandBlueprint(unitId, nodeId);

                    BlueprintList.Add(command);
                }
            }
        }
        
        public void Visit(SacrificeForPerunAction<TNode, TEdge, TUnit> action)
        {
            var unitId = action.Executor.Id;

            foreach (var node in Graph.Nodes)
            {
                if(action.CanSacrifice(node))
                {
                    var nodeId = node.Id;
                    var command = new PerunActionCommandBlueprint(unitId, nodeId);

                    BlueprintList.Add(command);
                }
            }
        }

        public void Visit(BlowWithSwingAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if(!node.LeftIsFree)
                {
                    ProcessUnit(node);  
                    return;
                }
                if(!node.RightIsFree)
                {
                    ProcessUnit(node);
                    return;
                }
            }

            void ProcessUnit(TNode node)
            {
                var command = new SwingCommandBlueprint(action.Executor.Id, node.LeftUnit.Id);
                BlueprintList.Add(command);
            }
        }
        public void Visit(ShotUnitAction<TNode, TEdge, TUnit> action)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, 1))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                foreach (var nodeTarget in Graph.Nodes)
                {
                    if (action.IsAvailable(unit, nodeTarget))
                        BlueprintList.Add(new ShotUnitBlueprint(action.Executor.Id, unit.Id, nodeTarget.Id));
                }
            }    
        }

        public void Visit(RLBuildAction<TNode, TEdge, TUnit> action)
        {
            // TODO 
        }

        private void ProcessAttackAction(AttackAction<TNode, TEdge, TUnit> action, uint range)
        {
            foreach (var node in Graph.GetNodesInRange(action.Executor.Node, range))
            {
                if (!node.LeftIsFree)
                    ProcessUnit(node.LeftUnit);
                if (!node.RightIsFree)
                    ProcessUnit(node.RightUnit);
            }

            void ProcessUnit(TUnit unit)
            {
                if (action.CanAttackFrom(action.Executor.Node, unit))
                {
                    var unitId = action.Executor.Id;
                    var command = new AttackCommandBlueprint(unitId, unit.Id, AttackCommandBlueprint.Target.Unit);

                    BlueprintList.Add(command);
                }
            }
        } 

        public void Visit(HealYourselfAction<TNode, TEdge, TUnit> action)
        {
            // TODO 
        }

        public void Visit(StunAttackAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }


        public void Visit(HealingAttackAction<TNode, TEdge, TUnit> action)
        {
            // TODO
        }
    }

    public static class ConvertUnitActionToBlueprints
    {
        public static ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit> Create<TNode, TEdge, TUnit>(
            IGraphForGame<TNode, TEdge, TUnit> graph,
            List<ICommandBlueprint> list)
            where TNode : class, INodeForGame<TNode, TEdge, TUnit>
            where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
            where TUnit : class, IUnit<TNode, TEdge, TUnit>
        {
            return new ConvertBaseUnitActionToBlueprints<TNode, TEdge, TUnit>(graph, list);
        }
    }
}
