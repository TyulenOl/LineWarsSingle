using UnityEngine;

namespace LineWars.Model
{
    public class MonoMeleeAttackAction : MonoAttackAction,
        IMeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        [field: SerializeField] public UnitBlockerSelector InitialBlockerSelector { get; private set; }
        
        /// <summary>
        /// указывет на то, нужно ли захватывать точку после атаки
        /// </summary>
        [field: SerializeField] public bool InitialOnslaught { get; private set; }

        protected override ExecutorAction GetAction()
        {
            return new MeleeAttackAction<Node, Edge, Unit, Owned, BasePlayer>(GetComponent<Unit>(), this);
        }
    }
}