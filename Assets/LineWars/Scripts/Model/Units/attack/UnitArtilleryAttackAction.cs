using System;
using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    //[CreateAssetMenu(fileName = "New ArtilleryAttack", menuName = "UnitActions/Attack/ArtilleryAttack", order = 61)]
    public class UnitArtilleryAttackAction : DistanceUnitAttackAction
    {
        [SerializeField] private Explosion explosionPrefab;

        public Explosion ExplosionPrefab => explosionPrefab;

        public override ModelComponentUnit.UnitAction GetAction(ModelComponentUnit unit)
        {
            var action = new ModelComponentUnit.ArtilleryAttackAction(unit, this);
            action.ActionStarted += (alive) =>
            {
                if (alive is not IHavePosition havePosition) return;
                var explosion = Instantiate(explosionPrefab);
                explosion.transform.position = havePosition.Position;
                explosion.ExplosionEnded += () => action.ExecuteAttack();
            };
            return action;
        }
    }

    public sealed partial class ModelComponentUnit
    {
        public class ArtilleryAttackAction : DistanceAttackAction
        {
            public Action<IAlive> ActionStarted;
            private Action attackAction;
            public ArtilleryAttackAction([NotNull] ModelComponentUnit unit, UnitArtilleryAttackAction data) : base(unit, data)
            {
                
            }

            public override bool CanAttackForm(ModelNode node, ModelEdge edge, bool ignoreActionPointsCondition = false)
            {
                var pathLen1 = node.FindShortestPath(edge.FirstNode).Count - 1;
                var pathLen2 = node.FindShortestPath(edge.SecondNode).Count - 1;
                return !AttackLocked
                       && Damage > 0
                       && edge.LineType >= LineType.CountryRoad
                       && (pathLen1 <= Distance && pathLen2 + 1 <= Distance
                           || pathLen2 <= Distance && pathLen1 + 1 <= Distance)
                       && (ignoreActionPointsCondition || ActionPointsCondition());
            }


            public override void Attack(ModelEdge edge)
            {
                ActionStarted?.Invoke(edge);
                attackAction = () => _Attack(edge);
            }

            public void AttackNotDelay(ModelEdge edge)
            {
                _Attack();
            }

            private void _Attack(ModelEdge edge)
            {
                DistanceAttack(edge, Damage); 
                CompleteAndAutoModify();
            }

            public override void Attack(ModelComponentUnit enemy)
            {
                ActionStarted?.Invoke(enemy);
                attackAction = () => _Attack(enemy);
            }

            private void _Attack(ModelComponentUnit enemy)
            {
                var damage = Damage;
                if (enemy.TryGetNeighbour(out var neighbour))
                {
                    damage /= 2;
                    DistanceAttack(neighbour, damage);
                }

                DistanceAttack(enemy, damage);
                CompleteAndAutoModify();
            }

            public void ExecuteAttack()
            {
                attackAction?.Invoke();
                attackAction = null;
            }
            public override CommandType GetMyCommandType() => CommandType.Explosion;
        }
    }
}