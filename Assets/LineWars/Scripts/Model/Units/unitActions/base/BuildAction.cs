﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LineWars.Model
{
    public class BuildAction <TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>, 
        IBuildAction<TNode, TEdge, TUnit>
    
        #region Сonstraints
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit> 
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
        #endregion 
    {
        public BuildAction(TUnit executor) : base(executor)
        {
        }
        
        public bool CanUpRoad([NotNull] TEdge edge, bool ignoreActionPointsCondition = false)
            => CanUpRoad(edge, MyUnit.Node, ignoreActionPointsCondition);

        public bool CanUpRoad([NotNull] TEdge edge, [NotNull] TNode node, bool ignoreActionPointsCondition = false)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.Edges.Contains(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public void UpRoad([NotNull] TEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LineType = LineTypeHelper.Up(edge.LineType);

            CompleteAndAutoModify();
        }

        public override CommandType CommandType => CommandType.Build;

        public Type TargetType => typeof(TEdge);
        public bool IsMyTarget(ITarget target) => target is TEdge;

        public ICommandWithCommandType GenerateCommand(ITarget target)
        {
            return new BuildCommand<TNode, TEdge, TUnit>(this, (TEdge)target);
        }

        public override void Accept(IUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
        public override TResult Accept<TResult>(IIUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) => visitor.Visit(this);
    }
}