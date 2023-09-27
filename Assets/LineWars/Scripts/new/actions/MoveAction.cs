using System.Diagnostics.CodeAnalysis;

namespace LineWars.Model
{
    public class MoveAction: UnitAction, ITargetedAction
    {
        public MoveAction([NotNull] IUnit unit, [NotNull] UnitMoveAction data) : base(unit, data)
        {
        }
        
        public bool CanMoveTo([NotNull] IReadOnlyNode target, bool ignoreActionPointsCondition = false)
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

        public void MoveTo([NotNull] INode target)
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

                if (MyUnit.Owner != target.Owner)
                    target.ConnectTo(MyUnit.Owner);
            }
        }
            
            
        public override CommandType GetMyCommandType() => CommandType.Move;
        public bool IsMyTarget(ITarget target) => target is INode;

        public ICommand GenerateCommand(ITarget target) => new MoveCommand(this, (INode) target);
        
        #region CallBack

        protected virtual void OnCapturingEnemyBase(){}

        protected virtual void OnCapturingEnemyNode(){}

        protected virtual void OnCapturingFreeNode(){}

        #endregion
    }
}