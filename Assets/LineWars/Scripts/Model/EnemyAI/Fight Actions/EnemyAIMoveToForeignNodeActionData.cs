using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    [CreateAssetMenu(fileName = "New Move To Foreign Node Action", menuName = "EnemyAI/Enemy Actions/Fight Phase/Move To Foreign Node")]
    public class EnemyAIMoveToForeignNodeActionData : EnemyActionData
    {
        [SerializeField] private float waitTime;

        [Header("White Node Settings")] [SerializeField]
        private float whiteBaseScore;

        [SerializeField] private float whiteScorePerPoint;

        [Header("Foreign Node Settings")] [SerializeField]
        private float foreignBaseScore;

        [SerializeField] private float foreignScorePerPoint;

        public float WhiteBaseScore => whiteBaseScore;
        public float WhiteScorePerPoint => whiteScorePerPoint;
        public float ForeignBaseScore => foreignBaseScore;
        public float ForeignScorePerPoint => foreignScorePerPoint;
        public float WaitTime => waitTime;

        public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
        {
            if (!(executor is ComponentUnit unit)) return;

            EnemyActionUtilities.GetNodesInIntModifierRange(unit.Node, unit.CurrentActionPoints,
                unit.MovePointsModifier,
                (prevNode, node, actionPoints) => NodeParser(prevNode, node, actionPoints, basePlayer, unit, list),
                unit);
        }

        private void NodeParser(Node _, Node node, int actionPoints,
            EnemyAI basePlayer, ComponentUnit unit, List<EnemyAction> actionList)
        {
            if (actionPoints < 0) return;
            if (node == unit.Node) return;
            if (node.Owner != basePlayer)
                actionList.Add(new MoveToForeignNodeAction(basePlayer, unit, node, this));
        }
    }

    public class MoveToForeignNodeAction : EnemyAction
    {
        private readonly List<Node> path;
        private readonly ComponentUnit unit;
        private readonly Node targetNode;
        private readonly EnemyAIMoveToForeignNodeActionData data;

        public MoveToForeignNodeAction(EnemyAI basePlayer, IExecutor executor,
            Node targetNode, EnemyAIMoveToForeignNodeActionData data)
            : base(basePlayer, executor)
        {
            if (executor is not ComponentUnit unit)
            {
                Debug.LogError($"{executor} is not a Unit!");
                return;
            }

            this.unit = unit;
            this.targetNode = targetNode;
            this.data = data;
            path = Graph.FindShortestPath(unit.Node, targetNode, unit);
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
                    if (node == unit.Node) continue;
                    if (!unit.CanMoveTo(node))
                        Debug.LogError($"{unit} cannot move to {node}");

                    UnitsController.ExecuteCommand(new UnitMoveCommand(unit, unit.Node, node));
                    yield return new WaitForSeconds(data.WaitTime);
                }

                InvokeActionCompleted();
            }
        }

        private float GetScore()
        {
            if (targetNode.Owner == null)
                return WhiteGetScore();
            return ForeignGetScore();
        }

        private float ForeignGetScore()
        {
            var pointsLeft = unit.CurrentActionPoints;

            foreach (var node in path)
            {
                if (node == unit.Node) continue;
                pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
            }

            return data.ForeignBaseScore
                   + pointsLeft * data.ForeignScorePerPoint;
        }

        private float WhiteGetScore()
        {
            var pointsLeft = unit.CurrentActionPoints;

            foreach (var node in path)
            {
                if (node == unit.Node) continue;
                pointsLeft = unit.MovePointsModifier.Modify(pointsLeft);
            }

            return data.WhiteBaseScore
                   + pointsLeft * data.WhiteScorePerPoint;
        }
    }
}