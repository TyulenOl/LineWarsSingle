using System;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoBuildRoadAction : MonoUnitAction<BuildAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IBuildAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private BuildAction<Node, Edge, Unit, Owned, BasePlayer> BuildAction
            => (BuildAction<Node, Edge, Unit, Owned, BasePlayer>) Action;

        [SerializeField] private SFXData buildSfx;

        public bool CanUpRoad(Edge edge, bool ignoreActionPointsCondition = false) =>
            BuildAction.CanUpRoad(edge, ignoreActionPointsCondition);

        public void UpRoad(Edge edge)
        {
            BuildAction.UpRoad(edge);
            SfxManager.Instance.Play(buildSfx);
        }

        public Type TargetType => typeof(Edge);
        public bool IsMyTarget(ITarget target) => target is Edge;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new BuildCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Edge) target);
        }

        protected override BuildAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new BuildAction<Node, Edge, Unit, Owned, BasePlayer>(Unit);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}