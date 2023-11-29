using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IRLBuildAction<TNode, TEdge, TUnit> :
        IUnitAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public IEnumerable<BuildingType> PossibleBuildings { get; }
        public IBuildingFactory Factory { get; }

        public bool CanBuild(TNode node, BuildingType buildingType);
        public void Build(TNode node, BuildingType buildingType);
    }
}