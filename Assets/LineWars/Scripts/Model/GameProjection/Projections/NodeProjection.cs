using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        public int Score { get; private set; }
        public Node Original { get; private set; } 
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

        public IBuilding Building { get; set; }
        public IEnumerable<UnitProjection> Units => new[] {LeftUnit, RightUnit}
            .Where(x => x != null)
            .Distinct();

        public Action<UnitProjection> UnitAdded;
        public NodeProjection(CommandPriorityData commandPriorityData,
            bool isBase, int index, int score, int visibility,
            int valueOfHidden, Node original = null,
            IEnumerable<EdgeProjection> edgeProjections = null, UnitProjection leftUnit = null,
            UnitProjection rightUnit = null)
        {
            CommandPriorityData = commandPriorityData;
            IsBase = isBase;
            Id = index;
            Score = score;
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
            node.Id, node.Score, node.Visibility, node.ValueOfHidden,
            node.Original, edges, leftUnit, rightUnit)
        {
        }

        public NodeProjection(Node original, int score, IEnumerable<EdgeProjection> edgeProjections = null, UnitProjection leftUnit = null,
            UnitProjection rightUnit = null) 
            : this(original.CommandPriorityData, original.IsBase, original.Id, score,
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
        
        public T Accept<T>(INodeVisitor<T> visitor) => visitor.Visit(this);

        public EdgeProjection GetLineOfNeighbour(NodeProjection otherNode) => 
            ((INode<NodeProjection, EdgeProjection>)this).GetLine(otherNode); 

        private void OnUnitDied(UnitDirection placement, UnitProjection unit)
        {
            switch(placement)
            {
                case UnitDirection.Left:
                    leftUnit = null;
                    break;
                case UnitDirection.Right:
                    rightUnit = null;
                    break;
                default:
                    throw new ArgumentException();
                   
            }
        }
 
    }

    public interface IReadOnlyNodeProjection : INumbered
    {
        public Node Original { get; }
        public int Score { get; }
        public BasePlayerProjection Owner { get; }
        public CommandPriorityData CommandPriorityData { get; }
        public IEnumerable<EdgeProjection> Edges { get; }
        public bool IsBase { get; }
        public int Visibility { get; }
        public int ValueOfHidden { get; }

        public UnitProjection LeftUnit { get; }
        public UnitProjection RightUnit { get; }

        public bool HasOriginal => Original != null;
    }

}
