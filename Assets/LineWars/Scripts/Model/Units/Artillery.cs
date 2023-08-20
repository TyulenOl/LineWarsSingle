using UnityEngine;

namespace LineWars.Model
{
    public class Artillery: Unit
    {
        [field: Header("ArtillerySettings")]
        [field: SerializeField] public int DistanceDamage { get; private set; }

        public override bool CanAttack(Edge edge)
        {
            return !AttackLocked &&
                   edge.LineType >= LineType.CountryRoad;
        }

        public override void Attack(Edge edge)
        {
            
        }
    }
}