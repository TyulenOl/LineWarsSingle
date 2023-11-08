using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoBuildRoadAction :
        MonoUnitAction<BuildAction<Node, Edge, Unit>>,
        IBuildAction<Node, Edge, Unit>
    {
        [SerializeField] private SFXData buildSfx;

        public bool CanUpRoad(Edge edge, bool ignoreActionPointsCondition = false) =>
            Action.CanUpRoad(edge, ignoreActionPointsCondition);

        public void UpRoad(Edge edge)
        {
            Action.UpRoad(edge);
            SfxManager.Instance.Play(buildSfx);
        }
        
        protected override BuildAction<Node, Edge, Unit> GetAction()
        {
            return new BuildAction<Node, Edge, Unit>(Unit);
        }

        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}