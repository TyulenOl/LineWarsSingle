using System.Collections.Generic;
using LineWars.Model;
using UnityEngine;

namespace LineWars
{
    public class CaptureThePointsGameReferee : ScoreReferee
    {
        [SerializeField] private List<Node> capturePoints;
        public IReadOnlyList<Node> CapturePoints => capturePoints;

        public override void Initialize(Player me, IEnumerable<BasePlayer> enemies)
        {
            base.Initialize(me, enemies);

            if(capturePoints.Count <= 0)
            {
                Debug.LogWarning("Точки захвата не были назначены!");
            }

            foreach(var node in capturePoints)
            {
                node.Replenished += () =>
                {
                    if (node.Owner != null)
                    {
                        SetScoreForPlayer(
                            node.Owner,
                            GetScoreForPlayer(node.Owner)
                            + node.GetComponent<NodeScore>().Score);
                    }
                };
            }
        }
    }
}