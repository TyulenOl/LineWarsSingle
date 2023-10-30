using System.Collections.Generic;

namespace LineWars.Model
{
    public interface IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>: 
            IUnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion

    {
        public IEnumerable<BuildingType> PossibleBuildings { get; }
        public IBuildingFactory Factory {get;}

        public bool CanBuild(TNode node, BuildingType buildingType);
        public void Build(TNode node, BuildingType buildingType);
    }
}