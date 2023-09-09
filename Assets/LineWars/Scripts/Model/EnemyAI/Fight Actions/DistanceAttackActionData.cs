using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Distance Attack Action", menuName = "EnemyAI/Enemy Actions/Artillery Phase/Distance Attack")]
    public class DistanceAttackActionData : EnemyActionData
    {
        [SerializeField] private IntModifier distanceAttackModifier;
        [SerializeField] private float waitTime;
        [SerializeField] private float baseScore;
        [SerializeField] private float bonusPerDistance;
        [SerializeField] private float bonusPerEnemyHpDamage;
        [SerializeField] private float bonusPerPoint;

        public float WaitTime => waitTime;
        public float BaseScore => baseScore;
        public float BonusPerDistance => bonusPerDistance;
        public float BonusPerEnemyHpDamage => bonusPerEnemyHpDamage;
        public float BonusPerPoint => bonusPerPoint;
        public override void AddAllPossibleActions(List<EnemyAction> actionList, EnemyAI basePlayer, IExecutor executor)
        {
            if (executor is not DistanceAttackUnit distanceUnit) return;
            var enemySet = new HashSet<Unit>();
            
            NodeParser(null, distanceUnit.Node, distanceUnit.CurrentActionPoints, 
                distanceUnit, enemySet, actionList, basePlayer);
            
            EnemyActionUtilities.GetNodesInIntModifierRange(distanceUnit.Node,
                distanceUnit.CurrentActionPoints,
                distanceUnit.MovePointsModifier,
                (prevNode, node, actionPoints) =>
                    NodeParser(prevNode, node, actionPoints, distanceUnit, enemySet, actionList, basePlayer),
                distanceUnit);
        }

        private void NodeParser(Node previousNode, Node node, int actionPoints, DistanceAttackUnit distanceUnit, 
            HashSet<Unit> enemySet, List<EnemyAction> list, EnemyAI basePlayer)
        {
            var enemies = CheckDistanceAttack(node, distanceUnit, enemySet);
            var pointsAfterAttack = distanceUnit.AttackPointsModifier.Modify(actionPoints);
            if(actionPoints > 0 && pointsAfterAttack >= 0)
                foreach (var enemy in enemies)
                {
                    list.Add(new DistanceAttackAction(basePlayer, distanceUnit, node, enemy, this));
                }
        }

        private List<Unit> CheckDistanceAttack(Node node, DistanceAttackUnit distanceUnit, HashSet<Unit> enemySet)
        {
            var list = new List<Unit>();
            var attackedNodes =
                EnemyActionUtilities.GetNodesInIntModifierRange(node,
                    distanceUnit.Distance,
                    distanceAttackModifier);
            foreach (var attackedNode in attackedNodes)
            {
                var unitsInNode = EnemyActionUtilities.GetUnitsInNode(attackedNode);
                foreach (var unit in unitsInNode)
                {
                    if(enemySet.Contains(unit)) continue;
                    enemySet.Add(unit);
                    if(unit.Owner == distanceUnit.Owner) continue;
                    if(distanceUnit.CanAttack(unit))
                        list.Add(unit);
                }
            }

            return list;
        }
    }
    
    public class DistanceAttackAction : EnemyAction
    {
        private readonly Node nodeToWalk;
        private readonly Unit target;
        private readonly DistanceAttackUnit distanceUnit;
        private readonly List<Node> path;
        private readonly DistanceAttackActionData data;
        public DistanceAttackAction(EnemyAI basePlayer, IExecutor executor, Node node, Unit targetUnit, DistanceAttackActionData data) : base(basePlayer, executor)
        {
            if (executor is not DistanceAttackUnit distanceUnit)
            {
                Debug.LogError($"{executor} is not a DistanceUnit!");
                return;
            }

            this.distanceUnit = distanceUnit;
            nodeToWalk = node;
            target = targetUnit;
            path = Graph.FindShortestPath(distanceUnit.Node, nodeToWalk, this.distanceUnit);
            path.Remove(distanceUnit.Node);
            score = GetScore();
            this.data = data;
        }

        public override void Execute()
        {
            basePlayer.StartCoroutine(ExecuteEnumerator());
            IEnumerator ExecuteEnumerator()
            {
                foreach (var node in path)
                {
                    if (node == distanceUnit.Node) continue;
                    if (!distanceUnit.CanMoveTo(node))
                    {
                        Debug.LogError($"{distanceUnit} cannot move to {node}");
                        yield break;
                    }
                    distanceUnit.MoveTo(node);
                    yield return new WaitForSeconds(data.WaitTime);
                }

                if (!distanceUnit.CanAttack(target))
                {
                    Debug.LogError($"{distanceUnit} cannot attack {target}");
                    yield break;
                }
                distanceUnit.Attack(target);
                InvokeActionCompleted();
            }
        }

        private float GetScore()
        {
            if (distanceUnit.Damage == 0) return 0f;

            var enemyDistance = Graph.FindShortestPath(target.Node, basePlayer.Base.Node, target).Count;
            if (enemyDistance == 0) enemyDistance = 1;
            var distanceBonus = data.BonusPerDistance / enemyDistance;
            var hpBonus = data.BonusPerEnemyHpDamage 
                          * (1 - target.CurrentHp / target.MaxHp);
            var pointsLeft = distanceUnit.CurrentActionPoints;
            foreach (var node in path)
            {
                if(node == target.Node) continue;
                if(node == distanceUnit.Node) continue;
                pointsLeft = distanceUnit.MovePointsModifier.Modify(pointsLeft);
            }

            pointsLeft = distanceUnit.AttackPointsModifier.Modify(pointsLeft);
            return data.BaseScore 
                   + distanceBonus 
                   + hpBonus 
                   + pointsLeft * data.BonusPerPoint;
        }
    }
    
    
}
