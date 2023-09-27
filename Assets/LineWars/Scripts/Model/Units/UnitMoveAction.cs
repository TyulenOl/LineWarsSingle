using System;
using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class UnitMoveAction : BaseUnitAction
    {
        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) => new ComponentUnit.MoveAction(unit, this);
    }
    
    public sealed partial class ComponentUnit
    {
        public class MoveAction : UnitAction, ITargetedAction
        {
            public override CommandType GetMyCommandType() => CommandType.Move;

            public MoveAction([NotNull] ComponentUnit unit, [NotNull] UnitMoveAction data) : base(unit, data)
            {
            }

            public bool CanMoveTo([NotNull] Node target, bool ignoreActionPointsCondition = false)
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

            public void MoveTo([NotNull] Node target)
            {
                if (MyUnit.Node.LeftUnit == MyUnit)
                    MyUnit.Node.LeftUnit = null;
                if (MyUnit.Node.RightUnit == MyUnit)
                    MyUnit.Node.RightUnit = null;

                InspectNodeForCallback();
                AssignNewNode();

                MyUnit.movementLogic.MoveTo(target.transform);
                SfxManager.Instance.Play(ActionSfx);
          
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
                    MyUnit.myNode = target;
                    if (MyUnit.Size == UnitSize.Large)
                    {
                        MyUnit.myNode.LeftUnit = MyUnit;
                        MyUnit.myNode.RightUnit = MyUnit;
                    }
                    else if (MyUnit.myNode.LeftIsFree && (MyUnit.UnitDirection == UnitDirection.Left ||
                                                          MyUnit.UnitDirection == UnitDirection.Right && !MyUnit.myNode.RightIsFree))
                    {
                        MyUnit.myNode.LeftUnit = MyUnit;
                        MyUnit.UnitDirection = UnitDirection.Left;
                    }
                    else
                    {
                        MyUnit.myNode.RightUnit = MyUnit;
                        MyUnit.UnitDirection = UnitDirection.Right;
                    }

                    if (MyUnit.Owner != MyUnit.myNode.Owner)
                        Owned.Connect(MyUnit.Owner, MyUnit.myNode);
                }
            }

            public bool IsMyTarget(IReadOnlyTarget target) => target is Node;

            public ICommand GenerateCommand(IReadOnlyTarget target) => new UnitMoveCommand(this, (Node) target);

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