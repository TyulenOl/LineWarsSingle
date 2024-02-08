using System;
using System.Collections.Generic;
using System.Linq;
using LineWars.Model;

namespace LineWars
{
    public static class UnitUtilities<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>
    {
        public static bool CanMoveTo(TUnit unit, TNode targetNode)
        {
            return unit.Node != targetNode
                   && OwnerCondition()
                   && SizeCondition()
                   && LineCondition()
                   && CanOwnerMoveCondition();

            bool SizeCondition()
            {
                return unit.Size == UnitSize.Little && targetNode.AnyIsFree
                       || unit.Size == UnitSize.Large && targetNode.AllIsFree;
            }

            bool LineCondition()
            {
                var line = unit.Node.GetLine(targetNode);
                return line != null
                       && unit.CanMoveOnLineWithType(line.LineType);
            }

            bool OwnerCondition()
            {
                return targetNode.OwnerId == -1
                       || targetNode.OwnerId == unit.OwnerId
                       || targetNode.OwnerId != unit.OwnerId && targetNode.AllIsFree;
            }

            bool CanOwnerMoveCondition()
            {
                return targetNode.CanOwnerMove(unit.OwnerId);
            }
        }

        public static void MoveTo(TUnit unit, TNode targetNode)
        {
            var startNode = unit.Node;

            if (startNode.LeftUnit == unit)
                startNode.LeftUnit = null;
            if (startNode.RightUnit == unit)
                startNode.RightUnit = null;

            AssignNewNode();

            void AssignNewNode()
            {
                if (unit.Size == UnitSize.Large)
                {
                    targetNode.LeftUnit = unit;
                    targetNode.RightUnit = unit;
                }
                else if (targetNode.LeftIsFree && (unit.UnitDirection == UnitDirection.Left ||
                                               unit.UnitDirection == UnitDirection.Right && !targetNode.RightIsFree))
                {
                    targetNode.LeftUnit = unit;
                    unit.UnitDirection = UnitDirection.Left;
                }
                else
                {
                    targetNode.RightUnit = unit;
                    unit.UnitDirection = UnitDirection.Right;
                }

                if (unit.OwnerId != targetNode.OwnerId)
                    targetNode.ConnectTo(unit.OwnerId);
                unit.Node = targetNode;
            }
        }
    }
}