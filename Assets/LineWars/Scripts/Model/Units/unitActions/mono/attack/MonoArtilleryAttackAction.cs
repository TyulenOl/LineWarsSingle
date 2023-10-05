using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoArtilleryAttackAction : MonoDistanceAttackAction,
        IArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer>
    {
        private ArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer> AttackAction
            => (ArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer>) ExecutorAction;
        
        [SerializeField] private Explosion explosionPrefab;

        public override void Attack(IAlive enemy)
        {
            if (enemy is not MonoBehaviour mono) return;
            var explosion = Instantiate(explosionPrefab);
            explosion.transform.position = mono.transform.position;
            SfxManager.Instance.Play(attackSfx);
            explosion.ExplosionEnded += () =>
            {
                AttackAction.Attack(enemy);
            };
        }

        protected override ExecutorAction GetAction()
        {
            return new ArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer>(Unit, this, MonoGraph.Instance);
        }
    }
}