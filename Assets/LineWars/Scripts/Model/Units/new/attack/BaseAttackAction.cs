using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public abstract class BaseAttackAction: UnitAction
    {
        [SerializeField] protected bool attackLocked;
        [SerializeField, Min(0)] protected int damage;
        [SerializeField] protected SFXData attackSfx;
        
        public int Damage => damage;
        
        public bool CanAttack(IAlive enemy) => CanAttackForm(MyUnit.Node, enemy);
        public bool CanAttackForm(Node node, IAlive enemy, bool ignoreActionPointsCondition = false)
        {
            return enemy switch
            {
                Edge edge => CanAttackForm(node, edge),
                CombinedUnit unit => CanAttackForm(node, unit),
                _ => false
            };
        }

        public void Attack(IAlive enemy)
        {
            switch (enemy)
            {
                case Edge edge:
                    Attack(edge);
                    break;
                case CombinedUnit unit:
                    Attack(unit);
                    break;
            }
        }
        

        public virtual bool CanAttackForm(Node node, CombinedUnit unit,  bool ignoreActionPointsCondition = false) => false;
        public virtual bool CanAttackForm(Node node, Edge unit,  bool ignoreActionPointsCondition = false) => false;
        
        public virtual void Attack(CombinedUnit unit) {}
        public virtual void Attack(Edge edge) {}
    }
}