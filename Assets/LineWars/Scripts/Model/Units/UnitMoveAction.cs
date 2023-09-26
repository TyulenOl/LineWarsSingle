using System;
using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New MoveAction", menuName = "UnitActions/MoveAction", order = 61)]
    public class UnitMoveAction : BaseUnitAction
    {
        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit) => new ModelComponentUnit.MoveAction(unit, this);
    }
    
    public sealed partial class ModelComponentUnit
    {
        public class MoveAction : UnitAction, ITargetedAction
        {
            public override CommandType GetMyCommandType() => CommandType.Move;

            public MoveAction([NotNull] ModelComponentUnit unit, [NotNull] UnitMoveAction data) : base(unit, data)
            {
            }

            public bool CanMoveTo([NotNull] ModelNode target, bool ignoreActionPointsCondition = false)
            {
                return MyUnit.Node != target
                       && OwnerCondition()
                       && SizeCondition()
                       && LineCondition()
                       && (ignoreActionPointsCondition || ActionPointsCondition());

                bool SizeCondition()
                {
                    return MyUnit.Size == UnitSize.Little && target.AnyIsFree
                           || MyUnit.Size == UnitSize.Large && target.AllIsFree;
                }

                bool LineCondition()
                {
                    var line = MyUnit.Node.GetLine(target);
                    return line != null
                           && MyUnit.CanMoveOnLineWithType(line.LineType);
                }

                bool OwnerCondition()
                {
                    return target.Owner == null 
                           || target.Owner == MyUnit.Owner 
                           || target.Owner != MyUnit.Owner && target.AllIsFree;
                }
            }

            public void MoveTo([NotNull] ModelNode target)
            {
                if (MyUnit.Node.LeftUnit == MyUnit)
                    MyUnit.Node.LeftUnit = null;
                if (MyUnit.Node.RightUnit == MyUnit)
                    MyUnit.Node.RightUnit = null;

                InspectNodeForCallback();
                AssignNewNode();
          
                CompleteAndAutoModify();

                void InspectNodeForCallback()
                {
                    if (target.Owner == null)
                    {
                        OnCapturingFreeNode();
                        return;
                    }

                    if (target.Owner != MyUnit.Owner)
                    {
                        OnCapturingEnemyNode();
                        if (target.IsBase)
                            OnCapturingEnemyBase();
                    }
                }

                void AssignNewNode()
                {
                    MyUnit.Node = target;
                    if (MyUnit.Size == UnitSize.Large)
                    {
                        MyUnit.Node.LeftUnit = MyUnit;
                        MyUnit.Node.RightUnit = MyUnit;
                    }
                    else if (MyUnit.Node.LeftIsFree && (MyUnit.UnitDirection == UnitDirection.Left ||
                                                          MyUnit.UnitDirection == UnitDirection.Right && !MyUnit.Node.RightIsFree))
                    {
                        MyUnit.Node.LeftUnit = MyUnit;
                        MyUnit.UnitDirection = UnitDirection.Left;
                    }
                    else
                    {
                        MyUnit.Node.RightUnit = MyUnit;
                        MyUnit.UnitDirection = UnitDirection.Right;
                    }

                    if (MyUnit.Owner != MyUnit.Node.Owner)
                        Owned.Connect(MyUnit.Owner, MyUnit.Node);
                }
            }

            public bool IsMyTarget(ITarget target) => target is ModelNode;

            public ICommand GenerateCommand(ITarget target) => new UnitMoveCommand(this, (ModelNode) target);

            #region CallBack

             protected virtual void OnCapturingEnemyBase()
             {
             }

             protected virtual void OnCapturingEnemyNode()
             {
             }

             protected virtual void OnCapturingFreeNode()
             {
             }

             #endregion
        }
    }
}