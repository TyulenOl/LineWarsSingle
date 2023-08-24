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
            [SerializeField] private float scoreForDistance;
            [SerializeField] private float scoreForFightSuperiority;
            [SerializeField] private float scorePerPoints;
            [SerializeField] private float defaultAttacksToKillUnit;

            public float BaseScore => baseScore;
            public float ScoreForDistance => scoreForDistance;
            public float ScoreForFightSuperiority => scoreForFightSuperiority;
            public float ScorePerPoints => scorePerPoints;
            public float WaitTime => waitTime;
            public float DefaultAttackToKillUnit => defaultAttacksToKillUnit;

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
                    Debug.Log($"{currentNodeInfo.Item1} {currentNodeInfo.Item2}");
                    foreach(var neighborNode in currentNodeInfo.Item1.GetNeighbors())
                    {
                        if(nodeSet.Contains(neighborNode)) continue;
                        var pointsAfterAttack = unit.AttackPointsModifier.Modify(currentNodeInfo.Item2);
                        var pointsAfterMove = unit.MovePointsModifier.Modify(currentNodeInfo.Item2);
                        var edge = neighborNode.GetLine(currentNodeInfo.Item1);
                        
                        if (currentNodeInfo.Item2 != 0 
                            && pointsAfterAttack >= 0 
                            && (int) edge.LineType >= (int) LineType.Firing)
                        {
                            var enemies = GetEnemiesInNode(neighborNode, basePlayer);
                            foreach (var enemy in enemies) 
                            {
                                if(enemySet.Contains(enemy)) continue;
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

            private List<Unit> GetEnemiesInNode(Node node, EnemyAI basePlayer)
            {
                var enemies = new List<Unit>();
                if (node.LeftUnit != null && node.LeftUnit.Owner != basePlayer)
                {
                    enemies.Add(node.LeftUnit);
                }
                if (node.RightUnit != null && node.RightUnit.Owner != basePlayer)
                {
                    enemies.Add(node.RightUnit);
                }

                return enemies;
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
                    Debug.Log(path.Count);
                    foreach (var node in path) 
                    {
                        if(node == target.Node) continue;
                        if(node == unit.Node) continue;
                        if (!unit.CanMoveTo(node))
                        {
                            Debug.LogError($"{unit} cannot move to {node}");
                        }
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
                var distanceToBase = 
                    Graph.FindShortestPath(target.Node, basePlayer.Base, unit).Count;
                var attacksToKillTarget = Mathf.Ceil((float) target.CurrentHp / unit.MeleeDamage);

                float attacksToKillUnit;
                if (target.MeleeDamage == 0)
                    attacksToKillUnit = data.DefaultAttackToKillUnit;
                else
                    attacksToKillUnit = Mathf.Ceil((float) unit.CurrentHp / target.MeleeDamage);

                var attackPoint = 0 - unit.AttackPointsModifier.Modify(0);
                var movePoint = 0 - unit.MovePointsModifier.Modify(0);
                var pointsLeft = unit.CurrentActionPoints - attackPoint - movePoint * path.Count;
                float distanceScore = 0f;
                if (distanceToBase != 0)
                    distanceScore = data.ScoreForDistance / distanceToBase;
                return data.BaseScore
                       + (distanceScore)
                       + (data.ScoreForFightSuperiority * (attacksToKillTarget - attacksToKillUnit))
                       + data.ScorePerPoints * pointsLeft;
            }
        }
    }

