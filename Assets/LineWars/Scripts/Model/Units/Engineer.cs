using System;
using System.Diagnostics.CodeAnalysis;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class Engineer: Unit, IRoadUpper
    {
        [field: Header("Engineer Settings")]
        [field: SerializeField] private IntModifier engineerPointModifier;

        [Header("Sound Settings")] 
        [SerializeField] private SFXData upRoadSFX;

        public IntModifier EngineerPointModifier => engineerPointModifier;

        protected override void Awake()
        {
            base.Awake();
            if (engineerPointModifier == null)
            {
                engineerPointModifier = DecreaseIntModifier.DecreaseOne;
                Debug.LogWarning($"{nameof(engineerPointModifier)} is null on {name}");
            }
        }

        public bool CanUpRoad([NotNull] Edge edge) => CanUpRoad(edge, Node);

        public bool CanUpRoad([NotNull] Edge edge, Node node)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            return node.ContainsEdge(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && ActionPointsCondition(engineerPointModifier, CurrentActionPoints);
        }

        public void UpRoad([NotNull] Edge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LevelUp();
            ActionCompleted.Invoke();
            SfxManager.Instance.Play(upRoadSFX);
        }
    }
}