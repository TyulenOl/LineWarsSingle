using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoMoveAction))]
    public class MonoMeleeAttackAction :
        MonoAttackAction<MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>>,
        IMeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [field: SerializeField] public UnitBlockerSelector InitialBlockerSelector { get; private set; }

        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [field: SerializeField]
        public bool InitialOnslaught { get; private set; }

        [SerializeField] private SimpleEffect slashEffectPrefab;

        public bool Onslaught => Action.Onslaught;
        public UnitBlockerSelector BlockerSelector => Action.BlockerSelector;

        public override void Attack(IAlive enemy)
        {
            if (enemy is Unit unit)
            {
                if (slashEffectPrefab == null)
                {
                    Debug.LogWarning($"Slash effect is nul on {name}");
                }
                else
                {
                    var helper = unit.GetComponent<UnitAnimationHelper>();
                    var slash = Instantiate(slashEffectPrefab);
                    slash.transform.position = unit.UnitDirection == UnitDirection.Left
                        ? helper.LeftCenter.transform.position
                        : helper.RightCenter.transform.position;
                }
            }

            base.Attack(enemy);
        }

        protected override MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>(
                Unit,
                InitialDamage,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }


        public override void Accept(IMonoUnitVisitor visitor) => visitor.Visit(this);

        public override TResult Accept<TResult>(
            IIUnitActionVisitor<TResult, Node, Edge, Unit, Owned, BasePlayer> visitor) => visitor.Visit(this);
    }
}