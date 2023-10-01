using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class MapCaptureScoreReferee: ScoreReferee
    {
        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);
            AssignNodes();


            foreach (var node in MonoGraph.Instance.Nodes)
            {
                node.OwnerChanged += (before, after) =>
                {
                    var nodeScore = node.GetComponent<NodeScore>().Score;
                    if (before != null)
                        SetScoreForPlayer(before, GetScoreForPlayer(before) - nodeScore);
                    if (after != null)
                        SetScoreForPlayer(after, GetScoreForPlayer(after) + nodeScore);
                };
            }
        }
        
        
        private void AssignNodes()
        {
            foreach (var node in MonoGraph.Instance.Nodes)
            {
                if (!node.TryGetComponent<NodeScore>(out var nodeScore))
                {
                    Debug.LogWarning($"Not component {nameof(NodeScore)} on {node.name}");
                    node.gameObject.AddComponent<NodeScore>();
                }
            }
        }
    }
}