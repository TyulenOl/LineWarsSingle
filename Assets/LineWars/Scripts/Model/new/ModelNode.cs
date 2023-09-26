using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LineWars.Extensions.Attributes;
using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    public class ModelNode: ModelOwned, ITarget, IHavePosition
    {
        public int Index { get; }

        private readonly List<ModelEdge> edges;

        //Если visibility равно 0, то видна только нода, если 1, то нода и ее соседи
        private readonly int defaultVisibility;

        /// <summary>
        /// Флаг, который указывает, что нода уже кому-то принадлежала
        /// </summary>
        public bool IsDirty { get; private set; }
        public IReadOnlyCollection<ModelEdge> Edges => edges;

        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool AllIsFree => LeftIsFree && RightIsFree;

        public int Visibility =>
            Math.Max(defaultVisibility,
                Math.Max(
                    LeftUnit?.Visibility ?? 0,
                    RightUnit?.Visibility ?? 0
                )
            );

        public int ValueOfHidden { get; }

        public int BaseIncome { get; }
        public Vector2 Position { get; }

        public ModelComponentUnit LeftUnit { get; set; }
        public ModelComponentUnit RightUnit { get; set; }

        public CommandPriorityData CommandPriorityData { get; }

        public bool IsBase => this == Owner.Base;
        public bool ContainsEdge(ModelEdge edge) => edges.Contains(edge);

        public event Action<ModelBasePlayer, ModelBasePlayer> OwnerChanged;


        public ModelNode(
            [NotNull] ModelBasePlayer owner, 
            int index,
            Vector2 position,
            [NotNull] IEnumerable<ModelEdge> edges,
            int defaultVisibility,
            int valueOfHidden,
            int baseIncome,
            [NotNull] CommandPriorityData commandPriorityData): base(owner)
        {
            if (edges == null) throw new ArgumentNullException(nameof(edges));
            if (commandPriorityData == null)  throw new ArgumentNullException(nameof(commandPriorityData));

            Index = index;
            Position = position;
            this.edges = edges.ToList();
            this.defaultVisibility = defaultVisibility;
            ValueOfHidden = valueOfHidden;
            BaseIncome = baseIncome;
            CommandPriorityData = commandPriorityData;
        }

        public IEnumerable<ModelNode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }
        
        public void SetOwner([MaybeNull]ModelBasePlayer newBasePlayer)
        {
            var temp = basePlayer;
            basePlayer = newBasePlayer;
            OwnerChanged?.Invoke(temp, newBasePlayer);
        }
    }
}