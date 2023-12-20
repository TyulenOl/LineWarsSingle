using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace LineWars.Model
{
    public partial class UnitMeleeAttackAnimation
    {
        [Header("Return Settings")]
        [SerializeField, Min(0)] private float returnTimeInSeconds;
        public class UnitMeleeAttackReturnState : State
        {
            public readonly UnitMeleeAttackAnimation animation;

            private Vector2 returnFromPosition;
            private float movementProgress;
            public UnitMeleeAttackReturnState(UnitMeleeAttackAnimation animation)
            {
                this.animation = animation;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                returnFromPosition = animation.ownerUnit.transform.position;
                movementProgress = 0f;

                if (animation.action.Onslaught
                   && (animation.ownerUnit == animation.targetNode.LeftUnit ||
                   animation.ownerUnit == animation.targetNode.RightUnit)) 
                {
                    animation.EndAnimation();
                    animation.stateMachine.SetState(animation.idleState);
                }
            }

            public override void OnLogic()
            {
                base.OnLogic();
                if(movementProgress < 1)
                {
                    movementProgress += Time.deltaTime / animation.returnTimeInSeconds;
                    animation.ownerUnit.transform.position = Vector2.Lerp(returnFromPosition, 
                        animation.startPosition, movementProgress);
                }
                else
                {
                    animation.EndAnimation();
                    animation.stateMachine.SetState(animation.idleState);
                }
            }
        }
    }  
}
