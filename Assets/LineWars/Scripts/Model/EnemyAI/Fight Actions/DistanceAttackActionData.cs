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
        public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
        {
            if (executor is not Artillery distanceUnit) return;

            var queue = new Queue<(Node, int)>();
            var nodeSet = new HashSet<Node>();
            var enemySet = new HashSet<Unit>();
            
            queue.Enqueue((distanceUnit.Node, distanceUnit.CurrentActionPoints));
            nodeSet.Add(distanceUnit.Node);
            while (queue.Count > 0)
            {
                var currentNodeInfo = queue.Dequeue();
                if(currentNodeInfo.Item2 == 0) continue;

                var enemies = CheckDistanceAttack(currentNodeInfo.Item1, distanceUnit, enemySet);
                var pointsAfterAttack = distanceUnit.AttackPointsModifier.Modify(currentNodeInfo.Item2);
                if(pointsAfterAttack >= 0)
                    foreach (var enemy in enemies)
                    {
                        list.Add(new DistanceAttackAction(basePlayer, executor, currentNodeInfo.Item1, enemy));
                    }
                
                foreach (var neighborNode in currentNodeInfo.Item1.GetNeighbors())
                {
                    if (nodeSet.Contains(neighborNode)) continue;

                    var pointsAfterMove = distanceUnit.MovePointsModifier.Modify(currentNodeInfo.Item2);

                    var edge = neighborNode.GetLine(currentNodeInfo.Item1);
                    if (pointsAfterMove >= 0 &&
                        distanceUnit.CanMoveOnLineWithType(edge.LineType)
                        && Graph.CheckNodeForWalkability(neighborNode, distanceUnit))
                    {
                        queue.Enqueue((currentNodeInfo.Item1, pointsAfterMove));
                        nodeSet.Add(currentNodeInfo.Item1);
                    }
                }
            }
        }

        private List<Unit> CheckDistanceAttack(Node node, Artillery distanceUnit, HashSet<Unit> enemySet)
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
        private readonly Artillery distanceUnit;
        private readonly List<Node> path;
        private readonly DistanceAttackActionData data;
        public DistanceAttackAction(EnemyAI basePlayer, IExecutor executor, Node node, Unit targetUnit) : base(basePlayer, executor)
        {
            if (executor is not Artillery distanceUnit)
            {
                Debug.LogError($"{executor} is not a DistanceUnit!");
                return;
            }

            this.distanceUnit = distanceUnit;
            nodeToWalk = node;
            target = targetUnit;
            path = Graph.FindShortestPath(distanceUnit.Node, nodeToWalk, this.distanceUnit);
            score = GetScore();
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
