using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class Engineer: Unit, IRoadUpper
    {
        [field: Header("Engineer Settings")]
        [field: SerializeField] private int engineerPoint;

        public bool CanUpRoad([NotNull] Edge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            return Node.ContainsEdge(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && engineerPoint > 0;
        }

        public void UpRoad([NotNull] Edge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LevelUp();
            engineerPoint--;
            ActionCompleted.Invoke();
        }
    }
}