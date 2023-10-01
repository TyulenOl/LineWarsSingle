using JetBrains.Annotations;
using LineWars.Controllers;
using UnityEngine;

namespace LineWars.Model
{
    public class MonoArtilleryAttackAction : MonoDistanceAttackAction,
        IArtilleryAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>
    {
        [SerializeField] private Explosion explosionPrefab;
        protected override ExecutorAction GetAction()
        {
            return new DistanceAttackAction<Node, Edge, Unit, Owned, BasePlayer, Nation>(GetComponent<Unit>(), this, MonoGraph.Instance);
        }
    }
}