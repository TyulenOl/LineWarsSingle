using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Move To Own Node", 
            menuName = "EnemyAI/Enemy Actions/Fight Phase/Move To Own Node")]
        public class MoveToOwnNodeActionData : EnemyActionData
        {
            [SerializeField] private float waitTime;
            [SerializeField] private float baseScore;
            [SerializeField] private float bonusPerUnitHp;
            [SerializeField] private float penaltyForEnemiesAttack;
            [SerializeField] private float bonusPerDistance;
            [SerializeField] private float bonusPerPoint;

            public float BaseScore => baseScore;
            public float WaitTime => waitTime;
            public float BonusPerUnitHp => bonusPerUnitHp;
            public float PenaltyForEnemiesAttack => penaltyForEnemiesAttack;
            public float BonusPerDistance => bonusPerDistance;
            public float BonusPerPoint => bonusPerPoint;

            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
            {
                if (!(executor is Unit unit)) return;

                var queue = new Queue<(Node, int)>();
                var nodeSet = new HashSet<Node>();
                queue.Enqueue((unit.Node, unit.CurrentActionPoints));
                nodeSet.Add(unit.Node);
                while (queue.Count > 0)
                {
                    var currentNodeInfo = queue.Dequeue();
                    
                    if(currentNodeInfo.Item2 == 0) continue;
                    foreach(var neighborNode in currentNodeInfo.Item1.GetNeighbors())
                    {
                        if(nodeSet.Contains(neighborNode)) continue;
                        
                        var pointsAfterMove = unit.MovePointsModifier.Modify(currentNodeInfo.Item2);
                        var edge = neighborNode.GetLine(currentNodeInfo.Item1);
                        
                        if (pointsAfterMove >= 0 
                            && (int) edge.LineType >= (int) unit.Passability 
                            && Graph.CheckNodeForWalkability(neighborNode, unit))
                        {
                            queue.Enqueue((neighborNode, pointsAfterMove));
                            nodeSet.Add(neighborNode);
                            if(neighborNode.Owner == basePlayer)
                                list.Add(new MoveToOwnNodeAction(basePlayer, executor, neighborNode, this));
                        }
                    }
                }
                
            }
        }

        public class MoveToOwnNodeAction : EnemyAction
        {
            private readonly Node targetNode;
            private readonly Unit unit;
            private readonly List<Node> path;
            private readonly MoveToOwnNodeActionData data;
            
            public MoveToOwnNodeAction(EnemyAI enemy, IExecutor executor, 
                Node targetNode, MoveToOwnNodeActionData data) : base(enemy, executor)
            {
                this.targetNode = targetNode;
                if (executor is not Unit unit)
                {
                    Debug.LogError("Executor is not a Unit");
                    return;
                }

                this.unit = unit;
                this.data = data;
                path = Graph.FindShortestPath(unit.Node, targetNode, unit);
                score = GetScore();
            }

            public override void Execute()
            {
                basePlayer.StartCoroutine(ExecuteCoroutine());
                IEnumerator ExecuteCoroutine()
                {
                    foreach (var node in path)
                    {
                        if(node == unit.Node) continue;
                        if(!unit.CanMoveTo(node))
                            Debug.LogError($"Unit cannot move to {node}");
                        
                        UnitsController.ExecuteCommand(new MoveCommand(unit, unit.Node, node));
                        yield return new WaitForSeconds(data.WaitTime);
                    }
                    InvokeActionCompleted();
                }
            }

            protected float GetScore()
            {
                var enemies = EnemyActionUtilities.FindAdjacentEnemies(unit.Node, basePlayer);
                var enemiesAttack = 0;
                foreach (var enemy in enemies)
                {
                    enemiesAttack += enemy.MeleeDamage;
                }
                var safetyBonus = data.BonusPerUnitHp * unit.CurrentHp 
                                  - data.PenaltyForEnemiesAttack * enemiesAttack;
                var oldNodeDistance = Graph.FindShortestPath(unit.Node, basePlayer.Base).Count;
                var newNodeDistance = Graph.FindShortestPath(targetNode, basePlayer.Base).Count;
                var pointsLeft = unit.CurrentActionPoints;
                foreach (var node in path)
                {
                    if(node == unit.Node) continue;
                    pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
                }
                return data.BaseScore 
                       + safetyBonus 
                       + data.BonusPerDistance * (newNodeDistance - oldNodeDistance)
                       + data.BonusPerPoint * pointsLeft;
            }
        }
    }


