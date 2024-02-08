
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
        [field: SerializeField] public bool InitialOnslaught { get; private set; }

        [SerializeField] private ActionUnitAnimation attackAnimation;
        public bool Onslaught => Action.Onslaught;
        public UnitBlockerSelector BlockerSelector => Action.BlockerSelector;
        protected override bool NeedAutoComplete => false;
        
        public override void Attack(ITargetedAlive enemy)
        {
            if (enemy is Unit unit && attackAnimation != null)
                AttackWithAnimation(unit);
            else
            {
                InstantAttack(enemy);
            }    
        }

        private void AttackWithAnimation(Unit unit)
        {
            attackAnimation.SetAction(() => base.Attack(unit));
            attackAnimation.Ended.AddListener(OnAnimationEnd);
            var context = new AnimationContext()
            {
                TargetUnit = unit
            };
            attackAnimation.Execute(context);
            void OnAnimationEnd(UnitAnimation unitAnimation)
            {
                attackAnimation.Ended.RemoveListener(OnAnimationEnd);
                Player.LocalPlayer.RecalculateVisibility();
                Complete();
            }
        }

        private void InstantAttack(ITargetedAlive enemy)
        {
            base.Attack(enemy);
            Executor.transform.position = Executor.Node.transform.position;
            Player.LocalPlayer.RecalculateVisibility();
            Complete();
        }

        protected override MeleeAttackAction<Node, Edge, Unit> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit>(
                Executor,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }


        public override void Accept(IMonoUnitActionVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(IUnitActionVisitor<TResult, Node, Edge, Unit> visitor) =>
            visitor.Visit(this);
    }
}