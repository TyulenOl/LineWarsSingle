﻿using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    /// <summary>
    /// Захвати n точек
    /// </summary>
    public class MapCaptureScoreReferee: ScoreReferee
    {
        public override void Initialize(Player player, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(player, enemies);
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

            foreach (var node in MonoGraph.Instance.Nodes)
            {
                var nodeScore = node.GetComponent<NodeScore>().Score;
                if (node.Owner != null)
                    SetScoreForPlayer(node.Owner, GetScoreForPlayer(node.Owner) + nodeScore);
            }
        } 
        
        private void AssignNodes()
        {
            foreach (var node in MonoGraph.Instance.Nodes)
            {
                if (!node.TryGetComponent<NodeScore>(out var nodeScore))
                {
                    Debug.LogWarning($"Not component {nameof(NodeScore)} on {node.name}");
                    node.gameObject.AddComponent<NodeScore>().Score = 1;
                }
            }
        }
    }
}