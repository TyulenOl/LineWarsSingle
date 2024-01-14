﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LineWars.Model
{
    public class NodeProjection : OwnedProjection,
        INodeForGame<NodeProjection, EdgeProjection, UnitProjection>, 
        IReadOnlyNodeProjection, INumbered
    {
        private UnitProjection leftUnit;
        private UnitProjection rightUnit;

        public int Id { get; set; }
        public int Score { get; set; }
        public Node Original { get; set; } 
        public CommandPriorityData CommandPriorityData { get; set; }
        public List<EdgeProjection> EdgesList { get; set; }
        public bool IsBase { get; set; }
        public int Visibility { get; set; }
        public int ValueOfHidden { get; set; }
        public List<int> BannedOwnerId { get; set; }

        public UnitProjection LeftUnit 
        {
            get => leftUnit;
            set
            { 
                leftUnit = value;
                UnitAdded?.Invoke(this, value);          
            } 
        }
        public UnitProjection RightUnit
        {
            get => rightUnit;
            set
            {
                rightUnit = value;
                UnitAdded?.Invoke(this, value);
            }
        }


        public IEnumerable<EdgeProjection> Edges => EdgesList;
        public IBuilding Building { get; set; }
        public IEnumerable<UnitProjection> Units => new[] {LeftUnit, RightUnit}
            .Where(x => x != null)
            .Distinct();

        public event Action<NodeProjection, UnitProjection> UnitLeft;
        public event Action<NodeProjection, UnitProjection> UnitAdded;

        public void AddEdge(EdgeProjection edge)
        {
            if (edge.FirstNode != this && edge.SecondNode != this)
                throw new ArgumentException();

            EdgesList.Add(edge);
        }

        public EdgeProjection GetLineOfNeighbour(NodeProjection otherNode) => 
            ((INode<NodeProjection, EdgeProjection>)this).GetLine(otherNode);

        public bool CanOwnerMove(int ownerId)
        {
            return !BannedOwnerId.Contains(ownerId);
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
        public List<int> BannedOwnerId { get; }

        public UnitProjection LeftUnit { get; }
        public UnitProjection RightUnit { get; }

        public bool HasOriginal => Original != null;
    }

}
