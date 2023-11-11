using System;
using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveAction<TNode, TEdge, TUnit> :
        UnitAction<TNode, TEdge, TUnit>,
        IMoveAction<TNode, TEdge, TUnit>
        where TNode : class, INodeForGame<TNode, TEdge, TUnit>
        where TEdge : class, IEdgeForGame<TNode, TEdge, TUnit>
        where TUnit : class, IUnit<TNode, TEdge, TUnit>

    {
        public override CommandType CommandType => CommandType.Move;
        public override ActionType ActionType => ActionType.Targeted;

        public MoveAction(TUnit executor) : base(executor)
        {
        }

        public bool CanMoveTo([NotNull] TNode target, bool ignoreActionPointsCondition = false)
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
                return target.OwnerId == -1
                       || target.OwnerId == MyUnit.OwnerId
                       || target.OwnerId != MyUnit.OwnerId && target.AllIsFree;
            }
        }

        public void MoveTo([NotNull] TNode target)
        {
            var startNode = MyUnit.Node;

            if (startNode.LeftUnit == MyUnit)
                startNode.LeftUnit = null;
            if (startNode.RightUnit == MyUnit)
                startNode.RightUnit = null;

            InspectNodeForCallback();
            AssignNewNode();


            CompleteAndAutoModify();

            void InspectNodeForCallback()
            {
                if (target.OwnerId == -1)
                {
                    OnCapturingFreeNode();
                    return;
                }

                if (target.OwnerId != MyUnit.OwnerId)
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
                    target.LeftUnit = MyUnit;
                    target.RightUnit = MyUnit;
                }
                else if (target.LeftIsFree && (MyUnit.UnitDirection == UnitDirection.Left ||
                                               MyUnit.UnitDirection == UnitDirection.Right && !target.RightIsFree))
                {
                    target.LeftUnit = MyUnit;
                    MyUnit.UnitDirection = UnitDirection.Left;
                }
                else
                {
                    target.RightUnit = MyUnit;
                    MyUnit.UnitDirection = UnitDirection.Right;
                }

                if (MyUnit.OwnerId != target.OwnerId)
                    target.ConnectTo(MyUnit.OwnerId);
            }
        }

        public override void Accept(IBaseUnitActionVisitor<TNode, TEdge, TUnit> visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, TNode, TEdge, TUnit> visitor) =>
            visitor.Visit(this);

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