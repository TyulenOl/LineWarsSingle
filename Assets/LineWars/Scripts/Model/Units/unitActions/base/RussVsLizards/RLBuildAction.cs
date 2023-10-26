using System;
using System.Collections.Generic;
using System.Linq;

namespace LineWars.Model
{
    public class RLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
            UnitAction<TNode, TEdge, TUnit, TOwned, TPlayer>,
            IRLBuildAction<TNode, TEdge, TUnit, TOwned, TPlayer>
        #region Сonstraints
        where TNode : class, TOwned, INodeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TUnit : class, TOwned, IUnit<TNode, TEdge, TUnit, TOwned, TPlayer>
        where TOwned : class, IOwned<TOwned, TPlayer>
        where TPlayer : class, IBasePlayer<TOwned, TPlayer>
        #endregion
    {
        public IBuildingFactory Factory { get; private set; }
        public IEnumerable<BuildingType> PossibleBuildings { get; }
        public bool CanBuild(TNode node, BuildingType buildingType)
        {
            return ActionPointsCondition()
                   && node.Building == null
                   && PossibleBuildings.Contains(buildingType);
        }

        public void Build(TNode node, BuildingType buildingType)
        {
            node.Building = Factory.Create(buildingType);
        }
        
        public override CommandType CommandType => CommandType.Build;
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);

        public RLBuildAction(
            TUnit executor,
            IEnumerable<BuildingType> possibleTypes,
            IBuildingFactory factory) : base(executor)
        {
            Factory = factory;
            PossibleBuildings = possibleTypes.ToHashSet();
        }
    }
}