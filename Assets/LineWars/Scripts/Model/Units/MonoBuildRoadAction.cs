using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New BuildRoadAction", menuName = "UnitActions/BuildRoadAction", order = 61)]
    public class MonoBuildRoadAction: MonoUnitAction
    {
        public override UnitAction GetAction(ComponentUnit unit) => new BuildAction(unit, this);
    }
    
    // public sealed partial class ComponentUnit
    // {
    //     public class BuildAction: UnitAction, ITargetedAction
    //     {
    //         public BuildAction([NotNull] ComponentUnit unit, UnitBuildRoadAction data) : base(unit, data)
    //         {}
    //         
    //         public bool CanUpRoad([NotNull] Edge edge, bool ignoreActionPointsCondition = false) => CanUpRoad(edge, MyUnit.Node, ignoreActionPointsCondition);
    //
    //         public bool CanUpRoad([NotNull] Edge edge, [NotNull] Node node, bool ignoreActionPointsCondition = false)
    //         {
    //             if (edge == null) throw new ArgumentNullException(nameof(edge));
    //             if (node == null) throw new ArgumentNullException(nameof(node));
    //             
    //             return node.ContainsEdge(edge)
    //                    && LineTypeHelper.CanUp(edge.LineType)
    //                    && (ignoreActionPointsCondition || ActionPointsCondition());
    //         }
    //
    //         public void UpRoad([NotNull] Edge edge)
    //         {
    //             if (edge == null) throw new ArgumentNullException(nameof(edge));
    //             edge.LevelUp();
    //             SfxManager.Instance.Play(ActionSfx);
    //             
    //             CompleteAndAutoModify();
    //         }
    //         
    //         public override CommandType GetMyCommandType() => CommandType.Build;
    //         
    //         public bool IsMyTarget(IReadOnlyTarget target) => target is Edge;
    //         public ICommand GenerateCommand(IReadOnlyTarget target) => new UnitUpRoadCommand(this, (Edge)target);
    //     }
    // } 
}