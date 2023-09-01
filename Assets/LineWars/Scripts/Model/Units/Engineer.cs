using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace LineWars.Model
{
    public class Engineer: Unit, IRoadUpper
    {
        [field: Header("Engineer Settings")]
        [field: SerializeField] private IntModifier engineerPointModifier;

        protected override void Awake()
        {
            base.Awake();
            if (engineerPointModifier == null)
            {
                engineerPointModifier = DecreaseIntModifier.DecreaseOne;
                Debug.LogWarning($"{nameof(engineerPointModifier)} is null on {name}");
            }
        }

        public bool CanUpRoad([NotNull] Edge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            return Node.ContainsEdge(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && ActionPointsCondition(engineerPointModifier, CurrentActionPoints);
        }

        public void UpRoad([NotNull] Edge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LevelUp();
            ActionCompleted.Invoke();
        }
    }
}