using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace LineWars.Model
{
    [RequireComponent(typeof(MonoMoveAction))]
    public class MonoMeleeAttackAction : MonoAttackAction,
        IMeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [field: SerializeField] public UnitBlockerSelector InitialBlockerSelector { get; private set; }
        
        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [field: SerializeField] public bool InitialOnslaught { get; private set; }

        public bool Onslaught => ((MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>) Action).Onslaught;
        public UnitBlockerSelector BlockerSelector => ((MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>)Action).BlockerSelector;

        protected override AttackAction<Node, Edge, Unit, Owned, BasePlayer> GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>(
                Unit, 
                InitialDamage,
                InitialIsPenetratingDamage,
                InitialOnslaught,
                InitialBlockerSelector);
        }
        

        public override void Accept(IMonoUnitVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}