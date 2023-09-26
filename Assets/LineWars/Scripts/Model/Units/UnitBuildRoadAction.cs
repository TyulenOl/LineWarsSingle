using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New BuildRoadAction", menuName = "UnitActions/BuildRoadAction", order = 61)]
    public class UnitBuildRoadAction: BaseUnitAction
    {
        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit) => new ModelComponentUnit.BuildAction(unit, this);
    }
    
    public sealed partial class ModelComponentUnit
    {
        public class BuildAction: UnitAction, ITargetedAction
        {
            public BuildAction([NotNull] ModelComponentUnit unit, UnitBuildRoadAction data) : base(unit, data)
            {}
            
            public bool CanUpRoad([NotNull] ModelEdge edge, bool ignoreActionPointsCondition = false) => CanUpRoad(edge, MyUnit.Node, ignoreActionPointsCondition);

            public bool CanUpRoad([NotNull] ModelEdge edge, [NotNull] ModelNode node, bool ignoreActionPointsCondition = false)
            {
                if (edge == null) throw new ArgumentNullException(nameof(edge));
                if (node == null) throw new ArgumentNullException(nameof(node));
                
                return node.ContainsEdge(edge)
                       && LineTypeHelper.CanUp(edge.LineType)
                       && (ignoreActionPointsCondition || ActionPointsCondition());
            }

            public void UpRoad([NotNull] ModelEdge edge)
            {
                if (edge == null) throw new ArgumentNullException(nameof(edge));
                edge.LevelUp();
                CompleteAndAutoModify();
            }
            
            public override CommandType GetMyCommandType() => CommandType.Build;
            
            public bool IsMyTarget(ITarget target) => target is ModelEdge;
            public ICommand GenerateCommand(ITarget target) => new UnitUpRoadCommand(this, (ModelEdge)target);
        }
    } 
}