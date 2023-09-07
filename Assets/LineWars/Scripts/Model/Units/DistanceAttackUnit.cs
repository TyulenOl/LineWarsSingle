using LineWars.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace LineWars.Model
{
    public class DistanceAttackUnit : Unit
    {
        [field: Header("Distance Attack Settings")]
        [field: SerializeField, Min(0)] public int Distance { get; private set; }

        [Header("Sound Settings")] 
        [SerializeField] private SFXData distanceAttackSFX;

        public override bool CanAttack(Unit unit) => CanAttack(unit, Node);

        public override bool CanAttack(Unit unit, Node node)
        {
            return !attackLocked
                   && Damage > 0
                   && unit.Owner != Owner
                   && node.FindShortestPath(unit.Node).Count - 1 <= Distance
                   && ActionPointsCondition(attackPointsModifier, CurrentActionPoints);
        }

        public override void Attack(Unit enemy)
        {
            DistanceAttack(enemy);
        }

        protected void DistanceAttack(IAlive alive)
        {
            alive.TakeDamage(new Hit(Damage, this, alive, isPenetratingDamage, true));
            CurrentActionPoints = attackPointsModifier.Modify(CurrentActionPoints);
        
            ActionCompleted.Invoke();
            SfxManager.Instance.Play(distanceAttackSFX);
        }

        public override CommandType GetAttackTypeBy(IAlive target)
        {
            return CommandType.Fire;
        }
    }
}