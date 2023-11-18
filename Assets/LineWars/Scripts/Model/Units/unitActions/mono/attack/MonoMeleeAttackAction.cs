using LineWars.Interface;
using UnityEngine;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoMoveAction))]
    public class MonoMeleeAttackAction :
        MonoAttackAction<MeleeAttackAction<Node, Edge, Unit>>,
        IMeleeAttackAction<Node, Edge, Unit>
    {
        [field: SerializeField] public UnitBlockerSelector InitialBlockerSelector { get; private set; }

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [field: SerializeField]
        public bool InitialOnslaught { get; private set; }

        [SerializeField] private UnitMeleeAttackAnimation attackAnimation;

        public bool Onslaught => Action.Onslaught;
        public UnitBlockerSelector BlockerSelector => Action.BlockerSelector;

        public override void Initialize()
        {
            base.Initialize();
            TryInitializeAttackAnimation();
            Action.Moved += node => Executor.MovementLogic.MoveTo(node.transform.position);
        }


        public override void Attack(ITargetedAlive enemy)
        {
            if (enemy is Unit unit && attackAnimation != null)
                AttackWithAnimation(unit);
            else
                base.Attack(enemy);
        }

        private void TryInitializeAttackAnimation()
        {
            if (attackAnimation == null)
                return;
            attackAnimation.Initialize(Executor);
        }

        private void AttackWithAnimation(Unit targetUnit)
        {
            attackAnimation.Attacked.AddListener(AttackOnEvent);
            var animContext = new AnimationContext()
            {
                TargetUnit = targetUnit,
                TargetNode = targetUnit.Node,
            };
            attackAnimation.Execute(animContext);

            void AttackOnEvent(UnitMeleeAttackAnimation _)
            {
                base.Attack(targetUnit);
                attackAnimation.Attacked.RemoveListener(AttackOnEvent);
                if (targetUnit.TryGetComponent(out AnimationResponses responses))
                {
                    var animContext = new AnimationContext()
                    {
                        TargetNode = Executor.Node,
                        TargetUnit = Executor
                    };

                    responses.Respond(AnimationResponseType.MeleeDamaged, animContext);
                }
            }
        }

        protected override MeleeAttackAction<Node, Edge, Unit> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit>(
                Executor,
                InitialDamage,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }


        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}