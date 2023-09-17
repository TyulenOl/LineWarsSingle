using System.Drawing;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MoveAction: UnitAction
    {
        public override CommandType GetMyCommandType() => CommandType.Move;
        private Node Node => MyUnit.Node;
        private UnitSize Size => MyUnit.Size;
        private BasePlayer Owner => MyUnit.Owner;
        
        public bool CanMoveTo([NotNull] Node target)
        {
            return Node != target
                   && OwnerCondition()
                   && SizeCondition()
                   && LineCondition()
                   && ActionPointsCondition();

            bool SizeCondition()
            {
                return Size == UnitSize.Little && target.AnyIsFree
                       || Size == UnitSize.Large && target.AllIsFree;
            }

            bool LineCondition()
            {
                var line = Node.GetLine(target);
                return line != null
                       && MyUnit.CanMoveOnLineWithType(line.LineType);
            }

            bool OwnerCondition()
            {
                return target.Owner == null || target.Owner == Owner || target.Owner != Owner && target.AllIsFree;
            }
        }

        public void MoveTo([NotNull] Node target)
        {
            if (Node.LeftUnit == this)
                Node.LeftUnit = null;
            if (Node.RightUnit == this)
                Node.RightUnit = null;

            InspectNodeForCallback();
            AssignNewNode();

            movementLogic.MoveTo(target.transform);
            MyUnit.CurrentActionPoints = ModifyActionPoints();
            SfxManager.Instance.Play(moveSFX);
            Complete();
            
            void InspectNodeForCallback()
            {
                if (target.Owner == null)
                {
                    OnCapturingFreeNode();
                    return;
                }

                if (target.Owner != this.Owner)
                {
                    OnCapturingEnemyNode();
                    if (target.IsBase)
                        OnCapturingEnemyBase();
                }
            }
            void AssignNewNode()
            {
                node = target;
                if (Size == UnitSize.Large)
                {
                    node.LeftUnit = this;
                    node.RightUnit = this;
                }
                else if (node.LeftIsFree && (UnitDirection == UnitDirection.Left ||
                                             UnitDirection == UnitDirection.Right && !node.RightIsFree))
                {
                    node.LeftUnit = this;
                    UnitDirection = UnitDirection.Left;
                }
                else
                {
                    node.RightUnit = this;
                    UnitDirection = UnitDirection.Right;
                }

                if (this.Owner != node.Owner)
                    Owned.Connect(Owner, node);
            }
        }
    }
}