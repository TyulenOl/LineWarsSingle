using System.Collections;
using System.Collections.Generic;
using System.Net;
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
                
                foreach(var owned in basePlayer.OwnedObjects)
                {
                    if(!(owned is Unit unit)) continue;
                    if(unit.CurrentHp == unit.MaxHp) continue;
                    if(unit == doctor) continue;
                    var pathToNode = Graph.FindShortestPath(doctor.Node, unit.Node, 
                       unit);
                    if(!pathToNode.Remove(unit.Node)) continue;
                    var healCost = 0 -
                                   doctor.HealPointModifier.Modify(0);
                    var moveCost = 0 -
                                   doctor.MovePointsModifier.Modify(0);
                    
                    if(moveCost * pathToNode.Count + healCost > doctor.CurrentActionPoints) continue;
                    list.Add(new HealAction(basePlayer, executor, unit, pathToNode, this));
                }
            }
        }

        public class HealAction : EnemyAction
        {
            private readonly Unit damagedUnit;
            private readonly Doctor doctor;
            private readonly List<Node> path;
            private readonly HealActionData data;
            
            public HealAction(EnemyAI enemy, IExecutor executor, Unit damagedUnit, List<Node> path, HealActionData data) 
                : base(enemy, executor)
            {
                if (!(executor is Doctor doctor))
                {
                    Debug.LogError("Executor is not a Doctor!");
                    return;
                }

                this.doctor = doctor;
                this.damagedUnit = damagedUnit;
                this.path = path;
                this.data = data;
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
                var hpPercent = (float)damagedUnit.CurrentHp / damagedUnit.MaxHp;
                var healCost = 0 -
                               doctor.HealPointModifier.Modify(0);
                var moveCost = doctor.CurrentActionPoints -
                               doctor.MovePointsModifier.Modify(0);
                var pointsLeft = doctor.CurrentActionPoints - (healCost + moveCost * path.Count);
                return data.BaseScore + hpPercent * data.DamagedUnitBonus + pointsLeft * data.BonusPerPoints;
                
            }
        }
    }

