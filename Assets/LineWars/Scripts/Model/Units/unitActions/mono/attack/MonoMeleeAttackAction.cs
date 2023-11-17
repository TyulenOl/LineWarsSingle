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
            if (Executor is Unit unit)
                attackAnimation.Initialize(unit);
            else
            {
                Debug.LogWarning("Attempt to add animation on not unit!");
                attackAnimation = null;
            }
        }

        private void AttackWithAnimation(Unit targetUnit)
        {
            attackAnimation.Attacked.AddListener(AttackOnEvent);
            attackAnimation.Execute(targetUnit);

            void AttackOnEvent(UnitMeleeAttackAnimation _)
            {
                base.Attack(targetUnit);
                attackAnimation.Attacked.RemoveListener(AttackOnEvent);
            }
        }

        protected override MeleeAttackAction<Node, Edge, Unit> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit>(
                Unit,
                InitialDamage,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }


        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) => visitor.Visit(this);
    }
}