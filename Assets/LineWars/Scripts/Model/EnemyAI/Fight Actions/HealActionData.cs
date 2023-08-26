using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
   
        [CreateAssetMenu(fileName = "New Heal Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Heal")]
        public class HealActionData : EnemyActionData
        {
            [SerializeField] private float waitTime;
            [SerializeField] private float baseScore;
            [SerializeField] private float damagedUnitBonus;
            [SerializeField] private float bonusPerPoints;

            public float BaseScore => baseScore;
            public float DamagedUnitBonus => damagedUnitBonus;
            public float BonusPerPoints => bonusPerPoints;
            public float WaitTime => waitTime;
            
            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
            {
                if(!(executor is Doctor doctor)) return;

                var queue = new Queue<(Node, int)>();
                var unitSet = new HashSet<Unit>();
                var nodeSet = new HashSet<Node>();

                nodeSet.Add(doctor.Node);
                queue.Enqueue((doctor.Node, doctor.CurrentActionPoints));
                while (queue.Count > 0)
                {
                    var currentNodeInfo = queue.Dequeue();
                    if(currentNodeInfo.Item2 == 0) continue;
                    foreach (var neighbor in currentNodeInfo.Item1.GetNeighbors())
                    {
                        if(nodeSet.Contains(neighbor)) continue;
                        var pointsAfterMove = doctor.MovePointsModifier.Modify(currentNodeInfo.Item2);
                        var pointsAfterHeal = doctor.HealPointModifier.Modify(currentNodeInfo.Item2);

                        var edge = neighbor.GetLine(currentNodeInfo.Item1);
                        if (pointsAfterHeal >= 0
                            && (int) edge.LineType >= (int) LineType.Firing)
                        {
                            foreach (var unit in EnemyActionUtilities.GetUnitsInNode(neighbor))
                            {
                                if(unit.Owner != basePlayer) continue;
                                if(unitSet.Contains(unit)) continue;
                                list.Add(new HealAction(basePlayer, executor, unit, this));
                                unitSet.Add(unit);
                            }
                        }

                        if (pointsAfterMove >= 0
                            && doctor.CanMoveOnLineWithType(edge.LineType)
                            && Graph.CheckNodeForWalkability(neighbor, doctor))
                        {
                            queue.Enqueue((neighbor, pointsAfterMove));
                            nodeSet.Add(neighbor);
                        }
                    }
                }
            }
        }

        public class HealAction : EnemyAction
        {
            private readonly Unit damagedUnit;
            private readonly Doctor doctor;
            private readonly HealActionData data;
            private readonly List<Node> path;
            
            public HealAction(EnemyAI enemy, IExecutor executor, Unit damagedUnit, HealActionData data) 
                : base(enemy, executor)
            {
                if (!(executor is Doctor doctor))
                {
                    Debug.LogError("Executor is not a Doctor!");
                    return;
                }

                this.doctor = doctor;
                this.damagedUnit = damagedUnit;
                this.data = data;
                path = Graph.FindShortestPath(doctor.Node, damagedUnit.Node, doctor);
                score = GetScore();
            }

            public override void Execute()
            {
                basePlayer.StartCoroutine(ExecuteCoroutine());
                IEnumerator ExecuteCoroutine()
                {
                    foreach (var node in path)
                    {
                        if(node == damagedUnit.Node) continue;
                        if (node == doctor.Node) continue;
                        if(!doctor.CanMoveTo(node))
                            Debug.LogError($"{doctor} cannot go to {node}");
                        
                        UnitsController.ExecuteCommand(new MoveCommand(doctor, doctor.Node, node));
                        yield return new WaitForSeconds(data.WaitTime);
                    }
                    
                    UnitsController.ExecuteCommand(new HealCommand(doctor, damagedUnit));
                    InvokeActionCompleted();
                }
            }

            private float GetScore()
            {
                var hpDamagePercent = 1 - (float)damagedUnit.CurrentHp / damagedUnit.MaxHp;
                if (hpDamagePercent == 0) return 0;
                var pointsLeft = doctor.CurrentActionPoints;
                foreach (var node in path)
                {
                    if(node == damagedUnit.Node) continue;
                    if (node == doctor.Node) continue;
                    pointsLeft = doctor.MovePointsModifier.Modify(pointsLeft);
                }

                pointsLeft = doctor.HealPointModifier.Modify(pointsLeft);
                return data.BaseScore 
                       + hpDamagePercent * data.DamagedUnitBonus 
                       + pointsLeft * data.BonusPerPoints;
                
            }
        }
    }

