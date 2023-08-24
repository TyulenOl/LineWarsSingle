using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    
        [CreateAssetMenu(fileName = "New Attack Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Attack")]
        public class AttackActionData : EnemyActionData
        {
            [SerializeField] private float waitTime;
            [SerializeField] private float baseScore;
            [SerializeField] private float bonusPerDistance;
            [SerializeField] private float bonusPerEnemyHpDamage;
            [SerializeField] private float bonusPerPoint;

            public float BaseScore => baseScore;
            public float WaitTime => waitTime;
            public float BonusPerDistance => bonusPerDistance;
            public float BonusPerEnemyHpDamage => bonusPerEnemyHpDamage;
            public float BonusPerPoint => bonusPerPoint;

            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
            {
                if(executor is not Unit unit) return;

                var queue = new Queue<(Node, int)>();
                var enemySet = new HashSet<Unit>();
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
                        var pointsAfterAttack = unit.AttackPointsModifier.Modify(currentNodeInfo.Item2);
                        var pointsAfterMove = unit.MovePointsModifier.Modify(currentNodeInfo.Item2);
                        var edge = neighborNode.GetLine(currentNodeInfo.Item1);
                        
                        if (pointsAfterAttack >= 0 
                            && (int) edge.LineType >= (int) LineType.Firing)
                        {
                            var units = EnemyActionUtilities.GetUnitsInNode(neighborNode);
                            foreach (var enemy in units) 
                            {
                                if(enemy.Owner == basePlayer) continue;
                                if(enemySet.Contains(enemy)) continue;
                                if(unit.CanAttack(enemy))
                                    list.Add(new AttackAction(basePlayer, executor, enemy, this));
                                enemySet.Add(enemy);
                            }
                        }
                        
                        if (pointsAfterMove >= 0 
                            && (int) edge.LineType >= (int) unit.MovementLineType 
                            && Graph.CheckNodeForWalkability(neighborNode, unit))
                        {
                            queue.Enqueue((neighborNode, pointsAfterMove));
                            nodeSet.Add(neighborNode);
                        }
                    }
                }
            }
        }

        public class AttackAction : EnemyAction
        {
            private readonly Unit target;
            private readonly Unit unit;
            private readonly AttackActionData data;
            private readonly List<Node> path;
            public AttackAction([NotNull] EnemyAI basePlayer, [NotNull] IExecutor executor, [NotNull] Unit target,
                [NotNull] AttackActionData data) 
                : base(basePlayer, executor)
            {
                if (basePlayer == null) throw new ArgumentNullException(nameof(basePlayer));
                if (executor == null) throw new ArgumentNullException(nameof(executor));
                if (target == null) throw new ArgumentNullException(nameof(target));
                if (data == null) throw new ArgumentNullException(nameof(data));
                if (executor is not Unit unit)
                {
                    Debug.LogError("Executor is not a Unit");
                    return;
                }

                this.unit = unit;
                this.target = target;
                this.data = data;
                
                path = Graph.FindShortestPath(unit.Node, target.Node, unit);
                path.Remove(unit.Node);
                score = GetScore();
            }

            public override void Execute()
            {
                basePlayer.StartCoroutine(ExecuteCoroutine());
                IEnumerator ExecuteCoroutine()
                {
                    foreach (var node in path) 
                    {
                        if(node == target.Node) continue;
                        if(node == unit.Node) continue;
                        if (!unit.CanMoveTo(node))
                            Debug.LogError($"{unit} cannot move to {node}");
                        
                        UnitsController.ExecuteCommand(new MoveCommand(unit, unit.Node, node));
                        yield return new WaitForSeconds(data.WaitTime);
                    }
                    if(!unit.CanAttack(target))
                        Debug.LogError($"{unit} cannot attack {target}");
                    
                    UnitsController.ExecuteCommand(new AttackCommand(unit, target));
                    InvokeActionCompleted();
                }
            }

            private float GetScore()
            {
                if (unit.MeleeDamage == 0) return 0f;

                var enemyDistance = Graph.FindShortestPath(target.Node, basePlayer.Base, target).Count;
                if (enemyDistance == 0) enemyDistance = 1;
                var distanceBonus = data.BonusPerDistance / enemyDistance;
                var hpBonus = data.BonusPerEnemyHpDamage 
                              * (1 - target.CurrentHp / target.MaxHp);
                var pointsLeft = unit.CurrentActionPoints;
                foreach (var node in path)
                {
                    if(node == target.Node) continue;
                    if(node == unit.Node) continue;
                    pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
                }

                pointsLeft = unit.AttackPointsModifier.Modify(pointsLeft);
                return data.BaseScore 
                       + distanceBonus 
                       + hpBonus 
                       + pointsLeft * data.BonusPerPoint;

            }
        }
    }

