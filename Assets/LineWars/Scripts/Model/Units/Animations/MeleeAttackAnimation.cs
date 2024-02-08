using LineWars.Interface;
using System;
using System.Collections;
using UnityEngine;

namespace LineWars.Model
{
    public class MeleeAttackAnimation : ActionUnitAnimation
    {
        [SerializeField] private UnitAnimation moveAnimation;
        [SerializeField, Range(0, 1)] private float movementRange;
        [SerializeField, Min(0)] private float pauseBeforeAttack;
        [SerializeField, Min(0)] private float pauseAfterAttack;
        [SerializeField] private SimpleEffect slashEffect;

        public override void Execute(AnimationContext context)
        {
            var unit = context.TargetUnit;
            var vectorToNode = ((Vector2)unit.Node.transform.position - (Vector2)transform.position) * movementRange;
            var destination = (Vector2)transform.position + vectorToNode;
            var context1 = new AnimationContext()
            {
                TargetPosition = destination
            };
            StartAnimation();
            moveAnimation.Ended.AddListener(OnFirstMoveEnd);
            moveAnimation.Execute(context1);

            void OnFirstMoveEnd(UnitAnimation animation)
            {
                StartCoroutine(AttackCorountine());
            }

            IEnumerator AttackCorountine()
            {
                moveAnimation.Ended.RemoveListener(OnFirstMoveEnd);
                //var targetNode = unit.Node;

                yield return new WaitForSeconds(pauseBeforeAttack);
                givenMethod();
                CreateSlashEffect(unit);
                yield return new WaitForSeconds(pauseAfterAttack);

                var context2 = new AnimationContext()
                {
                    TargetPosition = ownerUnit.Node.transform.position
                };
                moveAnimation.Ended.AddListener(OnSecondMoveEnd);
                moveAnimation.Execute(context2);
            }

            void OnSecondMoveEnd(UnitAnimation animation)
            {
                moveAnimation.Ended.RemoveListener(OnSecondMoveEnd);
                EndAnimation();
            }
        }

        private void CreateSlashEffect(Unit unit)
        {
            var helper = unit.GetComponent<UnitAnimationHelper>();
            if (slashEffect != null)
            {
                var slashPosition = unit.UnitDirection == UnitDirection.Left
                        ? helper.LeftCenter.transform.position
                        : helper.RightCenter.transform.position;
                Instantiate(slashEffect, slashPosition, Quaternion.identity);
            }
        }
    }
}
