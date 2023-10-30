using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoRLBuildAction :
        MonoUnitAction<RLBuildAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IRLBuildAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [SerializeField] public List<BuildingType> initialPossibleBuildingTypes;
        public IEnumerable<BuildingType> PossibleBuildings => Action.PossibleBuildings;

        public IBuildingFactory Factory => Action.Factory;

        public bool CanBuild(Node node, BuildingType buildingType)
        {
            return Action.CanBuild(node, buildingType);
        }

        public void Build(Node node, BuildingType buildingType)
        {
            //TODO: анимации и звуки
            Action.Build(node, buildingType);
        }

        protected override RLBuildAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new RLBuildAction<Node, Edge, Unit, Owned, BasePlayer>(
                Unit,
                initialPossibleBuildingTypes,
                new MonoBuildingFactory());
        }
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}