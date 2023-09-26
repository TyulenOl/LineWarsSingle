using System;
using System.Collections.Generic;
using LineWars.Extensions.Attributes;
using LineWars.Interface;

namespace LineWars.Model
{
    public class CNode: COwned
    {
        private List<CEdge> edges;

        //Если visibility равно 0, то видна только нода, если 1, то нода и ее соседи
        private int visibility;

        /// <summary>
        /// Флаг, который указывает, что нода уже кому-то принадлежала
        /// </summary>
        public bool IsDirty { get; private set; }
        public IReadOnlyCollection<CEdge> Edges => edges;
        public bool LeftIsFree => LeftUnit == null;
        public bool RightIsFree => RightUnit == null;
        public bool AnyIsFree => LeftIsFree || RightIsFree;
        public bool AllIsFree => LeftIsFree && RightIsFree;

        public int Index { get; set; }

        public int Visibility =>
            Math.Max(visibility,
                Math.Max(
                    LeftUnit != null ? LeftUnit.Visibility : 0,
                    RightUnit != null ? RightUnit.Visibility : 0
                )
            );

        public int ValueOfHidden { get; }

        public int BaseIncome { get; }

        public CComponentUnit LeftUnit { get; set; }
        public CComponentUnit RightUnit { get; set; }

        public CommandPriorityData CommandPriorityData { get; }

        public IEnumerable<CNode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirstNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirstNode;
            }
        }

        protected override void OnSetOwner(BasePlayer oldPlayer, BasePlayer newPlayer)
        {
            IsDirty = true;
        }
    }
}