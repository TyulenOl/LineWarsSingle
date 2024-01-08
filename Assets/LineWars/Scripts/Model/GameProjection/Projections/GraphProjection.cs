using DataStructures;
using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class GraphProjection :
        GraphForGame<NodeProjection, EdgeProjection, UnitProjection>,
        IReadOnlyGraphProjection
    {
        public GameProjection Game { get; set; }  
        public IndexList<NodeProjection> NodesIndexList { get; set; }
        public IndexList<EdgeProjection> EdgesIndexList { get; set; }
        public IndexList<UnitProjection> UnitsIndexList { get; set; }

        public GraphProjection(IEnumerable<NodeProjection> nodes, IEnumerable<EdgeProjection> edges, GameProjection game)
            : base(nodes, edges)
        {
            NodesIndexList = new IndexList<NodeProjection>();
            EdgesIndexList = new IndexList<EdgeProjection>();
            UnitsIndexList = new IndexList<UnitProjection>();
            Game = game;
            foreach (var node in nodes)
                AddNode(node);

            foreach (var edge in edges)
                AddEdge(edge);

            foreach (var unit in UnitsIndexList.Values)
                unit.InitializeActions(this);
        }

        private new void AddNode(NodeProjection node)
        {
            NodesIndexList.Add(node.Id, node);
            if (node.LeftUnit != null)
                AddUnit(node.LeftUnit);
            if (node.RightUnit != null && node.RightUnit != node.LeftUnit)
                AddUnit(node.RightUnit);
            node.UnitAdded += OnUnitAdded;
            node.Game = Game;
        }

        private new void AddEdge(EdgeProjection edge)
        {
            EdgesIndexList.Add(edge.Id, edge);
        }

        private void AddUnit(UnitProjection unit)
        {
            UnitsIndexList.Add(unit.Id, unit);
            unit.Died += OnUnitDied;
            unit.Game = Game;
        }

        private void OnUnitDied(UnitProjection unit)
        {
            UnitsIndexList.Remove(unit.Id);
        }

        private void OnUnitAdded(UnitProjection unit)
        {
            if (unit == null) return;
            if (UnitsIndexList.ContainsKey(unit.Id)) return;
            else
            {
                if (unit.HasId)
                {
                    UnitsIndexList.Add(unit.Id, unit);
                }
                else
                {
                    var id = UnitsIndexList.Add(unit);
                    unit.SetId(id);
                }
            }
        }
    }

    public interface IReadOnlyGraphProjection
    {
        public IReadOnlyList<NodeProjection> Nodes { get; }
        public IReadOnlyList<EdgeProjection> Edges { get; }
    }
}