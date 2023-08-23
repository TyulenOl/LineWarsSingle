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
            [Header("Further Settings")] 
            [SerializeField] private float furtherBaseScore;
            [SerializeField] private float furtherBonusPerHp;
            [SerializeField] private float furtherBonusPerAttack;
            [SerializeField] private float furtherBonusPerEnemyHp;
            [SerializeField] private float furtherBonusPerEnemyAttack;
            [SerializeField] private float furtherBonusPerPoints;

            [Header("Closer Settings")] 
            [SerializeField] private float closerBaseScore;
            [SerializeField] private float closerMaxBonusForHpDamage;
            [SerializeField] private float closerBonusPerPoints;

            #region Attributes
            public float FurtherBaseScore => furtherBaseScore;
            public float FurtherBonusPerHp => furtherBonusPerHp;
            public float FurtherBonusPerAttack => furtherBonusPerAttack;
            public float FurtherBonusPerEnemyHp => furtherBonusPerEnemyHp;
            public float FurtherBonusPerEnemyAttack => furtherBonusPerEnemyAttack;
            public float FurtherBonusPerPoints => furtherBonusPerPoints;
            public float CloserBaseScore => closerBaseScore;
            public float CloserMaxBonusForHpDamage => closerMaxBonusForHpDamage;
            public float CloserBonusPerPoints => closerBonusPerPoints;
            public float WaitTime => waitTime;
            #endregion

            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
            {
                if (!(executor is Unit unit)) return;

                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var possibleDistance = unit.CurrentActionPoints / moveCost + 1;
                var baseDistance = Graph.FindShortestPath(unit.Node, basePlayer.Base, 
                    unit).Count;
                var nearbyNodes = 
                    Graph.GetNodesInRange(unit.Node, (uint)possibleDistance, unit);
               
                foreach (var currentNode in nearbyNodes)   
                {
                    if(currentNode == unit.Node) continue;
                    if(currentNode.Owner != basePlayer) continue;
                    var currentNodeDistance = Graph.FindShortestPath(currentNode,
                        basePlayer.Base,
                        unit).Count;
                    list.Add(new MoveToOwnNodeAction(basePlayer, executor, currentNode, this,
                        currentNodeDistance > baseDistance));
                }
            }
        }

        public class MoveToOwnNodeAction : EnemyAction
        {
            private readonly Node targetNode;
            private readonly Unit unit;
            private readonly List<Node> path;
            private readonly MoveToOwnNodeActionData data;
            private readonly bool isFurther;
            public MoveToOwnNodeAction(EnemyAI enemy, IExecutor executor, 
                Node targetNode, MoveToOwnNodeActionData data, bool isFurther) : base(enemy, executor)
            {
                this.targetNode = targetNode;
                if (executor is not Unit unit)
                {
                    Debug.LogError("Executor is not a Unit");
                    return;
                }

                this.unit = unit;
                this.data = data;
                this.isFurther = isFurther;
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
                if (isFurther)
                   return GetFurtherScore();
                return GetCloserScore();
            }

            private float GetCloserScore()
            {
                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var pointsLeft = unit.CurrentActionPoints - moveCost * (path.Count - 1);
                var bonusPerHpDamage = (1 - (unit.CurrentHp / unit.MaxHp)) * data.CloserMaxBonusForHpDamage;
                var bonusPerPoints = data.CloserBonusPerPoints * pointsLeft;
                return data.CloserBaseScore + bonusPerPoints + bonusPerHpDamage;
            }

            private float GetFurtherScore()
            {
                var enemies = EnemyActionUtilities.FindAdjacentEnemies(targetNode, basePlayer);
                var enemiesHp = 0;
                var enemiesAttack = 0;
                foreach (var currentEnemy in enemies)
                {
                    enemiesHp += currentEnemy.CurrentHp;
                    enemiesAttack += currentEnemy.MeleeDamage;
                }

                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var pointsLeft = unit.CurrentActionPoints - moveCost * (path.Count - 1);
                var enemyBonus = 0f;
                if (enemies.Count > 0)
                {
                    enemyBonus = data.FurtherBonusPerHp * unit.CurrentHp +
                                 unit.MeleeDamage * data.FurtherBonusPerAttack +
                                 enemiesHp * data.FurtherBonusPerEnemyHp +
                                 enemiesAttack * data.FurtherBonusPerEnemyAttack;
                }
                return data.FurtherBaseScore + enemyBonus + pointsLeft * data.FurtherBonusPerPoints;
            }
        }
    }


