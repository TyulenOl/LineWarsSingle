using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    public class DistanceUnit: Unit
    {
        [field: Header("ArtillerySettings")]
        [field: SerializeField, Min(0)] public int DistanceDamage { get; private set; }
        [field: SerializeField, Min(0)] public int Distance { get; private set; }
        [field: SerializeField] public bool IsPenetrating { get; private set; }
        [SerializeField] private IntModifier distanceAttackPointsModifier;


        public override bool CanAttack(Edge edge)
        {
            return !AttackLocked &&
                   edge.LineType >= LineType.CountryRoad
                   && Node.FindShortestPath(edge.FirstNode).Count - 1 <= Distance
                   && Node.FindShortestPath(edge.SecondNode).Count - 1 <= Distance;
        }

        public override void Attack(Edge edge)
        {
            DistanceAttack(edge);
        }

        public override bool CanAttack(Unit unit)
        {
            return !AttackLocked
                   && unit.Owner != Owner
                   && Node.FindShortestPath(unit.Node).Count - 1 <= Distance;
        }

        public override void Attack(Unit enemy)
        {
            if (enemy.TryGetNeighbour(out var neighbour)) 
                DistanceAttack(neighbour);
            DistanceAttack(enemy);
        }

        private void DistanceAttack(IAlive alive)
        {
            alive.TakeDamage(new Hit(DistanceDamage, this, alive, IsPenetrating));
            CurrentActionPoints = distanceAttackPointsModifier
                ? distanceAttackPointsModifier.Modify(CurrentActionPoints)
                : 0;
        }

        //ЭТО ДЛЯ ВИЗУАЛИЗАЦИИ, чтобы можно было подсвечивать
        public IEnumerable<Node> GetNodeInMyDistance()
        {
            return Graph.GetNodesInRange(Node, (uint)Distance);
        }
    }
}