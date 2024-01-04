using LineWars.Interface;
using System;
using System.Collections;
using UnityEngine;


namespace LineWars.Model
{
    public partial class UnitMeleeAttackAnimation
    {
        [Header("Move To Unit Settings")]
        [SerializeField, Min(0)] private float moveToUnitTimeInSeconds;
        [SerializeField, Range(0, 1)] private float movementRange;
        [SerializeField, Min(0)] private float pauseBeforeAttack;
        [SerializeField, Min(0)] private float pauseAfterAttack;
        [SerializeField] private SimpleEffect slashEffect;
        public class MeleeMoveToUnitState : State
        {
            public readonly UnitMeleeAttackAnimation animation;

            //public event Action Started;
            //public event Action Ended;

            
            private float movementProgress;
            private bool isPlayed;
            
            public MeleeMoveToUnitState(UnitMeleeAttackAnimation animation)
            {
                this.animation = animation;
            }

            public override void OnEnter()
            {
                base.OnEnter();
                isPlayed = false;
                animation.startPosition = animation.ownerUnit.transform.position;
                movementProgress = 0f;
                animation.StartAnimation();
            }

            public override void OnLogic()
            {
                base.OnLogic();
                if(movementProgress < animation.movementRange)
                {
                    movementProgress += Time.deltaTime / animation.moveToUnitTimeInSeconds;
                    animation.ownerUnit.transform.position = 
                        Vector2.Lerp(animation.startPosition, 
                        animation.currentTarget.transform.position, 
                        movementProgress);
                }
                else if(!isPlayed)
                {
                    isPlayed = true;
                    animation.StartCoroutine(AttackCoroutine());
                }
            }

            private IEnumerator AttackCoroutine()
            {
                yield return new WaitForSeconds(animation.pauseBeforeAttack);

                if (animation.slashEffect == null)
                {
                    Debug.LogWarning($"slash effect is null on {animation}");
                    animation.Attacked.Invoke(animation);
                    animation.EndAnimation();
                    yield break;
                }
                var helper = animation.currentTarget.GetComponent<UnitAnimationHelper>();
                var slashPosition = animation.currentTarget.UnitDirection == UnitDirection.Left
                        ? helper.LeftCenter.transform.position
                        : helper.RightCenter.transform.position;
                Instantiate(animation.slashEffect, slashPosition, Quaternion.identity);
                animation.Attacked.Invoke(animation);

                yield return new WaitForSeconds(animation.pauseAfterAttack);

                animation.stateMachine.SetState(animation.returnState);
            }
        }
    }
}


