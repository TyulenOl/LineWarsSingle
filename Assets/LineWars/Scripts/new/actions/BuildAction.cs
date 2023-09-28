using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineWars.Model
{
    public class BuildAction : UnitAction, ITargetedAction
    {
        public BuildAction([NotNull] IUnit unit, [NotNull] MonoBuildRoadAction data) : base(unit, data)
        {
        }

        public bool CanUpRoad([NotNull] IReadOnlyEdge edge, bool ignoreActionPointsCondition = false)
            => CanUpRoad(edge, MyUnit.Node, ignoreActionPointsCondition);

        public bool CanUpRoad([NotNull] IReadOnlyEdge edge, [NotNull] IReadOnlyNode node,
            bool ignoreActionPointsCondition = false)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            if (node == null) throw new ArgumentNullException(nameof(node));

            return node.ContainsEdge(edge)
                   && LineTypeHelper.CanUp(edge.LineType)
                   && (ignoreActionPointsCondition || ActionPointsCondition());
        }

        public void UpRoad([NotNull] IEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));
            edge.LevelUp();

            CompleteAndAutoModify();
        }

        public bool IsMyTarget(ITarget target) => target is IEdge;
        public ICommand GenerateCommand(ITarget target) => new BuildCommand(this, (IEdge) target);
        public override CommandType GetMyCommandType() => CommandType.Build;
    }
}