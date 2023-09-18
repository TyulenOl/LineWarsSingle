using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Heal Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Heal")]
    public class EnemyAIHealActionData : EnemyActionData
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
            if (!(executor is Doctor doctor)) return;
            var unitSet = new HashSet<Unity.VisualScripting.Unit>();
            EnemyActionUtilities.GetNodesInIntModifierRange(doctor.Node, doctor.CurrentActionPoints,
                doctor.MovePointsModifier,
                (prevNode, node, actionPoints) =>
                    NodeParser(prevNode, node, actionPoints, unitSet, doctor, basePlayer, list),
                doctor);
        }

        private void NodeParser(Node previousNode, Node node, int actionPoints,
            HashSet<Unity.VisualScripting.Unit> unitSet, Doctor doctor, EnemyAI basePlayer,
            List<EnemyAction> actionList)
        {
            if (actionPoints <= 0) return;
            var pointsAfterHeal = doctor.HealPointModifier.Modify(actionPoints);
            if (pointsAfterHeal < 0) return;

            var allies = EnemyActionUtilities.FindAdjacentAllies(node, basePlayer, LineType.Firing);
            foreach (var ally in allies)
            {
                if (unitSet.Contains(ally)) continue;
                if (ally == doctor) continue;
                actionList.Add(new HealAction(basePlayer, doctor, ally, this));
                unitSet.Add(ally);
            }
        }
    }

    public class HealAction : EnemyAction
    {
        private readonly Unity.VisualScripting.Unit damagedUnit;
        private readonly Doctor doctor;
        private readonly EnemyAIHealActionData data;
        private readonly List<Node> path;

        public HealAction(EnemyAI enemy, IExecutor executor, Unity.VisualScripting.Unit damagedUnit,
            EnemyAIHealActionData data)
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
            path.Remove(doctor.Node);
            score = GetScore();
            //Debug.Log($"DAMAGED - {damagedUnit}");
        }

        public override void Execute()
        {
            basePlayer.StartCoroutine(ExecuteCoroutine());

            IEnumerator ExecuteCoroutine()
            {
                foreach (var node in path)
                {
                    if (node == damagedUnit.Node) continue;
                    if (node == doctor.Node) continue;
                    if (!doctor.CanMoveTo(node))
                        Debug.LogError($"{doctor} cannot go to {node}");

                    UnitsController.ExecuteCommand(new UnitMoveCommand(doctor, doctor.Node, node));
                    yield return new WaitForSeconds(data.WaitTime);
                }

                UnitsController.ExecuteCommand(new UnitHealCommand(doctor, damagedUnit));
                InvokeActionCompleted();
            }
        }

        private float GetScore()
        {
            var hpDamagePercent = 1 - (float) damagedUnit.CurrentHp / damagedUnit.MaxHp;
            Debug.Log($"{damagedUnit.CurrentHp} / {damagedUnit.MaxHp} {damagedUnit}");
            if (hpDamagePercent == 0) return 0;
            var pointsLeft = doctor.CurrentActionPoints;
            foreach (var node in path)
            {
                if (node == damagedUnit.Node) continue;
                if (node == doctor.Node) continue;
                pointsLeft = doctor.MovePointsModifier.Modify(pointsLeft);
            }

            Debug.Log($"{data.BaseScore} {hpDamagePercent * data.DamagedUnitBonus} " +
                      $"{pointsLeft * data.BonusPerPoints}");
            pointsLeft = doctor.HealPointModifier.Modify(pointsLeft);
            return data.BaseScore
                   + hpDamagePercent * data.DamagedUnitBonus
                   + pointsLeft * data.BonusPerPoints;
        }
    }
}