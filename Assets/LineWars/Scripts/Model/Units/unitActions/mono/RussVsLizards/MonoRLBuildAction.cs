using System.Collections.Generic;

namespace LineWars.Model
{
    public class MonoRLBuildAction :
        MonoUnitAction<RLBuilderAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IRLBuildAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        public IEnumerable<BuildingType> PossibleBuildings => Action.PossibleBuildings;
        
        public bool CanBuild(Node node, BuildingType buildingType)
        {
            return Action.CanBuild(node, buildingType);
        }

        public void Build(Node node, BuildingType buildingType)
        {
            //TODO: анимации и звуки
            Action.Build(node, buildingType);
        }

        protected override RLBuilderAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new RLBuilderAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, new MonoBuildingFactory());
        }
        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);
    }
}