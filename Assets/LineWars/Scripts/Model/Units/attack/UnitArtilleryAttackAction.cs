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

        public override ComponentUnit.UnitAction GetAction(ComponentUnit unit) =>
            new ComponentUnit.ArtilleryAttackAction(unit, this);
    }

    public sealed partial class ComponentUnit
    {
        public class ArtilleryAttackAction : DistanceAttackAction
        {
            private readonly Explosion explosionPrefab;
            public ArtilleryAttackAction([NotNull] ComponentUnit unit, Model.UnitArtilleryAttackAction data) : base(unit, data)
            {
                explosionPrefab = data.ExplosionPrefab;
            }

            public override bool CanAttackForm(Node node, Edge edge, bool ignoreActionPointsCondition = false)
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
            

            public override void Attack(Edge edge)
            {
                var explosion = Instantiate(explosionPrefab);
                SfxManager.Instance.Play(ActionSfx);
                explosion.transform.position = edge.transform.position;
                explosion.ExplosionEnded += () => { DistanceAttack(edge, Damage); };
            }

            public override void Attack(ComponentUnit enemy)
            {
                var explosion = Instantiate(explosionPrefab);
                SfxManager.Instance.Play(ActionSfx);
                explosion.transform.position = enemy.Node.Position;
                explosion.ExplosionEnded += () =>
                {
                    var damage = Damage;
                    if (enemy.TryGetNeighbour(out var neighbour))
                    {
                        damage /= 2;
                        DistanceAttack(neighbour, damage);
                    }

                    DistanceAttack(enemy, damage);
                };
            }
            
            public override CommandType GetMyCommandType() => CommandType.Explosion;
        }
    }
}