using AYellowpaper.SerializedCollections.KeysGenerators;
using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class NodeProjection
        : OwnedProjection, INodeForGame<NodeProjection, EdgeProjection,
            UnitProjection, OwnedProjection, BasePlayerProjection>, 
        IReadOnlyNodeProjection, INumbered
    {
        private List<EdgeProjection> edges;
        private UnitProjection leftUnit;
        private UnitProjection rightUnit;
        public Node Original { get; private set; }
        public GraphProjection Graph { get; private set; }
        public CommandPriorityData CommandPriorityData { get; private set; }
        public IEnumerable<EdgeProjection> Edges => edges;
        public bool IsBase { get; private set; }
        public int Id { get; private set; }
        public int Visibility { get; private set; }
        public int ValueOfHidden { get; private set; }

        public UnitProjection LeftUnit 
        {
            get => leftUnit;
            set
            {
                leftUnit = value;
                UnitAdded?.Invoke(value);
            } 
        }
        public UnitProjection RightUnit
        {
            get => rightUnit;
            set
            {
                rightUnit = value;
                UnitAdded?.Invoke(value);
            }
        }

        public Action<UnitProjection> UnitAdded;
        public NodeProjection(CommandPriorityData commandPriorityData,
            bool isBase, int index, int visibility,
            int valueOfHidden, Node original = null,
            IEnumerable<EdgeProjection> edgeProjections = null, UnitProjection leftUnit = null,
            UnitProjection rightUnit = null)
        {
            CommandPriorityData = commandPriorityData;
            IsBase = isBase;
            Id = index;
            Visibility = visibility;
            ValueOfHidden = valueOfHidden;
            this.leftUnit = leftUnit;
            this.rightUnit = rightUnit;
            Original = original;
            edges = edgeProjections == null ? 
                new List<EdgeProjection>() : new List<EdgeProjection>(edgeProjections);
        }

        public NodeProjection(IReadOnlyNodeProjection node, IEnumerable<EdgeProjection> edges = null,
            UnitProjection leftUnit = null, UnitProjection rightUnit = null) 
            : this(node.CommandPriorityData, node.IsBase,
            node.Id, node.Visibility, node.ValueOfHidden, 
            node.Original, edges, leftUnit, rightUnit)
        {
        }

        public NodeProjection(Node original, IEnumerable<EdgeProjection> edgeProjections = null, UnitProjection leftUnit = null,
            UnitProjection rightUnit = null) 
            : this(original.CommandPriorityData, original.IsBase, original.Id, 
                  original.Visibility, original.ValueOfHidden, original, edgeProjections, leftUnit, rightUnit)
        {
        }

        public IEnumerable<NodeProjection> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.SecondNode == this) yield return edge.FirstNode;
                else yield return edge.SecondNode;
            }
        }

        public void AddEdge(EdgeProjection edge)
        {
            if (edge.FirstNode != this && edge.SecondNode != this)
                throw new ArgumentException();

            edges.Add(edge);
        }
    }

    public interface IReadOnlyNodeProjection
    {
        public Node Original { get; }
        public GraphProjection Graph { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public IEnumerable<EdgeProjection> Edges { get; }
        public bool IsBase { get; }
        public int Id { get; }
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public UnitProjection LeftUnit { get; }
        public UnitProjection RightUnit { get; }

        public bool HasOriginal => Original != null;
    }

}
