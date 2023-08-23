using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace LineWars.Model
{
    
        [CreateAssetMenu(fileName = "New Move To Foreign Node Action", 
            menuName = "EnemyAI/Enemy Actions/Fight Phase/Move To Foreign Node")]
        public class MoveToForeignNodeActionData : EnemyActionData
        {
            [SerializeField] private float waitTime;
            [Header("White Node Settings")]
            [SerializeField] private float whiteBaseScore;
            [SerializeField] private float whiteScorePerPoint;

            [Header("Foreign Node Settings")] 
            [SerializeField] private float foreignBaseScore;
            [SerializeField] private float foreignScorePerPoint;
            
            public float WhiteBaseScore => whiteBaseScore;
            public float WhiteScorePerPoint => whiteScorePerPoint;
            public float ForeignBaseScore => foreignBaseScore;
            public float ForeignScorePerPoint => foreignScorePerPoint;
            public float WaitTime => waitTime;
            
            public override void AddAllPossibleActions(List<EnemyAction> list, EnemyAI basePlayer, IExecutor executor)
            {
                if (!(executor is Unit unit)) return;

                var moveCost = unit.CurrentActionPoints - unit.MovePointsModifier.Modify(unit.CurrentActionPoints);
                var possibleDistance = unit.CurrentActionPoints / moveCost + 1;
                var nearbyNodes = 
                    Graph.GetNodesInRange(unit.Node, (uint)possibleDistance, unit);
               
                foreach (var currentNode in nearbyNodes)   
                {
                    if(currentNode == unit.Node) continue;
                    if(currentNode.Owner == basePlayer) continue;
                    list.Add(new MoveToForeignNodeAction(basePlayer, executor, currentNode, this));
                }
            }
        }

        public class MoveToForeignNodeAction : EnemyAction
        {
            private readonly List<Node> path;
            private readonly Unit unit;
            private readonly Node targetNode;
            private readonly MoveToForeignNodeActionData data;
            public MoveToForeignNodeAction(EnemyAI basePlayer, IExecutor executor, 
                Node targetNode, MoveToForeignNodeActionData data) 
                : base(basePlayer, executor)
            {
                if (executor is not Unit unit)
                {
                    Debug.LogError($"{executor} is not a Unit!");
                    return;
                }

                this.unit = unit;
                this.targetNode = targetNode;
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
                        
                        if (!unit.CanMoveTo(node))
                        {
                            Debug.LogError($"{unit} cannot move to {node}");
                        }
                        
                        UnitsController.ExecuteCommand(new MoveCommand(unit, unit.Node, node));
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
                var moveCost = 0 - unit.MovePointsModifier.Modify(0);
                var pointsLeft = unit.CurrentActionPoints - moveCost * (path.Count - 1);
                return data.ForeignBaseScore
                       + pointsLeft * data.ForeignScorePerPoint;
            }

            private float WhiteGetScore()
            {
                var moveCost = 0 - unit.MovePointsModifier.Modify(0);
                var pointsLeft = unit.CurrentActionPoints - moveCost * (path.Count - 1);
                return data.WhiteBaseScore
                       + pointsLeft * data.WhiteScorePerPoint;
            }
        }
    }

