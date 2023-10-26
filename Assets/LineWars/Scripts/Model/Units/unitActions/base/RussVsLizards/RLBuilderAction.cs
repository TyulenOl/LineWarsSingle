using System;
using System.Collections.Generic;

namespace LineWars.Model
{
    public class RLBuilderAction<TNode, TEdge, TUnit, TOwned, TPlayer> :
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
        private IBuildingFactory Factory { get; set; }

        public IEnumerable<BuildingType> PossibleBuildings { get; }
        public bool CanBuild(TNode node, BuildingType buildingType)
        {
            throw new NotImplementedException();
        }

        public void Build(TNode node, BuildingType buildingType)
        {
            throw new NotImplementedException();
        }
        
        
        public Type TargetType { get; } = typeof(TNode);
        public bool IsMyTarget(ITarget target) => target is TNode;
        
        public override CommandType CommandType => CommandType.Build;
        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit, TOwned, TPlayer> visitor) => visitor.Visit(this);

        public RLBuilderAction(TUnit executor, IBuildingFactory factory) : base(executor)
        {
            Factory = factory;
        }
    }
}