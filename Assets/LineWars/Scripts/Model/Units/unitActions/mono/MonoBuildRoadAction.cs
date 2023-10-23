﻿using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoBuildRoadAction: MonoUnitAction,
        IBuildAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] private SFXData buildSfx;
        private BuildAction<Node, Edge, Unit, Owned, BasePlayer> BuildAction
            => (BuildAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;

        public bool CanUpRoad(Edge edge, bool ignoreActionPointsCondition = false) =>
            BuildAction.CanUpRoad(edge, ignoreActionPointsCondition);

        public void UpRoad(Edge edge)
        {
            BuildAction.UpRoad(edge);
            SfxManager.Instance.Play(buildSfx);
        }

        public bool IsMyTarget(ITarget target) => BuildAction.IsMyTarget(target);
        public ICommand GenerateCommand(ITarget target)
        {
            return new BuildCommand<Node, Edge, Unit, Owned, BasePlayer>(this, (Edge) target);
        }

        protected override ExecutorAction GetAction()
        {
            return new BuildAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this);
        }

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}