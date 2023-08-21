using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    public partial class EnemyAI
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
            

            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI enemy, IExecutor executor)
            {
                if (!(executor is Unit unit)) return;

                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var possibleDistance = unit.CurrentActionPoints / moveCost;
                var baseDistance = Graph.FindShortestPath(unit.Node, enemy.Base, unit.Passability).Count;
                var nearbyNodes = Graph.GetNodesInRange(unit.Node, (uint)possibleDistance, unit.Passability);
                foreach (var currentNode in nearbyNodes)   
                {
                    if(currentNode == unit.Node) continue;
                    if(currentNode.Owner != enemy) continue;

                    var currentNodeDistance = Graph.FindShortestPath(currentNode,
                        enemy.Base,
                        unit.Passability).Count;
                    list.Add(new MoveToOwnNodeAction(enemy, executor, currentNode, this,
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
                if (executor is not Unit unit1)
                {
                    Debug.LogError("Executor is not a Unit");
                    return;
                }

                unit = unit1;
                this.data = data;
                this.isFurther = isFurther;
                path = Graph.FindShortestPath(unit.Node, targetNode, unit.Passability);
            }

            public override void Execute()
            {
                enemy.StartCoroutine(ExecuteCoroutine());
                IEnumerator ExecuteCoroutine()
                {
                    foreach (var node in path)
                    {
                        if(!unit.CanMoveTo(node))
                            Debug.LogError($"Unit cannot move to {node}");
                        UnitsController.ExecuteCommand(new MoveCommand(unit, unit.Node, node));
                        yield return new WaitForSeconds(data.WaitTime);
                    }
                }
            }

            protected override float GetScore()
            {
                if (isFurther)
                   return GetFurtherScore();
                return GetCloserScore();
            }

            private float GetCloserScore()
            {
                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var pointsLeft = unit.CurrentActionPoints - moveCost * path.Count;
                var bonusPerHpDamage = (1 - (unit.CurrentHp / unit.MaxHp)) * data.CloserMaxBonusForHpDamage;
                var bonusPerPoints = data.CloserBonusPerPoints * pointsLeft;
                return data.CloserBaseScore + bonusPerPoints + bonusPerHpDamage;
            }

            private float GetFurtherScore()
            {
                var enemies = EnemyActionUtilities.FindAdjacentEnemies(unit.Node, enemy);
                var enemiesHp = 0;
                var enemiesAttack = 0;
                foreach (var enemy in enemies)
                {
                    enemiesHp += enemy.CurrentHp;
                    enemiesAttack += enemy.MeleeDamage;
                }

                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var pointsLeft = unit.CurrentActionPoints - moveCost * path.Count;
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
}

