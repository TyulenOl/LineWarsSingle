﻿using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars.Controllers
{
    public class GetAvailableTargetActionInfoVisitor :
        IIUnitActionVisitor<IEnumerable<TargetActionInfo>, Node, Edge, Unit>
    {
        private readonly GetAvailableTargetActionInfoForShotUnitAction forShotUnitAction;

        public GetAvailableTargetActionInfoVisitor(
            GetAvailableTargetActionInfoForShotUnitAction forShotUnitAction)
        {
            this.forShotUnitAction = forShotUnitAction;
        }
        public IEnumerable<TargetActionInfo> Visit(IBuildAction<Node, Edge, Unit> action)
        {
            return action.MyUnit.Node.Edges
                .Where(action.IsAvailable)
                .Select(x => new TargetActionInfo(x, action.CommandType));
        }

        public IEnumerable<TargetActionInfo> Visit(IBlockAction<Node, Edge, Unit> action)
        {
            return ForSimple(action);
        }

        public IEnumerable<TargetActionInfo> Visit(IMoveAction<Node, Edge, Unit> action)
        {
            return ForNode(action, 1);
        }

        public IEnumerable<TargetActionInfo> Visit(IHealAction<Node, Edge, Unit> action)
        {
            return ForUnit(action, 1);
        }

        public IEnumerable<TargetActionInfo> Visit(IDistanceAttackAction<Node, Edge, Unit> action)
        {
            return ForUnit(action, action.Distance);
        }

        public IEnumerable<TargetActionInfo> Visit(IArtilleryAttackAction<Node, Edge, Unit> action)
        {
            return ForUnit(action, action.Distance);
        }

        public IEnumerable<TargetActionInfo> Visit(IMeleeAttackAction<Node, Edge, Unit> action)
        {
            return ForUnit(action, 1);
        }

        public IEnumerable<TargetActionInfo> Visit(IRLBlockAction<Node, Edge, Unit> action)
        {
            return ForSimple(action);
        }

        public IEnumerable<TargetActionInfo> Visit(IRamAction<Node, Edge, Unit> action)
        {
            return ForNode(action, 1);
        }

        public IEnumerable<TargetActionInfo> Visit(IBlowWithSwingAction<Node, Edge, Unit> action)
        {
            return ForSimple(action);
        }

        public IEnumerable<TargetActionInfo> Visit(IShotUnitAction<Node, Edge, Unit> action)
        {
            return forShotUnitAction.Visit(action);
        }

        public IEnumerable<TargetActionInfo> Visit(ISacrificeForPerunAction<Node, Edge, Unit> action)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TargetActionInfo> Visit(IRLBuildAction<Node, Edge, Unit> action)
        {
            throw new System.NotImplementedException();
        }


        private static IEnumerable<TargetActionInfo> ForSimple<TAction>(TAction action)
            where TAction : ISimpleAction, IUnitAction<Node, Edge, Unit>
        {
            if (action.CanExecute())
                return new[]
                {
                    new TargetActionInfo(null, action.CommandType)
                };
            return Enumerable.Empty<TargetActionInfo>();
        }

        private static IEnumerable<TargetActionInfo> ForNode<TAction>(TAction action, uint distance)
            where TAction : ITargetedAction<Node>, IUnitAction<Node, Edge, Unit>
        {
            return MonoGraph.Instance.GetNodesInRange(action.MyUnit.Node, distance)
                .Where(action.IsAvailable)
                .Select(e => new TargetActionInfo(e, action.CommandType));
        }

        private static IEnumerable<TargetActionInfo> ForUnit<TAction>(TAction action, uint distance)
            where TAction : ITargetedAction<Unit>, IUnitAction<Node, Edge, Unit>
        {
            return MonoGraph.Instance.GetUnitsInRange(action.MyUnit.Node, distance)
                .Where(action.IsAvailable)
                .Select(e => new TargetActionInfo(e, action.CommandType));
        }
    }

    public class GetAvailableTargetActionInfoForShotUnitAction
    {
        private ITarget[] targets;

        public GetAvailableTargetActionInfoForShotUnitAction(ITarget[] targets)
        {
            this.targets = targets;
        }

        public IEnumerable<TargetActionInfo> Visit(IShotUnitAction<Node, Edge, Unit> action)
        {
            if (targets.Length == 0)
                return MonoGraph.Instance.GetUnitsInRange(action.MyUnit.Node, 1)
                    .Where(action.IsAvailable)
                    .Select(x => new TargetActionInfo(x, action.CommandType));
            else if (targets.Length == 1)
            {
                return MonoGraph.Instance.Nodes
                    .Where(x => action.IsAvailable(targets.Concat(new[] {x}).ToArray()))
                    .Select(x => new TargetActionInfo(x, action.CommandType));
            }

            throw new Exception();
        }
    }
}